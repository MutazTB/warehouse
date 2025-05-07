using Application.CQRS.WarehouseItems.Commands;
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

namespace Application.CQRS.WarehouseItems.Handlers
{
    public class DeleteWarehouseItemCommandHandler : IRequestHandler<DeleteWarehouseItemCommand, Response<string>>
    {
        private readonly IGenericRepository<WarehouseItem> _repo;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public DeleteWarehouseItemCommandHandler(IGenericRepository<WarehouseItem> repo, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _repo = repo;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(DeleteWarehouseItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return Response<string>.Fail("Item not found.");

            _repo.Delete(item);
            await _repo.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Delete",
            "Warehouse",
            request.Id.ToString(),
            before: item
        );

            return Response<string>.SuccessResponse("Warehouse item deleted successfully.");
        }
    }
}
