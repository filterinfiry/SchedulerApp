

namespace SchedulerApp.Model
{
    public class Appointment
    {
        public DateTime Start { get; set; }

        public DateTime TimeStart { get; set; }
        public DateTime End { get; set; }

        public string Cliente { get; set; }

        public string Matricula { get; set; }

        public string? CorreoCliente { get; set; }

        public double Tiempo { get; set; }

        public string? Descripcion { get; set; }

        public int idUsuario { get; set; }

        public string Usuario { get; set; }

        public int id { get; set; }

        public string color { get; set; }

        public string StartByTable { get; set; }

        public string EndByTable { get; set; }

        public string Text { get; set; }
    }
}