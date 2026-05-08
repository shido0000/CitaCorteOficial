using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class SolicitudDeSuscripcionValidator : AbstractValidator<SolicitudDeSuscripcion>
    {
        private readonly IUnitOfWork<SolicitudDeSuscripcion> _repositorios;

        public SolicitudDeSuscripcionValidator(IUnitOfWork<SolicitudDeSuscripcion> repositorios)
        {
            _repositorios = repositorios;

           RuleFor(m => m.SuscripcionId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
