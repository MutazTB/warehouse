using Application.CQRS.Warehouses.Commands;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services.Audit;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Warehouses.Handlers
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, Response<string>>
    {
        private readonly IGenericRepository<Warehouse> _repository;
        private readonly IMapper _mapper;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public UpdateWarehouseCommandHandler(IGenericRepository<Warehouse> repository, IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.Id);
            if (warehouse == null)
                return Response<string>.Fail("Warehouse not found.", 404);
            _mapper.Map(request.Dto, warehouse);

            _repository.Update(warehouse);
            await _repository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Update",
            "Warehouse",
            request.Id.ToString(),
            before: request,
            after: warehouse
        );

            return Response<string>.SuccessResponse("Warehouse updated successfully.");
        }
    }
}
