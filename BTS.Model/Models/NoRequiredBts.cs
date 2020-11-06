using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("NoRequiredBtss")]
    public class NoRequiredBts : Auditable
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

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
                
        public int SubBtsQuantity { get; set; }

        [MaxLength(255)]
        [Required]
        public string SubBtsCodes { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsOperatorIDs { get; set; }

        [MaxLength(512)]
        [Required]
        public string SubBtsEquipments { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenNums { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsConfigurations { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsPowerSums { get; set; }

        [MaxLength(256)]
        [Required]
        public string SubBtsBands { get; set; }

        [MaxLength(150)]
        [Required]
        public string SubBtsAntenHeights { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AnnouncedDate { get; set; }

        [MaxLength(50)]
        public string AnnouncedDoc { get; set; }
        
        public bool IsCanceled { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CanceledDate { get; set; }

        [MaxLength(150)]
        public string CanceledReason { get; set; }

        [ForeignKey("OperatorID")]
        public virtual Operator Operator { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        public virtual IEnumerable<SubBtsInNoRequiredBts> SubBtsInNoRequiredBtss { get; set; }
        public NoRequiredBts()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}