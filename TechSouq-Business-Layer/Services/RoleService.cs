using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddRole(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var newId = await _roleRepository.AddRole(role);

            if (newId == 0)
            {
                _logger.LogError("Failed to add Role to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Role: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<RoleDto>> GetRoleById(int roleId)
        {
            if (roleId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", roleId);
                return OperationResult<RoleDto>.BadRequest("Invalid data");
            }

            var role = await _roleRepository.GetRole(roleId);

            if (role == null)
            {
                _logger.LogWarning("Role With id: {Id} Not Found Or Deleted", roleId);
                return OperationResult<RoleDto>.NotFound("Role not found");
            }

            _logger.LogInformation("Result Id: {Id} Get Successfully", roleId);
            return OperationResult<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }

        public async Task<OperationResult<bool>> UpdateRole(RoleDto roleDto)
        {
            if (roleDto.Id <= 0)
            {
                _logger.LogWarning("Update Role {Id} Failed - User Enter Id Under 1", roleDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id");
            }

            var existingRole = await _roleRepository.GetRole(roleDto.Id);

            if (existingRole == null)
            {
                _logger.LogWarning("Update Role {Id} Failed - Record not found", roleDto.Id);
                return OperationResult<bool>.NotFound("Not found");
            }

            _mapper.Map(roleDto, existingRole);
            var result = await _roleRepository.UpdateRole(existingRole);

            if (!result)
            {
                _logger.LogError("Update Role With id: {Id} Failed", roleDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update Role {Id} Successfully", roleDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteRole(int roleId)
        {
            if (roleId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", roleId);
                return OperationResult<bool>.BadRequest("Invalid data");
            }

            var isExists = await _roleRepository.IsRoleExists(roleId);

            if (!isExists)
            {
                _logger.LogWarning("Role With Id: {Id} Not Found. Deleted Failed", roleId);
                return OperationResult<bool>.NotFound("Not Found");
            }

            var result = await _roleRepository.DeleteRole(roleId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Role with Id {Id}.", roleId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete RoleId: {Id} Successfully", roleId);
            return OperationResult<bool>.Success(result);
        }
    }
}