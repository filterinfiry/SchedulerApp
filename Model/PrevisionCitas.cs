namespace SchedulerApp.Model
{
    public class PrevisionCitas
    {
        public int id { get; set; }

        public int idUsuario { get; set; }

        public string Usuario { get; set; }

        public string? NombreCliente { get; set; }

        public string? CorreoCliente { get; set; }

        public string Matricula {  get; set; }

        public DateTime Fecha { get; set; }
        public DateTime FechaFin { get; set; }

        public double Tiempo { get; set; }

        public string? Descripcion { get; set; }

        public string color { get; set; }
    }
}
