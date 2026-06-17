using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class SystemSettingsService
    {

        private readonly ISystemSettingsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SystemSettingsService> _logger;

        public SystemSettingsService(ISystemSettingsRepository repository,IMapper mapper, ILogger<SystemSettingsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(SystemSettingDto dto)
        {
            var isExists = await _repository.IsExists(dto.SettingKey);

            if (isExists)
            {
                _logger.LogWarning("Setting Key Already Exists for Key:{SettingKey} ", dto.SettingKey);
                return OperationResult<int>.BadRequest("Setting Key Already Exists");
            }

            var setting = _mapper.Map<SystemSettings>(dto);

            var id = await _repository.Add(setting);

            _logger.LogInformation("Setting Key Add Successfully");
            return OperationResult<int>.Success(id);
        }

        public async Task<OperationResult<bool>> Update(SystemSettingDto dto)
        {
            if (dto.Id <= 0)
            {
                _logger.LogWarning("Failed to update SystemSetting. Reason: Invalid Id ({SystemSettingId}).", dto.Id);
                return OperationResult<bool>.BadRequest("Invalid id");
            }

            var setting = await _repository.GetById(dto.Id);

            if (setting is null)
            {
                _logger.LogWarning("Setting Key Not Found for id:{id} ", dto.Id);
                return OperationResult<bool>.NotFound("Setting Not Found");
            }

            _mapper.Map(dto, setting);

            var result = await _repository.Update(setting);

            if(!result)
            {
                _logger.LogWarning("Setting Key Updateing Failed for id:{id} ", dto.Id);
                return OperationResult<bool>.BadRequest("Updateing Failed");
            }

            _logger.LogInformation("Setting Key Updateing Successfully for id:{id} ", dto.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var setting = await _repository.GetById(id);

            if (setting is null)
            {
                _logger.LogWarning("Setting Key Not Found for id:{id} ", id);
                return OperationResult<bool>.NotFound("Setting Not Found");
            }

            var result = await _repository.Delete(id);

            if (!result)
            {
                _logger.LogError("Setting Key Delete for id:{id} Failed", id);
                return OperationResult<bool>.Failure("Delete Failed Try Again Later");
            }

            _logger.LogInformation("Setting Key Delete for id:{id} Successfully", id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<SystemSettingDto>> GetSystemSettingsByKey(string Key)
        {
            var setting = await _repository.GetByKey(Key);

            if (setting is null)
            {
                _logger.LogWarning("Setting Key Not Found for Key:{Key} ", Key);
                return OperationResult<SystemSettingDto>.NotFound("Setting Not Found");
            }

            var SystemSettingDto = _mapper.Map<SystemSettingDto>(setting);

            _logger.LogInformation("Setting Key Get by Key:{Key} Successfully", Key);
            return OperationResult<SystemSettingDto>.Success(SystemSettingDto);
        }

        public async Task<OperationResult<List<SystemSettings>>> GetAllSystemSettings()
        {

            var data = await _repository.GetAllSystemSettings();

            _logger.LogInformation("GetAllSystemSettings Successfully");
            return OperationResult<List<SystemSettings>>.Success(data);
        }
    }




}
