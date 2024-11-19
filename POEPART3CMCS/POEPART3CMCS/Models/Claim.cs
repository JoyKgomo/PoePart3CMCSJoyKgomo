using System.ComponentModel.DataAnnotations.Schema;

namespace POEPART3CMCS.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string status { get; set; }
        public DateTime DateClaimed { get; set; }
        public int HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double AmountDue { get; set; }
        public string AdditionalNotes { get; set; }
        [NotMapped]
        public IFormFile Document { get; set; }

        public virtual User User { get; set; }
    }
}
