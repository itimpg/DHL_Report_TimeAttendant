using System.ComponentModel.DataAnnotations;

namespace DHL.Report.TimeAttendance.Data.Entities
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string WorkFrom { get; set; }
        public string WorkTo { get; set; }
        public string MealFrom { get; set; }
        public string MealTo { get; set; }
        public string BreakFrom { get; set; }
        public string BreakTo { get; set; }
    }
}
