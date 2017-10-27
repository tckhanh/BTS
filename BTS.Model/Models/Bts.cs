using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    [Table("Btss")]
    public class Bts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ProfileID { get; set; }

        [Required]
        [StringLength(10)]
        public string OperatorID { get; set; }

        [Required]
        [StringLength(100)]
        public string BtsCode { get; set; }
                  
        [Required]
        [StringLength(255)]
        public string Address { get; set; }

         [Required]
        [StringLength(3)]
        public string CityID { get; set; }

        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }
                      
        public int InCaseOfID { get; set; }
        
        public bool IsCertificated { get; set; }

        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [ForeignKey("InCaseOfID")]
        public virtual InCaseOf InCaseOf { get; set; }

    }
}
