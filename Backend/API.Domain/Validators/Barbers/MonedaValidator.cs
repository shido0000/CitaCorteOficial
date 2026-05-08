using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class MonedaValidator : AbstractValidator<Moneda>
    {
        private readonly IUnitOfWork<Moneda> _repositorios;

        public MonedaValidator(IUnitOfWork<Moneda> repositorios)
        {
            _repositorios = repositorios;



            RuleFor(m => m.TasaEnMN).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                             .NotNull().WithMessage("Es un campo obligatorio.");


            RuleFor(m => m.Codigo).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(150).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Descripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(255).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");


            RuleFor(m => m).MustAsync(async (Moneda, cancelacion) => await _repositorios.Monedas.AnyAsync(e => e.Id != Moneda.Id && e.Codigo == Moneda.Codigo))
                                 .WithMessage("Ya existe una Moneda con ese código.");
            RuleFor(m => m).MustAsync(async (Moneda, cancelacion) => await _repositorios.Monedas.AnyAsync(e => e.Id != Moneda.Id && e.Descripcion == Moneda.Descripcion))
                                 .WithMessage("Ya existe una Moneda con esa descripció.");


        }

    }
}
