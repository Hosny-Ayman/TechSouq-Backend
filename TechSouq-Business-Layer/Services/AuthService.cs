using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using Google.Apis.Auth; 

namespace TechSouq.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;
        private readonly IEmailService _EmailService;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, IMapper mapper, TokenService tokenService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _EmailService = emailService;
        }

        public async Task<OperationResult<int>> Register(RegisterDto registerDto)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var newUser = _mapper.Map<User>(registerDto);
            newUser.Password = hashedPassword;

            var newId = await _userRepository.AddUser(newUser);

            if (newId == 0)
            {
                _logger.LogError("Failed to register new user.");
                return OperationResult<int>.Failure("Registration failed.");
            }

            _logger.LogInformation("User registered successfully with ID: {Id}", newId);
            return OperationResult<int>.Success(newId, "User registered successfully.");
        }

        public async Task<OperationResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email, true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                _logger.LogWarning("Login failed for email: {Email}.", loginDto.Email);
                return OperationResult<LoginResponseDto>.BadRequest("Invalid Email or OldPassword.");
            }

            var accessToken = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var IsUpdated = await _userRepository.UpdateUser(user);

            if (!IsUpdated)
            {
                _logger.LogError("Update User With id: {Id} Failed", user.Id);
                return OperationResult<LoginResponseDto>.Failure();
            }

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var userData = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email,
                RoleId = user.RoleId
            };

            var loginResponseDto = new LoginResponseDto
            {
                User = userData,
                Token = tokenDto
            };

            _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);

            return OperationResult<LoginResponseDto>.Success(loginResponseDto, "Login successful.");
        }

        public async Task<OperationResult<TokenDto>> RefreshToken(TokenDto tokenDto)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                              ?? principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            if (userId <= 0)
            {
                return OperationResult<TokenDto>.BadRequest("Invalid client request.");
            }

            var user = await _userRepository.GetUser(userId);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return OperationResult<TokenDto>.BadRequest("Invalid client request.");
            }

            var newAccessToken = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUser(user);

            var newTokenDto = new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return OperationResult<TokenDto>.Success(newTokenDto, "Token refreshed successfully.");
        }

        public async Task<OperationResult<bool>> Logout(int userId)
        {
            var user = await _userRepository.GetUser(userId, true);

            if (user == null)
            {
                _logger.LogWarning("User not found. with id: {UserId}", userId);
                return OperationResult<bool>.BadRequest("User not found.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);

            var result = await _userRepository.UpdateUser(user);

            if (!result)
            {
                _logger.LogError("Update User With id: {Id} Failed", user.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("User LogOut Successfully with id: {UserId}", user.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> LogoutWithToken(string accessToken)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                                  ?? principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    return await Logout(userId); 
                }
            }
            catch
            {
                
            }

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email,true);

            if (user == null)
            {
                return OperationResult<bool>.Success(true);
            }

            var Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

            user.PasswordResetToken = Token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userRepository.UpdateUser(user);

            var resetLink = $"http://localhost:4200/Reset-Password?email={user.Email}&token={Token}";

            await _EmailService.SendEmailAsync(user.Email, "Reset Your Password - TechSouq", $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>");

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email,true);
            if (user == null)
                return OperationResult<bool>.BadRequest("Invalid Request");

            if (user.PasswordResetToken != dto.Token || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return OperationResult<bool>.BadRequest("Token is invalid or expired.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword); 

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateUser(user);

            return OperationResult<bool>.Success(true);
        }


      

public async Task<OperationResult<LoginResponseDto>> GoogleLogin(GoogleLoginDto dto)
    {
        try
        {
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

            var user = await _userRepository.GetUserByEmailAsync(payload.Email, true);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName ?? "User",
                    SecondName = payload.FamilyName ?? "Google",
                    Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString() + "A1@a"),
                    RoleId = 2 
                };

                var newId = await _userRepository.AddUser(user);
                if (newId == 0) return OperationResult<LoginResponseDto>.Failure("Failed to create Google user.");
                user.Id = newId;
            }

            var accessToken = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(user);

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var userData = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email,
                RoleId = user.RoleId
            };

            var loginResponseDto = new LoginResponseDto
            {
                User = userData,
                Token = tokenDto
            };

            _logger.LogInformation("User logged in successfully via Google: {Email}", payload.Email);
            return OperationResult<LoginResponseDto>.Success(loginResponseDto, "Google Login successful.");
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogWarning(ex, "Invalid Google Token.");
            return OperationResult<LoginResponseDto>.BadRequest("Invalid Google authentication token.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google login failed.");
            return OperationResult<LoginResponseDto>.Failure("An error occurred during Google login.");
        }
    }
}
}