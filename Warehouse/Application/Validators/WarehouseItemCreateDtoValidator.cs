using Application.DTOs;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class WarehouseItemCreateDtoValidator : AbstractValidator<WarehouseItemCreateDto>
    {
        public WarehouseItemCreateDtoValidator(IGenericRepository<Warehouse> warehouseRepo)
        {
            RuleFor(x => x.ItemName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.SKUCode).NotEmpty();
            RuleFor(x => x.Qty).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.MSRPPrice).GreaterThanOrEqualTo(0).When(x => x.MSRPPrice.HasValue);

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0)
                .MustAsync(async (id, ct) =>
                {
                    var exists = await warehouseRepo.GetByIdAsync(id);
                    return exists != null;
                }).WithMessage("Warehouse does not exist.");
        }
    }
}
