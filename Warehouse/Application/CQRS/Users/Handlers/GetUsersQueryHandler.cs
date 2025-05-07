using Application.CQRS.Users.Queries;
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

namespace Application.CQRS.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Response<PagedResponseDto<UserDto>>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IGenericRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedResponseDto<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var all = await _userRepository.GetAllAsync();
            var total = all.Count();

            var paged = all
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mapped = _mapper.Map<IEnumerable<UserDto>>(paged);
            var pagedResponse = new PagedResponseDto<UserDto>(mapped, total);

            return Response<PagedResponseDto<UserDto>>.SuccessResponse(pagedResponse);
        }
    }
}