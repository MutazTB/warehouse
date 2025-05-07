using Application.CQRS.Dashboard.Queries;
using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Dashboard.Handlers
{
    public class GetWarehouseStatusQueryHandler : IRequestHandler<GetWarehouseStatusQuery, Response<PagedResponseDto<WarehouseStatusDto>>>
    {
        private readonly IGenericRepository<Warehouse> _warehouseRepo;

        public GetWarehouseStatusQueryHandler(IGenericRepository<Warehouse> warehouseRepo)
        {
            _warehouseRepo = warehouseRepo;
        }

        public async Task<Response<PagedResponseDto<WarehouseStatusDto>>> Handle(GetWarehouseStatusQuery request, CancellationToken cancellationToken)
        {
            var all = await _warehouseRepo.GetAllAsync();
            var total = all.Count();

            var paged = all
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = paged.Select(w => new WarehouseStatusDto
            {
                WarehouseId = w.Id,
                WarehouseName = w.Name,
                ItemCount = w.Items?.Count ?? 0
            });

            var pagedResponse = new PagedResponseDto<WarehouseStatusDto>(result, total);
            return Response<PagedResponseDto<WarehouseStatusDto>>.SuccessResponse(pagedResponse);
        }
    }
}
