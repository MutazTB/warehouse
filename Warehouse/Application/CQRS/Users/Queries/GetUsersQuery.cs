using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Queries
{
    public class GetUsersQuery : IRequest<Response<PagedResponseDto<UserDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetUsersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

}
