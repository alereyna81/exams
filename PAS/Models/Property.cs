namespace PAS.Models
{
    public class Property
    {
        public int id { get; set; }
        public String title { get; set; }
        public String address { get; set; }
        public String description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime disabled_at { get; set; }
        public String status { get; set; }
    }
}
