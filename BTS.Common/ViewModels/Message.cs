using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Common.ViewModels
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsError { get; set; }
        public string Description { get; set; }

        public Message(int id, bool isError, string description)
        {
            Id = id;
            IsError = isError;
            Description = description;
        }
    }
}