using System;

public class HorarioLaboralTurnoPartido
{
    // Definir los turnos del horario laboral
    private readonly TimeSpan inicioTurno1 = new TimeSpan(8, 0, 0);   // 9:00 AM
    private readonly TimeSpan finTurno1 = new TimeSpan(14, 0, 0);     // 1:00 PM
    private readonly TimeSpan inicioTurno2 = new TimeSpan(16, 0, 0);  // 3:00 PM
    private readonly TimeSpan finTurno2 = new TimeSpan(19, 0, 0);     // 7:00 PM

    // Suma horas laborales a una fecha dada
    public DateTime SumarHorasLaborales(DateTime fechaInicio, double horasASumar)
    {
        DateTime fechaActual = fechaInicio;
        double horasRestantes = horasASumar;

        while (horasRestantes > 0)
        {
            // Si el día actual es fin de semana, pasamos al siguiente lunes
            if (EsFinDeSemana(fechaActual))
            {
                fechaActual = fechaActual.AddDays(1).Date;
                continue;
            }

            // Definir el inicio y fin de los turnos del día actual
            DateTime inicioTrabajoTurno1 = fechaActual.Date.Add(inicioTurno1);
            DateTime finTrabajoTurno1 = fechaActual.Date.Add(finTurno1);
            DateTime inicioTrabajoTurno2 = fechaActual.Date.Add(inicioTurno2);
            DateTime finTrabajoTurno2 = fechaActual.Date.Add(finTurno2);

            // Comenzamos calculando el tiempo en el primer turno (si aún estamos en él)
            if (fechaActual < finTrabajoTurno1)
            {
                if (fechaActual < inicioTrabajoTurno1)
                {
                    fechaActual = inicioTrabajoTurno1; // Si es antes del turno 1, mover a inicio
                }

                // Calcular horas disponibles en el primer turno
                TimeSpan horasDisponiblesTurno1 = finTrabajoTurno1 - fechaActual;

                if (horasRestantes <= horasDisponiblesTurno1.TotalHours)
                {
                    // Sumar las horas restantes y finalizar
                    fechaActual = fechaActual.AddHours(horasRestantes);
                    horasRestantes = 0;
                    continue;
                }
                else
                {
                    // Restamos las horas del turno 1 y pasamos al siguiente periodo
                    horasRestantes -= horasDisponiblesTurno1.TotalHours;
                    fechaActual = finTrabajoTurno1;
                }
            }

            // Si el primer turno ya ha terminado, saltamos al segundo turno
            if (fechaActual >= finTrabajoTurno1 && fechaActual < inicioTrabajoTurno2)
            {
                // Saltamos al inicio del segundo turno
                fechaActual = inicioTrabajoTurno2;
            }

            // Ahora calculamos el tiempo en el segundo turno
            if (fechaActual < finTrabajoTurno2)
            {
                // Calcular horas disponibles en el segundo turno
                TimeSpan horasDisponiblesTurno2 = finTrabajoTurno2 - fechaActual;

                if (horasRestantes <= horasDisponiblesTurno2.TotalHours)
                {
                    // Sumar las horas restantes y finalizar
                    fechaActual = fechaActual.AddHours(horasRestantes);
                    horasRestantes = 0;
                }
                else
                {
                    // Restamos las horas del segundo turno y pasamos al siguiente día
                    horasRestantes -= horasDisponiblesTurno2.TotalHours;
                    fechaActual = fechaActual.AddDays(1).Date;
                }
            }
        }

        return fechaActual;
    }

    // Verificar si el día es fin de semana
    private bool EsFinDeSemana(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
    }
}
