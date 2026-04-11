using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class CategorieService
    {
        private readonly ICategorieRepository _categorieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategorieService> _logger;

        public CategorieService(ICategorieRepository categorieRepository, IMapper mapper, ILogger<CategorieService> logger)
        {
            _categorieRepository = categorieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> CreateCategorie(CategorieDto categorieDto)
        {
            var categorie = _mapper.Map<Categorie>(categorieDto);
            var newId = await _categorieRepository.AddCategorie(categorie);

            if (newId == 0)
            {
                _logger.LogError("Failed to add Categorie to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Categorie: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<CategorieDto>> GetCategorie(int categorieId)
        {
            if (categorieId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", categorieId);
                return OperationResult<CategorieDto>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {categorieId}" });
            }

            var categorie = await _categorieRepository.GetCategorie(categorieId);

            if (categorie == null)
            {
                _logger.LogWarning("Result Id: {Id} Get Failed", categorieId);
                return OperationResult<CategorieDto>.NotFound("Categorie not found or already deleted.");
            }

            var categorieDto = _mapper.Map<CategorieDto>(categorie);

            _logger.LogInformation("Result Id: {Id} Get Successfully", categorieId);
            return OperationResult<CategorieDto>.Success(categorieDto);
        }

        public async Task<OperationResult<bool>> UpdateCategorie(CategorieDto categorieDto)
        {
            if (categorieDto.Id <= 0)
            {
                _logger.LogWarning("Update Categorie {CategorieId} Failed - User Enter Id Under 1", categorieDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {categorieDto.Id}" });
            }

            var categorie = await _categorieRepository.GetCategorie(categorieDto.Id);

            if (categorie == null)
            {
                _logger.LogWarning("Categorie id: {Id} Not Found", categorieDto.Id);
                return OperationResult<bool>.NotFound($"Categorie id: {categorieDto.Id} Not Found"); 
            }

            _mapper.Map(categorieDto, categorie);

            var result = await _categorieRepository.UpdateCategorie(categorie);

            if (!result)
            {
                _logger.LogError("Update Categorie With id: {Id} Failed", categorieDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update Categorie With Id {Id} Successfully", categorie.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> DeleteCategorie(int categorieId)
        {
            if (categorieId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", categorieId);
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {categorieId}" });
            }

            var isExists = await _categorieRepository.IsCategorieExists(categorieId);

            if (!isExists)
            {
                _logger.LogWarning("Categorie With Id: {CategorieId} Not Found. Deleted Failed", categorieId);
                return OperationResult<bool>.NotFound($"Categorie With Id: {categorieId} Not Found");
            }

            var result = await _categorieRepository.DeleteCategorie(categorieId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting categorie with Id {CategorieId}.", categorieId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete Categorie {CategorieId} Successfully", categorieId);
            return OperationResult<bool>.Success(result);
        }
    }
}