using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Model.Models
{
    [Table("InCaseOfs")]
    public class InCaseOf
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Case { get; set; }

        public virtual IEnumerable<Bts> Btss { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }
    }
}
