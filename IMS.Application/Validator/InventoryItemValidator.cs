using FluentValidation;
using IMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Validator
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemDto>
    {
        public InventoryItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Item name is required")
                .MaximumLength(100).WithMessage("Item name cannot exceed 100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
}
