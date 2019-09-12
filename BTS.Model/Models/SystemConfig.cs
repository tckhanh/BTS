using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    [Table("SystemConfigs")]
    public class SystemConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Code { set; get; }

        [MaxLength(50)]
        public string ValueString { set; get; }

        public int? ValueInt { set; get; }
    }
}