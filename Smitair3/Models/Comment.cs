using System.ComponentModel.DataAnnotations;

namespace SmitairDOTNET.Models
{
    public class Comment
    {
        [Key]
        public int CommentsID { get; set; }

        public string Text { get; set; }
        public int AuthorID { get; set; }
        public int EffectID { get; set; }
    }
}