using System;
using System.ComponentModel.DataAnnotations;

namespace BTS.Model.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedDate { set; get; }

        [StringLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [StringLength(256)]
        public string UpdatedBy { set; get; }

        [StringLength(256)]
        public string MetaKeyword { set; get; }

        [StringLength(256)]
        public string MetaDescription { set; get; }

        public bool Status { set; get; }
    }
}