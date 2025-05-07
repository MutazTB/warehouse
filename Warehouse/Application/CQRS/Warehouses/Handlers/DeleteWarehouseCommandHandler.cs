using Application.CQRS.Warehouses.Commands;
using Application.Responses;
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
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, Response<string>>
    {
        private readonly IGenericRepository<Warehouse> _repository;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public DeleteWarehouseCommandHandler(IGenericRepository<Warehouse> repository, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.Id);
            if (warehouse == null)
                return Response<string>.Fail("Warehouse not found.");

            if (warehouse.Items != null && warehouse.Items.Any())
                return Response<string>.Fail("Cannot delete warehouse because it contains items.");

            _repository.Delete(warehouse);
            await _repository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Delete",
            "Warehouse",
            request.Id.ToString(),
            before: warehouse
        );

            return Response<string>.SuccessResponse("Warehouse deleted successfully.");
        }
    }
}
