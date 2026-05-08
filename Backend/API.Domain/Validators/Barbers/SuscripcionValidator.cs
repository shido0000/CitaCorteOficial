using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class SuscripcionValidator : AbstractValidator<Suscripcion>
    {
        private readonly IUnitOfWork<Suscripcion> _repositorios;

        public SuscripcionValidator(IUnitOfWork<Suscripcion> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.TipoSuscripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.NivelSuscripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Precio).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.TiempoVigencia).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                                      .NotNull().WithMessage("Es un campo obligatorio.");
           
            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(150).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Descripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(255).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");


            RuleFor(m => m).MustAsync(async (Suscripcion, cancelacion) => await _repositorios.Suscripciones.AnyAsync(e => e.Id != Suscripcion.Id && e.Nombre == Suscripcion.Nombre))
                                 .WithMessage("Ya existe una Suscripción con ese nombre.");
            RuleFor(m => m).MustAsync(async (Suscripcion, cancelacion) => await _repositorios.Suscripciones.AnyAsync(e => e.Id != Suscripcion.Id && e.Descripcion == Suscripcion.Descripcion))
                                 .WithMessage("Ya existe una Suscripción con esa descripción.");


        }

    }
}
