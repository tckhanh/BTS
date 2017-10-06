using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Dítricts")]
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(3)]
        public string CityID { get; set; }

        public virtual IEnumerable<BTSCertificate> BTSCertificates { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }
    }
}