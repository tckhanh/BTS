﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTS.Model.Models
{
    [Table("Cities")]
    public class City
    {
        [Key]
        [StringLength(3)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<BTSCertificate> BTSCertificates { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}