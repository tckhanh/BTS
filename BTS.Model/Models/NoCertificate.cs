using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    [Table("NoCertificates")]
    public class NoCertificate : Auditable
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(36)]
        public string ProfileID { get; set; }

        [Required]
        [MaxLength(20)]
        public string OperatorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string BtsCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(3)]
        public string CityID { get; set; }

        public double? Longtitude { get; set; }

        public double? Latitude { get; set; }

        public int InCaseOfID { get; set; }

        [MaxLength(20)]
        public string LabID { get; set; }

        [MaxLength(30)]
        public string TestReportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TestReportDate { get; set; }

        [MaxLength(255)]
        [Required]
        public string ReasonNoCertificate { get; set; }

        public bool IsSigned { get; set; }
        
        public bool IsCanceled { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CanceledDate { get; set; }

        [MaxLength(150)]
        public string CanceledReason { get; set; }


        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [ForeignKey("InCaseOfID")]
        public virtual InCaseOf InCaseOf { get; set; }

        [ForeignKey("LabID")]
        public virtual Lab Lab { get; set; }

        public NoCertificate()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}