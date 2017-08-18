using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Applicants")]
    public class Applicant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string OperatorID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Telephone { get; set; }

        [StringLength(30)]
        public string Fax { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}