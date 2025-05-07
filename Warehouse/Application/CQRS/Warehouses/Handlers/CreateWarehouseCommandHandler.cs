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
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Response<string>>
    {
        private readonly IGenericRepository<Warehouse> _repository;
        private readonly IMapper _mapper;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public CreateWarehouseCommandHandler(IGenericRepository<Warehouse> repository, IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var exists = (await _repository.FindAsync(w => w.Name == request.Dto.Name)).Any();
            if (exists)
                return Response<string>.Fail("Warehouse name must be unique.");

            var warehouse = _mapper.Map<Warehouse>(request.Dto);
            await _repository.AddAsync(warehouse);           
            await _repository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Create",
            "Warehouse",
            warehouse.Id.ToString(),
            after: warehouse
        );

            return Response<string>.SuccessResponse("Warehouse created successfully.");
        }
    }
}
