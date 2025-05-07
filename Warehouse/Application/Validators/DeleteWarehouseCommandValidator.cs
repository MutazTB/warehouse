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
    public class DeleteWarehouseCommandValidator : AbstractValidator<DeleteWarehouseCommand>
    {
        public DeleteWarehouseCommandValidator(IGenericRepository<Warehouse> warehouseRepo)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Warehouse ID must be greater than zero.")
                .MustAsync(async (id, ct) =>
                {
                    var warehouse = await warehouseRepo.GetByIdAsync(id);
                    return warehouse != null;
                }).WithMessage("Warehouse does not exist.")
                .MustAsync(async (id, ct) =>
                {
                    var warehouse = await warehouseRepo.GetByIdAsync(id);
                    return warehouse != null && (warehouse.Items == null || !warehouse.Items.Any());
                }).WithMessage("Cannot delete warehouse because it contains items.");
        }
    }
}

