using BTS.Model.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("SubBtsInNoRequiredBtss")]
    public class SubBtsInNoRequiredBts : Auditable
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(36)]
        public string NoRequiredBtsID { get; set; }

        public int BtsSerialNo { get; set; }

        [MaxLength(50)]
        public string BtsCode { get; set; }

        [MaxLength(20)]
        public string OperatorID { get; set; }

        [MaxLength(50)]
        public string Manufactory { get; set; }

        [MaxLength(80)]
        public string Equipment { get; set; }

        public int AntenNum { get; set; }

        [MaxLength(30)]
        public string Configuration { get; set; }

        [MaxLength(30)]
        public string PowerSum { get; set; }

        [MaxLength(60)]
        public string Band { get; set; }

        [MaxLength(30)]
        public string Technology { get; set; }

        [MaxLength(30)]
        public string AntenHeight { get; set; }

        [ForeignKey("NoRequiredBtsID")]
        public virtual NoRequiredBts NoRequiredBtss { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        public SubBtsInNoRequiredBts()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}