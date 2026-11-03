using Microsoft.VisualBasic;
using SchedulerApp.Model;
using SchedulerApp.sql;
using System.Security.AccessControl;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SchedulerApp.Service
{
    public interface IServices
    {
        Task<List<sys_rol>> GetRols();
        Task<bool> InsertNewUser(int rol, string name, string color);
        Task<List<usuariostareas>> GetListUserTareas();
        Task<bool> DeleteUser(int id);

        Task<bool> DeleteCita(int id);
        Task<bool> InsertCitas(Appointment citas);

        Task<bool> UpdateCitas(Appointment citas);
        Task<Appointment> GetAppointmentByPrevison(PrevisionCitas cita);

        Task<Appointment> GetAppointmentAsyncById(int id);

        Task<List<Appointment>> GetAppointmentsListByMonth(DateTime date);

        Task<List<Appointment>> GetAppointmentsListByMonthWithIdRol(DateTime date, int idRol);


        List<Appointment> GetAppointmentsListByMonthByDate(DateTime ini,DateTime end);

        Task<List<Appointment>> GetAppointmentsListByMonthAndIdUsuario(DateTime date,int idUsuario);

        Task<bool> CheckCitas(DateTime start, DateTime end,int idUsuario);

        Task<DateTime> GetFechaIni(DateTime start, DateTime timestart);

        DateTime SumarHorasLaborales(DateTime fechaInicio, double horasASumar);

        Task<List<Appointment>> GetAppointmentsListByMonthAndIdUsuario(DateTime date, int id, int rol);

        Task<List<usuariostareas>> GetUserByIdRol(int id);

        Task<string> GetRangoTiempos(int id, DateTime date);
    }

    public class Services : IServices
    {
        readonly static string connect = "server=localhost;port=3306;username=root;password=4utosEan5ose;database=autosanjose";

        readonly repository repository = new repository(connect);

        public async Task<List<sys_rol>> GetRols()
        {
            return await repository.GetRolType();
        }

        public async Task<List<usuariostareas>> GetUserByIdRol(int id)
        {
            return await repository.GetUsuariostareasByRolAsync(id);
        }

        public async Task<bool> InsertNewUser(int rol, string name,string color)
        {
            return await repository.CreateUser(name, rol,color);
        }

        public async Task<bool> CheckCitas(DateTime start,DateTime end,int idUsuario)
        {
            return await repository.CheckCitas(start, end,idUsuario);
        }

        public async Task<List<usuariostareas>> GetListUserTareas()
        {
            return await repository.GetUsuariostareasAsync();
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await repository.DeleteUser(id);
        }

        public async Task<bool> DeleteCita(int id)
        {
            return await repository.DeleteCita(id);
        }

        public async Task<bool> InsertCitas(Appointment appointment)
        {
            var citas = GetPrevisionCitasByAppointment(appointment);
            return await repository.InsertCita(citas);
        }

        public async Task<bool> UpdateCitas(Appointment appointment)
        {
            var citas = GetPrevisionCitasByAppointment(appointment);
            return await repository.UpdateCitas(citas);
        }

        public async Task<Appointment> GetAppointmentAsyncById(int id)
        {
            var cita = await repository.GetPrevisionCitasAsyncById(id);
            if(cita != null)
            {
                return await GetAppointmentByPrevison(cita);
            }
            return new Appointment();
        }

        public async Task<List<Appointment>> GetAppointmentsListByMonth(DateTime date)
        {
            var citasList = await repository.GetPrevisionCitasByMoth(date);

            var listAppointment = new List<Appointment>();

            foreach (var item in citasList)
            {
                listAppointment.Add(await GetAppointmentByPrevison(item));
            }

            return listAppointment;
        }
        public async Task<List<Appointment>> GetAppointmentsListByMonthWithIdRol(DateTime date, int idRol)
        {
            var citasList = await repository.GetPrevisionCitasByMothWhitRol(date, idRol);
            var listAppointment = new List<Appointment>();

            foreach (var item in citasList)
            {
                listAppointment.Add(await GetAppointmentByPrevison(item));
            }

            return listAppointment;
        }
        public List<Appointment> GetAppointmentsListByMonthByDate(DateTime ini, DateTime end)
        {
            var citasList =  repository.GetPrevisionCitasByMoth(ini,end);

            var listAppointment = new List<Appointment>();

            foreach (var item in citasList)
            {
                listAppointment.Add(GetAppointmentByPrevisonNotAsync(item));
            }

            return listAppointment;
        }

        public async Task<List<Appointment>> GetAppointmentsListByMonthAndIdUsuario(DateTime date, int id)
        {
            var citasList = await repository.GetPrevisionCitasByMothAndUser(date,id);

            var listAppointment = new List<Appointment>();

            foreach (var item in citasList)
            {
                listAppointment.Add(await GetAppointmentByPrevison(item));
            }

            return listAppointment;
        }

        public async Task<List<Appointment>> GetAppointmentsListByMonthAndIdUsuario(DateTime date, int id,int rol)
        {
            var citasList = await repository.GetPrevisionCitasByMothAndUserRol(date, id,rol);

            var listAppointment = new List<Appointment>();

            foreach (var item in citasList)
            {
                listAppointment.Add(await GetAppointmentByPrevison(item));
            }

            return listAppointment;
        }


        private PrevisionCitas GetPrevisionCitasByAppointment(Appointment appointment)
        {
            PrevisionCitas aux = new();

            if (appointment != null)
            {

                aux.CorreoCliente = appointment.CorreoCliente;
                aux.NombreCliente = appointment.Cliente;
                aux.Descripcion = appointment.Descripcion;
                var hours = appointment.TimeStart.Hour;
                var minutos = appointment.TimeStart.Minute;
                aux.Tiempo = appointment.Tiempo;
                aux.Fecha = appointment.Start.AddHours(hours).AddMinutes(minutos);
                aux.FechaFin = appointment.End;
                aux.idUsuario = appointment.idUsuario;
                aux.Matricula = appointment.Matricula;
                aux.color = appointment.color;
                aux.id = appointment.id;
                aux.Usuario = appointment.Usuario;
            }

            return aux;
        }

        
        // Suma horas laborales a una fecha dada
        public DateTime SumarHorasLaborales(DateTime fechaInicio, double horasASumar)
        {
            HorarioLaboralTurnoPartido aux = new();

            var fecha = aux.SumarHorasLaborales(fechaInicio, horasASumar);
            return fecha;
        }

        // Verificar si el día es fin de semana
        private bool EsFinDeSemana(DateTime fecha)
        {
            return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
        }


        public async Task<DateTime> GetFechaIni(DateTime start, DateTime timestart)
        {
            var hours = timestart.Hour;
            var minutos = timestart.Minute;
            DateTime date = new DateTime();
            date = start.AddHours(hours).AddMinutes(minutos);

            return date;
        }
        public async Task<Appointment> GetAppointmentByPrevison(PrevisionCitas cita) 
        {
            Appointment aux = new();

            if(cita != null)
            {
                aux.Cliente = cita.NombreCliente;
                aux.CorreoCliente = cita.CorreoCliente;
                aux.Descripcion = cita.Descripcion;
                aux.Start = cita.Fecha.Date;
                aux.TimeStart = cita.Fecha;
                aux.End = cita.FechaFin;
                aux.Tiempo = cita.Tiempo;
                aux.idUsuario = cita.idUsuario;
                aux.End = cita.FechaFin;
                aux.Matricula = cita.Matricula;
                aux.id = cita.id;
                aux.color = cita.color;
                aux.StartByTable = cita.Fecha.ToString("dd/MM/yyyy HH:mm");
                aux.EndByTable = cita.FechaFin.ToString("dd/MM/yyyy HH:mm");
                aux.Usuario = cita.Usuario;
                //aux.Text = aux.Usuario + "-" + aux.StartByTable + "-" + aux.EndByTable;
                aux.Text = aux.Matricula + " -Horas:" + aux.Tiempo;
            }
            return aux;
        }

        public async Task<string> GetRangoTiempos(int id, DateTime date)
        {
            var list = await repository.GetRangoTiemposAsync(id, date.Date);

            TimeSpan horarioMañanaInicio = new TimeSpan(8, 0, 0);  // 9:00
            TimeSpan horarioMañanaFin = new TimeSpan(14, 0, 0);    // 13:00

            TimeSpan horarioTardeInicio = new TimeSpan(16, 0, 0);  // 15:00
            TimeSpan horarioTardeFin = new TimeSpan(19, 0, 0);     // 19:00  

            List<Intervalo> ocupados = new List<Intervalo>();
            StringBuilder libres = new StringBuilder();
            
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item.EndTask - item.StartTask < new TimeSpan(9, 0, 0))
                    {

                        ocupados.Add(new Intervalo(item.StartTask.TimeOfDay, item.EndTask.TimeOfDay));
                    }
                    else
                    {
                        return "NO TIENE LIBRE HOY";
                    }
                }

                List<Intervalo> libresMañana = await CalcularHorasLibres(horarioMañanaInicio, horarioMañanaFin, ocupados);
                // Calculamos las horas libres en la tarde
                List<Intervalo> libresTarde = await CalcularHorasLibres(horarioTardeInicio, horarioTardeFin, ocupados);

                foreach (var item in libresMañana)
                {
                    libres.AppendLine(item.ToString());
                }
                foreach (var item in libresTarde) 
                { 
                    libres.AppendLine(item.ToString());
                }
            }

            return libres.ToString();
        }


        public async Task<List<Intervalo>> CalcularHorasLibres(TimeSpan inicioHorario, TimeSpan finHorario, List<Intervalo> ocupados)
        {
            List<Intervalo> libres = new List<Intervalo>();

            // Filtramos los intervalos ocupados que caen dentro del horario
            var ocupadosEnHorario = ocupados
                .Where(i => i.Inicio < finHorario && i.Fin > inicioHorario)
                .OrderBy(i => i.Inicio)
                .ToList();

            // Establecemos el tiempo disponible desde el inicio del horario
            TimeSpan disponibleDesde = inicioHorario;

            // Recorremos los intervalos ocupados para determinar los huecos libres
            foreach (var ocupado in ocupadosEnHorario)
            {
                // Ajustamos el intervalo ocupado a los límites del horario
                TimeSpan inicioOcupado = ocupado.Inicio < inicioHorario ? inicioHorario : ocupado.Inicio;
                TimeSpan finOcupado = ocupado.Fin > finHorario ? finHorario : ocupado.Fin;

                // Si hay un espacio libre antes del intervalo ocupado
                if (disponibleDesde < inicioOcupado)
                {
                    libres.Add(new Intervalo(disponibleDesde, inicioOcupado));
                }

                // Actualizamos el tiempo disponible al final del intervalo ocupado
                disponibleDesde = finOcupado;
            }

            // Si queda tiempo libre después del último intervalo ocupado
            if (disponibleDesde < finHorario)
            {
                libres.Add(new Intervalo(disponibleDesde, finHorario));
            }

            return libres;
        }


        public Appointment GetAppointmentByPrevisonNotAsync(PrevisionCitas cita)
        {
            Appointment aux = new();

            if (cita != null)
            {
                aux.Cliente = cita.NombreCliente;
                aux.CorreoCliente = cita.CorreoCliente;
                aux.Descripcion = cita.Descripcion;
                aux.Start = cita.Fecha.Date;
                aux.TimeStart = cita.Fecha;
                aux.Tiempo = cita.Tiempo;
                aux.idUsuario = cita.idUsuario;
                aux.Usuario = cita.Usuario;
                aux.End = SumarHorasLaborales(cita.Fecha,cita.Tiempo);
                aux.Matricula = cita.Matricula;
                aux.id = cita.id;
                aux.color = cita.color;
                aux.StartByTable = cita.Fecha.ToString("dd/MM/yyyy HH:mm");
                aux.EndByTable = cita.FechaFin.ToString("dd/MM/yyyy HH:mm");
                aux.Text = aux.Usuario + "-" + aux.StartByTable + "-" + aux.EndByTable;
                aux.Text = aux.Matricula;
            }
            return aux;
        }

    }
}
