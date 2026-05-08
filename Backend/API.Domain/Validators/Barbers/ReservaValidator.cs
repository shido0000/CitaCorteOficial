using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class ReservaValidator : AbstractValidator<Reserva>
    {
        private readonly IUnitOfWork<Reserva> _repositorios;

        public ReservaValidator(IUnitOfWork<Reserva> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.ClienteId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Fecha).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Hora).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.ServicioId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
