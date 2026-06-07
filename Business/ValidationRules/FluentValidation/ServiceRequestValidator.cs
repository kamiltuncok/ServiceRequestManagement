using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ServiceRequestValidator : AbstractValidator<ServiceRequest>
    {
        public ServiceRequestValidator()
        {
            RuleFor(s => s.CustomerName)
                .NotEmpty().WithMessage("Müşteri adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Müşteri adı 100 karakteri geçemez.");

            RuleFor(s => s.DeviceName)
                .NotEmpty().WithMessage("Cihaz adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Cihaz adı 100 karakteri geçemez.");

            RuleFor(s => s.Description)
                .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Açıklama 500 karakteri geçemez.");
                
        }
    }
}
