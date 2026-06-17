using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.interfaces;
using TechSouq.Application.Queries;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class DashboardService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardService> _logger;
        private readonly IDashboardQuery _boardQueryService;

        public DashboardService(IOrderRepository orderRepository, IMapper mapper, ILogger<DashboardService> logger,
            ICartRepository cartRepository, IOrderQuery orderQueryService, IDashboardQuery boardQueryService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _boardQueryService = boardQueryService;
        }

        public async Task<OperationResult<PagedResponse<RecentOrderDto>>> RecentSales(IRecentSaleceQueryParams queryParams)
        {
            var data = await _boardQueryService.RecentSales(queryParams);

            _logger.LogInformation("Result Get RecentSales Successfully");
            return OperationResult<PagedResponse<RecentOrderDto>>.Success(data);
        }

        public async Task<OperationResult<DashboardDto>> ShowDashboardInfo()
        {
            var data = await _boardQueryService.ShowDashboardInfo();

            if (data == null)
            {
                _logger.LogWarning("ShowDashboardInfo failed");
                return OperationResult<DashboardDto>.NotFound($"failed or not found");
            }

            _logger.LogInformation("Result Get ShowDashboardInfo Successfully");
            return OperationResult<DashboardDto>.Success(data);
        }

        public async Task<OperationResult<List<decimal>>> SalesLast7Days()
        {
            var data = await _boardQueryService.SalesLast7Days();

            if(data == null)
            {
                _logger.LogWarning("SalesLast7Days failed");
                return OperationResult<List<decimal>>.NotFound($"failed or not found");
            }

            _logger.LogInformation("Result Get SalesLast7Days Successfully");
            return OperationResult<List<decimal>>.Success(data);
        }

        public async Task<OperationResult<List<BestSellingProductsDto>>> BestSellingProducts()
        {
            var data = await _boardQueryService.BestSellingProducts();

            if (data == null)
            {
                _logger.LogWarning("BestSellingProducts failed");
                return OperationResult<List<BestSellingProductsDto>>.NotFound($"failed or not found");
            }

            _logger.LogInformation("Result Get BestSellingProducts Successfully");
            return OperationResult<List<BestSellingProductsDto>>.Success(data);
        }



    }
}
