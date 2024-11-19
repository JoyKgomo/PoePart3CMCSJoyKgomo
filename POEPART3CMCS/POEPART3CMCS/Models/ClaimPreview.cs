namespace POEPART3CMCS.Models
{
    public class ClaimPreview
    {
        public int Id { get; set; }

        public int user_id { get; set; }

        public int cliam_id { get; set; }

        public double Amount { get; set; }

        public double Total {  get; set; }

        public string FirstName { get; set;  }
        public string LastName  { get; set;}

        public string Username { get; set; }

        public DateTime Claimdate { get; set; }

        public Document document { get; set; }

    }
}
