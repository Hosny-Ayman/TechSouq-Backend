using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Customer,Admin")] 
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressesController(AddressService AddressService)
        {
            _addressService = AddressService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddAddress(AddressDto address)
        {
            address.UserId = User.GetUserId();
            var result = await _addressService.AddAddress(address);
            return this.ToHttpResponse(result);
        }

        [HttpGet("MyAddresses")] 
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = User.GetUserId();
            var result = await _addressService.GetAddresses(userId);
            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAddress(AddressDto address)
        {
            address.UserId = User.GetUserId();
            var result = await _addressService.UpdateAddress(address);
            return this.ToHttpResponse(result);
        }

        [HttpDelete("{AddressId}")]
        public async Task<IActionResult> DeleteAddress(int AddressId)
        {
            var userId = User.GetUserId();
           
            var result = await _addressService.DeleteAddress(AddressId, userId);
            return this.ToHttpResponse(result);
        }

        [HttpPut("{AddressId}")]
        public async Task<IActionResult> setAsDefault(int AddressId)
        {
            var userId = User.GetUserId();

            var result = await _addressService.setAsDefaultAsync(AddressId, userId);
            return this.ToHttpResponse(result);
        }

        [HttpGet("{AddressId}")]
        public async Task<IActionResult> GetAddress(int AddressId)
        {
            var userId = User.GetUserId();

            var result = await _addressService.GetAddressAsync(AddressId, userId);
            return this.ToHttpResponse(result);
        }

        [HttpGet("GetOnlyDefaultAddress")]
        public async Task<IActionResult> GetOnlyDefaultAddress()
        {
            var userId = User.GetUserId();
            var result = await _addressService.GetOnlyDefaultAddress(userId);
            return this.ToHttpResponse(result);
        }

        [HttpGet("GetCityShippingCost")]
        public async Task<IActionResult> GetCityShippingCost(string? CityName)
        {
            var userId = User.GetUserId();
            var result = await _addressService.GetCityShippingCost(userId, CityName);
            return this.ToHttpResponse(result);
        }
    }
}