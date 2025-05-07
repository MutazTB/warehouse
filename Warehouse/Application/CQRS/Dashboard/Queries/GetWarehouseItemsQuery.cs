using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Dashboard.Queries
{
    public class GetWarehouseItemsQuery : IRequest<Response<PagedResponseDto<WarehouseItemDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortDirection { get; set; } = "desc"; // "asc" or "desc"

        public GetWarehouseItemsQuery(int pageNumber, int pageSize, string sortDirection)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortDirection = sortDirection.ToLower();
        }
    }
}
