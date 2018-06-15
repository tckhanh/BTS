using BTS.Model.Extensions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTS.Model.Abstract
{
    public abstract class Auditable : IAuditable
    {
        [DefaultDateTimeValue("Now")]
        public DateTime? CreatedDate { set; get; }

        [StringLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [StringLength(256)]
        public string UpdatedBy { set; get; }
    }
}