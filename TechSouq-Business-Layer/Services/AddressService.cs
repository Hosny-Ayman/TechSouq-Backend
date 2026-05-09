using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class AddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IAddressRepository addressRepository, IMapper mapper, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddAddress(AddressDto addressDto)
        {
            var howManyAddressUserHave = await _addressRepository.HowManyAddressesHeHaveAsync(addressDto.UserId);

            if(howManyAddressUserHave==5)
            {
                _logger.LogWarning("User has many addresses userId ({UserId}).", addressDto.UserId);
                return OperationResult<int>.BadRequest("Add Address Failed User has many addresses");
            }

            var address = _mapper.Map<Address>(addressDto);
            var newId = await _addressRepository.AddAddress(address);

            if (newId == 0)
            {
                _logger.LogError("Failed to add Address to the database.");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Address with Id: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<List<AddressDto>>> GetAddresses(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Failed to retrieve Addresses. Reason: Invalid userId ({UserId}).", userId);
                return OperationResult<List<AddressDto>>.BadRequest("Invalid Data", new List<string> { $"Invalid userId {userId}" });
            }

            var addresses = await _addressRepository.GetAddresses(userId);

            if (addresses == null || !addresses.Any())
            {
                _logger.LogWarning("Addresses Not Found For UserId: {UserId}", userId);
                return OperationResult<List<AddressDto>>.NotFound("Addresses Not Found");
            }

            var addressesDto = _mapper.Map<List<AddressDto>>(addresses);

            _logger.LogInformation("User {UserId} retrieved Addresses successfully.", userId);
            return OperationResult<List<AddressDto>>.Success(addressesDto);
        }

        public async Task<OperationResult<bool>> UpdateAddress(AddressDto addressDto)
        {
            if (addressDto.Id <= 0)
            {
                _logger.LogWarning("Failed to update Address. Reason: Invalid AddressId ({AddressId}).", addressDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {addressDto.Id}" });
            }

            var address = await _addressRepository.GetAddressById(addressDto.Id, addressDto.UserId);

            if (address == null)
            {
                _logger.LogWarning("Address with Id {Id} Not Found", addressDto.Id);
                return OperationResult<bool>.NotFound($"Address id: {addressDto.Id} not found."); 
            }

            _mapper.Map(addressDto, address);

            var result = await _addressRepository.UpdateAddress(address); 

            if (!result)
            {
                _logger.LogError("Update Address with Id {Id} Failed.", addressDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Address with Id {Id} Updated Successfully", address.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> DeleteAddress(int addressId,int userId)
        {
            if (addressId <= 0 || userId<=0)
            {
                _logger.LogWarning("Delete Address with Invalid AddressId {AddressId} or userId: {userId}", addressId,userId);
                return OperationResult<bool>.BadRequest("Invalid Data");
            }

            

            var isExists = await _addressRepository.IsAddressExists(addressId, userId);

            if (!isExists)
            {
                _logger.LogWarning("Address with Id {AddressId} Not Found. Delete Failed.", addressId);
                return OperationResult<bool>.NotFound($"Address with Id {addressId} Not Found");
            }

            var result = await _addressRepository.DeleteAddress(addressId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Address with Id {AddressId}.", addressId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Address with Id {AddressId} Deleted Successfully", addressId);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> setAsDefaultAsync(int addressId, int userId)
        {
            if (addressId <= 0 || userId <= 0)
            {
                _logger.LogWarning("Delete Address with Invalid AddressId {AddressId} or userId: {userId}", addressId, userId);
                return OperationResult<bool>.BadRequest("Invalid Data");
            }

            var isExists = await _addressRepository.IsAddressExists(addressId, userId);

            if (!isExists)
            {
                _logger.LogWarning("Address with Id {AddressId} Not Found. Delete Failed.", addressId);
                return OperationResult<bool>.NotFound($"Address with Not Found Or Delted");
            }

            var IsSuccess = await _addressRepository.setAsDefaultAsync(addressId, userId);

            if(!IsSuccess)
            {
                _logger.LogError("An unexpected error occurred while  setAsDefault Address with Id {AddressId}.", addressId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Address with Id {AddressId} setAsDefault Successfully", addressId);
            return OperationResult<bool>.Success(IsSuccess);
        }

        public async Task<OperationResult<AddressDto>> GetAddressAsync(int addressId, int userId)
        {
            if (addressId <= 0 || userId <= 0)
            {
                _logger.LogWarning("Delete Address with Invalid AddressId {AddressId} or userId: {userId}", addressId, userId);
                return OperationResult<AddressDto>.BadRequest("Invalid Data");
            }

            var address = await _addressRepository.GetAddressById(addressId, userId);

            if (address == null)
            {
                _logger.LogWarning("Address with Id {AddressId} Not Found. Delete Failed.", addressId);
                return OperationResult<AddressDto>.NotFound($"Address with Not Found Or Delted");
            }

            var addressDto = _mapper.Map<AddressDto>(address);


            _logger.LogInformation("Address with Id {AddressId} setAsDefault Successfully", addressId);
            return OperationResult<AddressDto>.Success(addressDto);
        }

    }
}