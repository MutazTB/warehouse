using Application.CQRS.WarehouseItems.Queries;
using Application.DTOs;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WarehouseItems.Handlers
{
    public class GetWarehouseItemsQueryHandler : IRequestHandler<GetWarehouseItemsQuery, Response<PagedResponseDto<WarehouseItemDto>>>
    {
        private readonly IGenericRepository<WarehouseItem> _repo;
        private readonly IMapper _mapper;

        public GetWarehouseItemsQueryHandler(IGenericRepository<WarehouseItem> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Response<PagedResponseDto<WarehouseItemDto>>> Handle(GetWarehouseItemsQuery request, CancellationToken cancellationToken)
        {
            var all = await _repo.GetAllAsync();
            var total = all.Count();

            var paged = all
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = paged.Select(i => new WarehouseItemDto
            {
                Id = i.Id,
                ItemName = i.ItemName,
                SKUCode = i.SKUCode,
                Qty = i.Qty,
                CostPrice = i.CostPrice,
                MSRPPrice = i.MSRPPrice,
                WarehouseName = i.Warehouse?.Name ?? "Unknown"
            });

            var pagedResult = new PagedResponseDto<WarehouseItemDto>(result, total);
            return Response<PagedResponseDto<WarehouseItemDto>>.SuccessResponse(pagedResult);
        }
    }
}
