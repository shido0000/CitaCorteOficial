namespace API.Data.Dtos.HomeDto
{
    public class DatosBarberiasBarberosHomeDto
    {
        public required string NombreBarberia { get; set; }
        public required decimal Calificacion { get; set; }
        public required string Direccion { get; set; }
        public required string HorarioApertura { get; set; }
        public required string HorarioCierre { get; set; }
        public required string Telefono { get; set; }
        public required string LogoUrl { get; set; }
    }
}
