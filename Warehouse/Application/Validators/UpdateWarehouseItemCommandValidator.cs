using Application.CQRS.WarehouseItems.Commands;
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
    public class UpdateWarehouseItemCommandValidator : AbstractValidator<UpdateWarehouseItemCommand>
    {
        public UpdateWarehouseItemCommandValidator(IGenericRepository<Warehouse> warehouseRepo)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid item ID.");

            RuleFor(x => x.Dto).NotNull();

            When(x => x.Dto != null, () =>
            {
                RuleFor(x => x.Dto.ItemName).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Dto.SKUCode).NotEmpty();
                RuleFor(x => x.Dto.Qty).GreaterThanOrEqualTo(0);
                RuleFor(x => x.Dto.CostPrice).GreaterThanOrEqualTo(0);
                RuleFor(x => x.Dto.MSRPPrice).GreaterThanOrEqualTo(0).When(x => x.Dto.MSRPPrice.HasValue);
                RuleFor(x => x.Dto.WarehouseId)
                    .GreaterThan(0)
                    .MustAsync(async (id, ct) =>
                    {
                        var exists = await warehouseRepo.GetByIdAsync(id);
                        return exists != null;
                    }).WithMessage("Warehouse does not exist.");
            });
        }
    }
}
