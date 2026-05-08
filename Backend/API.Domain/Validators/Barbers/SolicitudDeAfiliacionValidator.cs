using API.Data.Entidades.Barbers;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Barbers
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class SolicitudDeAfiliacionValidator : AbstractValidator<SolicitudDeAfiliacion>
    {
        private readonly IUnitOfWork<SolicitudDeAfiliacion> _repositorios;

        public SolicitudDeAfiliacionValidator(IUnitOfWork<SolicitudDeAfiliacion> repositorios)
        {
            _repositorios = repositorios;
        }
    }
}
