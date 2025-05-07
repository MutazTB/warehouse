using Application.CQRS.Audit.Queries;
using Application.DTOs;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services.Audit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Audit.Handlers
{
    public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Response<PagedResponseDto<AuditLogDto>>>
    {
        private readonly IGenericRepository<AuditLog> _repo;
        private readonly IMapper _mapper;

        public GetAuditLogsQueryHandler(IGenericRepository<AuditLog> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Response<PagedResponseDto<AuditLogDto>>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var all = await _repo.GetAllAsync();
            var total = all.Count();

            var paged = all
                .OrderByDescending(a => a.Timestamp)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mapped = _mapper.Map<IEnumerable<AuditLogDto>>(paged);
            return Response<PagedResponseDto<AuditLogDto>>.SuccessResponse(new PagedResponseDto<AuditLogDto>(mapped, total));
        }
    }
}
