using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class ServicioValidator : AbstractValidator<Servicio>
    {
        private readonly IUnitOfWork<Servicio> _repositorios;

        public ServicioValidator(IUnitOfWork<Servicio> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.Precio).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.TiempoDemora).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(150).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Descripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(255).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
