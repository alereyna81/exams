namespace PAS.Models
{
    public class Survey
    {
        public int id { get; set; }
        public int activity_id { get; set; }
        public String answers { get; set; }
        public DateTime created_at { get; set; }
    }
}
