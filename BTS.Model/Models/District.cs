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
    public class District: Auditable
    {

        [Key]
        [MaxLength(5)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(3)]
        public string CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public virtual IEnumerable<Ward> Wards { get; set; }
    }
}
