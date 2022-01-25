namespace PAS.Models
{
    public class Activity
    {
        public int id { get; set; }
        public int property_id { get; set; }
        public DateTime schedule { get; set; }
        public String title { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public String status { get; set; }
        public String condition { get; set; }
        public String property { get; set; }
        public String survey { get; set; }
    }
}
