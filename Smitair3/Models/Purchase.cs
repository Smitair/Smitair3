using Smitair3.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmitairDOTNET.Models
{
    public class Purchase
    {
        [Key]
        public Guid PurchaseID { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }

        [ForeignKey("EffectId")]
        public Effect Effect { get; set; }

        public int Grade { get; set; }
    }
}