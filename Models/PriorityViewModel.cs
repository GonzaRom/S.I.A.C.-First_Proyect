using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S.I.A.C.Models
{
    public class PriorityViewModel
    {
        public int keyPriority { get; set; }
        public string valuePriority { get; set; }

        public PriorityViewModel (){}

        public PriorityViewModel(int keyPriority, string valuePriority)
        {
            this.keyPriority = keyPriority;
            this.valuePriority = valuePriority;
        }

    }
}