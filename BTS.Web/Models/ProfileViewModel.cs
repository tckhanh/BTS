using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ProfileViewModel
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string ApplicantID { get; set; }

        [MaxLength(30)]
        public string ProfileNum { get; set; }

        public DateTime ProfileDate { get; set; }

        public int? BtsQuantity { get; set; }

        public DateTime ApplyDate { get; set; }

        public DateTime? ValidDate { get; set; }

        public int? Fee { get; set; }

        [StringLength(30)]
        public string FeeAnnounceNum { get; set; }

        public DateTime? FeeAnnounceDate { get; set; }

        public DateTime? FeeReceiptDate { get; set; }

        public virtual Applicant Applicant { get; set; }

        public virtual IEnumerable<Bts> Btss { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public DateTime? DeletedDate { set; get; }

        public string DeletedBy { set; get; }
    }
}