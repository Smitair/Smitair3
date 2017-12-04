using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmitairDOTNET.Models
{
    public class Effect
    {
        [Key]
        public int EffectID { get; set; }

        public string EffectName { get; set; }
        public int AuthorID { get; set; }
        public double AvgGrade { get; set; }
        public int CountGrade { get; set; }
        public string YoutubeLink { get; set; }
        public string EffectLink { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }
}