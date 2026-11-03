namespace SchedulerApp.Model
{
    public class RangoTiempos
    {
        public int IdUser { get; set; }

        public DateTime StartTask { get; set; }

        public DateTime EndTask { get; set; } 

        public DateTime? StartFreeTask { get; set; }

        public DateTime? EndFreeTask { get; set; }


    }
}
