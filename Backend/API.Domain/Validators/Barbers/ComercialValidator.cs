using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class ComercialValidator : AbstractValidator<Comercial>
    {
        private readonly IUnitOfWork<Comercial> _repositorios;

        public ComercialValidator(IUnitOfWork<Comercial> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.UsuarioId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
