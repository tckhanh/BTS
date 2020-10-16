using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Cities")]
    public class City : Auditable
    {
        [Key]
        [MaxLength(3)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(20)]
        public string Area { get; set; }

        [MaxLength(5)]
        public string Code { get; set; }

        public virtual IEnumerable<Bts> BTSs { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }

        public virtual IEnumerable<District> Districts { get; set; }

        public City()
        {
            
        }
    }
}