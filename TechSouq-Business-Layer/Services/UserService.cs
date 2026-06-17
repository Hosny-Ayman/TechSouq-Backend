using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomersQuery _CustomersQuery;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, ICustomersQuery CustomersQuery)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _CustomersQuery = CustomersQuery;
        }

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

        public async Task<OperationResult<PagedResponse<CustomerSummaryDto>>> GetAllCustomersPaged(int pageNumber, int pageSize, string? EmailSearch)
        {
            if(pageNumber<1 || pageSize<1)
            {
                _logger.LogWarning("Invalid data Pagenumber:{pageNumber} or pageSize:{pageSize}", pageNumber, pageSize);
                return OperationResult<PagedResponse<CustomerSummaryDto>>.BadRequest("Invalid data");
            }

            var data = await _CustomersQuery.GetAllCustomersPaged(pageNumber, pageSize, EmailSearch);

            _logger.LogInformation("Get All Customers Paged Successfully");
            return OperationResult<PagedResponse<CustomerSummaryDto>>.Success(data);

        }

        public async Task<OperationResult<CustomerDetailsDto>> GetCustomerDetails(int customerId)
        {
            if(customerId<1)
            {
                _logger.LogWarning("Invalid data customerId:{customerId}", customerId);
                return OperationResult<CustomerDetailsDto>.BadRequest("Invalid data");
            }

            var data = await _CustomersQuery.GetCustomerDetails(customerId);

            if(data==null)
            {
                _logger.LogWarning("Get Customer Details customerId:{customerId} Failed - Record not found", customerId);
                return OperationResult<CustomerDetailsDto>.NotFound($" customer with customerId:{customerId} Not found or Deleted ");
            }

            _logger.LogInformation("GetCustomerDetails Successfully");
            return OperationResult<CustomerDetailsDto>.Success(data);
        }


        public async Task<OperationResult<bool>> IsActive(int customerId)
        {
            if (customerId < 1)
            {
                _logger.LogWarning("Invalid data customerId:{customerId}", customerId);
                return OperationResult<bool>.BadRequest("Invalid data");
            }

            var IsChanged = await _CustomersQuery.SetActive(customerId);

            if (IsChanged == false)
            {
                _logger.LogWarning("Change Active customerId:{customerId} Failed - Record not found", customerId);
                return OperationResult<bool>.NotFound($" customer with customerId:{customerId} Not found or Deleted ");
            }

            _logger.LogInformation("GetCustomerDetails Successfully");
            return OperationResult<bool>.Success(IsChanged);
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

            var oldPass = existingUser.Password;
            var currentRoleId = existingUser.RoleId;

            _mapper.Map(userDto, existingUser);

            existingUser.RoleId = currentRoleId;

            if (!string.IsNullOrEmpty(userDto.OldPassword) && !string.IsNullOrEmpty(userDto.NewPassword))
            {
                if (!BCrypt.Net.BCrypt.Verify(userDto.OldPassword, oldPass))
                {
                    _logger.LogWarning("Update User {Id} Failed - User Enter Invalid OldPassword", userDto.Id);
                    return OperationResult<bool>.BadRequest("Invalid OldPassword");
                }

                var hashedpassword = BCrypt.Net.BCrypt.HashPassword(userDto.NewPassword);
                existingUser.Password = hashedpassword;
            }
            else
            {
                existingUser.Password = oldPass;
            }

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