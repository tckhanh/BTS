using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    [Table("Labs")]
    public class Lab
    {
        [Key]                
        [StringLength(20)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string Fax { get; set; }
        
        public virtual IEnumerable<Certificate> Certificates { get; set; }
    }
}
