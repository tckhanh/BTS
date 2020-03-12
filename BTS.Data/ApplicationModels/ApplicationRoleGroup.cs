using BTS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.ApplicationModels
{
    [Table("ApplicationRoleGroups")]
    public class ApplicationRoleGroup : Auditable
    {
        [Key]
        [Column(Order = 1)]
        [StringLength(36)]
        public string GroupId { set; get; }

        [Column(Order = 2)]
        [Key]
        public string RoleId { set; get; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup ApplicationGroup { set; get; }
    }
}