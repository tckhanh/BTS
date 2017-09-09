using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Infastructure.Core
{
    public class PaginationSet<T>
    {
        public int Page { set; get; }
        public int Count
        {
            get
            {
                return items.Count();
            }
        }
        public int TotalPages { set; get; }
        public int TotalCount { set; get; }

        public IEnumerable<T> items { set; get; }
    }
}