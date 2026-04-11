using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //public async Task<OperationResult<int>> AddUser(UserDto userDto)
        //{
        //    var user = _mapper.Map<User>(userDto);
        //    var newId = await _userRepository.AddUser(user);

        //    if (newId == 0)
        //    {
        //        _logger.LogError("Failed to add User to the database");
        //        return OperationResult<int>.Failure();
        //    }

        //    _logger.LogInformation("Create User: {Id} Successfully", newId);
        //    return OperationResult<int>.Success(newId);
        //}

        public async Task<OperationResult<UserDto>> GetUserById(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", userId);
                return OperationResult<UserDto>.BadRequest("Invalid data");
            }

            var user = await _userRepository.GetUser(userId);

            if (user == null)
            {
                _logger.LogWarning("User With id: {Id} Not Found Or Deleted", userId);
                return OperationResult<UserDto>.NotFound("User not found");
            }

            _logger.LogInformation("Result Id: {Id} Get Successfully", userId);
            return OperationResult<UserDto>.Success(_mapper.Map<UserDto>(user));
        }

        public async Task<OperationResult<bool>> UpdateUser(UserDto userDto)
        {
            if (userDto.Id <= 0)
            {
                _logger.LogWarning("Update User {Id} Failed - User Enter Id Under 1", userDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id");
            }

            var existingUser = await _userRepository.GetUser(userDto.Id);

            if (existingUser == null)
            {
                _logger.LogWarning("Update User {Id} Failed - Record not found", userDto.Id);
                return OperationResult<bool>.NotFound("Not found");
            }

            _mapper.Map(userDto, existingUser);
            var result = await _userRepository.UpdateUser(existingUser);

            if (!result)
            {
                _logger.LogError("Update User With id: {Id} Failed", userDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update User {Id} Successfully", userDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", userId);
                return OperationResult<bool>.BadRequest("Invalid data");
            }

            var isExists = await _userRepository.IsUserExists(userId);

            if (!isExists)
            {
                _logger.LogWarning("User With Id: {Id} Not Found. Deleted Failed", userId);
                return OperationResult<bool>.NotFound("Not Found");
            }

            var result = await _userRepository.DeleteUser(userId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting User with Id {Id}.", userId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete UserId: {Id} Successfully", userId);
            return OperationResult<bool>.Success(result);
        }
    }
}