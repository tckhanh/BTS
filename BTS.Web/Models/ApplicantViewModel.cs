
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Models
{
    public class ApplicantViewModel
    {
        public int ID { get; set; }

        public string OperatorID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Fax { get; set; }

        public virtual OperatorViewModel Operator { get; set; }

        public virtual ICollection<ProfileViewModel> Profiles { get; set; }
    }
}