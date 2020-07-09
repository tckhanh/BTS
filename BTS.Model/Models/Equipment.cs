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
    public class Equipment: Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int Tx { get; set; }

        [MaxLength(30)]
        public string Band { get; set; }

        [MaxLength(10)]
        public string OperatorRootID { get; set; }

        public double MaxPower { get; set; }
    }
}
