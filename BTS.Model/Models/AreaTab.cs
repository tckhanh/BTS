using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Model.Models
{
    public class AreaTab
    {
        
        [MaxLength(5)]
        public string WardId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string WardName { get; set; }
                
        [Required]
        [MaxLength(5)]
        public string DistrictId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DistrictName { get; set; }

        [MaxLength(3)]
        [Required]
        public string CityId { get; set; }
    }
}