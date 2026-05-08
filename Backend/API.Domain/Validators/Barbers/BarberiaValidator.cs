using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class BarberiaValidator : AbstractValidator<Barberia>
    {
        private readonly IUnitOfWork<Barberia> _repositorios;

        public BarberiaValidator(IUnitOfWork<Barberia> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.UsuarioId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.SuscripcionId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.EstadoSuscripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.HorarioApertura).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                         .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.HorarioCierre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                         .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(150).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Direccion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(255).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");


            RuleFor(m => m).MustAsync(async (Barberia, cancelacion) => await _repositorios.Barberias.AnyAsync(e => e.Id != Barberia.Id && e.Nombre == Barberia.Nombre))
                                 .WithMessage("Ya existe una Barbería con ese nombre.");


        }

    }
}
