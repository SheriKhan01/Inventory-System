using FluentValidation;
using IMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Validator
{
    public class SupplierValidator : AbstractValidator<SupplierDto>
    {
        public SupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Supplier name is required")
                .MaximumLength(100).WithMessage("Supplier name cannot exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits");
        }
    }
}
