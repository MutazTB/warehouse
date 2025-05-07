using Application.CQRS.Dashboard.Queries;
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

namespace Application.CQRS.Dashboard.Handlers
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

            var sorted = request.SortDirection == "asc"
                ? all.OrderBy(i => i.Qty)
                : all.OrderByDescending(i => i.Qty);

            var paged = sorted
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mapped = _mapper.Map<IEnumerable<WarehouseItemDto>>(paged);
            var pagedResponse = new PagedResponseDto<WarehouseItemDto>(mapped, total);

            return Response<PagedResponseDto<WarehouseItemDto>>.SuccessResponse(pagedResponse);
        }
    }
}
