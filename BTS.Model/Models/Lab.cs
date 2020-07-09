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
    [Table("Labs")]
    public class Lab : Auditable
    {
        [Key]
        [MaxLength(20)]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(30)]
        public string Phone { get; set; }

        [MaxLength(30)]
        public string Fax { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }

        public Lab()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}