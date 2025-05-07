using Application.CQRS.WarehouseItems.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class DeleteWarehouseItemCommandValidator : AbstractValidator<DeleteWarehouseItemCommand>
    {
        public DeleteWarehouseItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid item ID.");
        }
    }
}
