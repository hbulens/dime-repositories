namespace Dime.Scheduler.Models
{
    public class Appointment
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public Task Task { get; set; }
    }
}