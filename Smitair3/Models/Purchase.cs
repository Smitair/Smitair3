using System.ComponentModel.DataAnnotations;

namespace SmitairDOTNET.Models
{
    public class Purchase
    {
        [Key]
        public int ID { get; set; }
        public User User { get; set; }
        public Effect Effect { get; set; }
        public int Grade { get; set; }
    }
}