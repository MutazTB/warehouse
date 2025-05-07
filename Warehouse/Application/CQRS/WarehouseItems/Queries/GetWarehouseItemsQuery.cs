using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WarehouseItems.Queries
{
    public class GetWarehouseItemsQuery : IRequest<Response<PagedResponseDto<WarehouseItemDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetWarehouseItemsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
