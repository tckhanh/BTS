using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("SubBTSs")]
    public class SubBTS
    {
        [Key]        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(10)]
        public string OperatorID { get; set; }

        public int? BTSCertificateID { get; set; }

        [StringLength(50)]
        public string BTSCode { get; set; }

        [StringLength(50)]
        public string Equipment { get; set; }

        public int? AntenNum { get; set; }

        [StringLength(30)]
        public string Configuration { get; set; }

        [StringLength(30)]
        public string PowerSum { get; set; }

        [StringLength(30)]
        public string Band { get; set; }

        [StringLength(30)]
        public string HeightAnten { get; set; }

        public bool? Status { get; set; }

        [ForeignKey("BTSCertificateID")]
        public virtual BTSCertificate BTSCertificate { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }
    }
}