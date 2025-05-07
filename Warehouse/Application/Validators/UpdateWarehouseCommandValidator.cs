using Application.CQRS.Warehouses.Commands;
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
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator(IGenericRepository<Warehouse> warehouseRepo)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid warehouse ID.")
                .MustAsync(async (id, ct) =>
                {
                    var exists = await warehouseRepo.GetByIdAsync(id);
                    return exists != null;
                }).WithMessage("Warehouse not found.");

            RuleFor(x => x.Dto)
                .NotNull().WithMessage("Warehouse data must be provided.");

            When(x => x.Dto != null, () =>
            {
                RuleFor(x => x.Dto.Name)
                    .NotEmpty().MaximumLength(100)
                    .MustAsync(async (cmd, name, ct) =>
                    {
                        var duplicates = await warehouseRepo.FindAsync(w =>
                            w.Name == name && w.Id != cmd.Id);
                        return !duplicates.Any();
                    }).WithMessage("Warehouse name already exists.");

                RuleFor(x => x.Dto.Address).NotEmpty();
                RuleFor(x => x.Dto.City).NotEmpty();
                RuleFor(x => x.Dto.Country).NotEmpty();
            });
        }
    }
}
