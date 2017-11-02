using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class Message
    {
        bool _error { get; set; }
        string _description { get; set; }

        public Message(bool error, string description)
        {
            _error = error;
            _description = description;
        }
    }
}
