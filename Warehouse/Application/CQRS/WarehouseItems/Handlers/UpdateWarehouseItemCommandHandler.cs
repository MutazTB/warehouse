using Application.CQRS.WarehouseItems.Commands;
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

namespace Application.CQRS.WarehouseItems.Handlers
{
    public class UpdateWarehouseItemCommandHandler : IRequestHandler<UpdateWarehouseItemCommand, Response<string>>
    {
        private readonly IGenericRepository<WarehouseItem> _itemRepo;
        private readonly IGenericRepository<Warehouse> _warehouseRepo;
        private readonly IMapper _mapper;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public UpdateWarehouseItemCommandHandler(
            IGenericRepository<WarehouseItem> itemRepo,IGenericRepository<Warehouse> warehouseRepo,IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _itemRepo = itemRepo;
            _warehouseRepo = warehouseRepo;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(UpdateWarehouseItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepo.GetByIdAsync(request.Id);
            if (item == null)
                return Response<string>.Fail("Item not found.");

            var warehouse = await _warehouseRepo.GetByIdAsync(request.Dto.WarehouseId);
            if (warehouse == null)
                return Response<string>.Fail("Warehouse not found.");

            _mapper.Map(request.Dto, item);
            _itemRepo.Update(item);
            await _itemRepo.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Update",
            "Warehouse",
            request.Id.ToString(),
            before: request,
            after: item
        );

            return Response<string>.SuccessResponse("Warehouse item updated successfully.");
        }
    }
}
