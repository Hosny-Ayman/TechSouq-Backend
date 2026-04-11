using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger,IMapper mapper, TokenService tokenService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
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

     
        public async Task<OperationResult<TokenDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email,true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                _logger.LogWarning("Login failed for email: {Email}.", loginDto.Email);
                return OperationResult<TokenDto>.BadRequest("Invalid Email or Password.");
            }

        
            var accessToken = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var IsUpdated = await _userRepository.UpdateUser(user);

            if(!IsUpdated)
            {
                _logger.LogError("Update User With id: {Id} Failed", user.Id);
                return OperationResult<TokenDto>.Failure();
            }

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);

            return OperationResult<TokenDto>.Success(tokenDto, "Login successful.");

         
        }

        public async Task<OperationResult<TokenDto>> RefreshToken(TokenDto tokenDto)
        {
           
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);

                var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                                  ?? principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

                int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            if(userId <= 0)
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

        public async Task <OperationResult<bool>> Logout(int userId)
        {
            var user = await _userRepository.GetUser(userId,true);

            if (user == null)
            {
                _logger.LogWarning("User not found. with id: {UserId}", userId);

                return OperationResult<bool>.BadRequest("User not found.");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);

            var result = await _userRepository.UpdateUser(user);

            if(!result)
            {
                _logger.LogError("Update User With id: {Id} Failed", user.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("User LogOut Successfully with id: {UserId}", user.Id);
            return OperationResult<bool>.Success(result);
        }
    }
}