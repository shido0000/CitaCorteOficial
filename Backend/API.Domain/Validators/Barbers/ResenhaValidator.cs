using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class ResenhaValidator : AbstractValidator<Resenha>
    {
        private readonly IUnitOfWork<Resenha> _repositorios;

        public ResenhaValidator(IUnitOfWork<Resenha> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.UsuarioId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Texto).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
