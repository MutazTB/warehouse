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
    public class CreateWarehouseItemCommandHandler : IRequestHandler<CreateWarehouseItemCommand, Response<string>>
    {
        private readonly IGenericRepository<WarehouseItem> _itemRepo;
        private readonly IGenericRepository<Warehouse> _warehouseRepo;
        private readonly IMapper _mapper;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public CreateWarehouseItemCommandHandler(
            IGenericRepository<WarehouseItem> itemRepo,IGenericRepository<Warehouse> warehouseRepo, IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _itemRepo = itemRepo;
            _warehouseRepo = warehouseRepo;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(CreateWarehouseItemCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(request.Dto.WarehouseId);
            if (warehouse == null)
                return Response<string>.Fail("Warehouse not found.");

            var item = _mapper.Map<WarehouseItem>(request.Dto);
            await _itemRepo.AddAsync(item);
            await _itemRepo.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Create",
            "Warehouse Item",
            warehouse.Id.ToString(),
            after: warehouse
        );


            return Response<string>.SuccessResponse("Warehouse item created.");
        }
    }
}
