namespace SchedulerApp.Service
{
    public class Intervalo
    {
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fin { get; set; }

        public Intervalo(TimeSpan inicio, TimeSpan fin)
        {
            Inicio = inicio;
            Fin = fin;
        }

        public override string ToString()
        {
            return $"[{Inicio} - {Fin}]";
        }
    }
}
