using BTS.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Cities")]
    public class City : Auditable
    {
        [Key]
        [StringLength(3)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual IEnumerable<Bts> BTSs { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }
    }
}