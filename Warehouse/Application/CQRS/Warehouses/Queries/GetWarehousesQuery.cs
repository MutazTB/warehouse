using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Warehouses.Queries
{
    public class GetWarehousesQuery : IRequest<Response<PagedResponseDto<WarehouseDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetWarehousesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

}
