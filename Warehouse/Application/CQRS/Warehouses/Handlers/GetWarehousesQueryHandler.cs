using Application.CQRS.Warehouses.Queries;
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

namespace Application.CQRS.Warehouses.Handlers
{
    public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, Response<PagedResponseDto<WarehouseDto>>>
    {
        private readonly IGenericRepository<Warehouse> _repository;
        private readonly IMapper _mapper;

        public GetWarehousesQueryHandler(IGenericRepository<Warehouse> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<PagedResponseDto<WarehouseDto>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();
            var total = all.Count();

            var paged = all
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mapped = _mapper.Map<IEnumerable<WarehouseDto>>(paged);
            var pagedDto = new PagedResponseDto<WarehouseDto>(mapped, total);

            return Response<PagedResponseDto<WarehouseDto>>.SuccessResponse(pagedDto);
        }

    }
}
