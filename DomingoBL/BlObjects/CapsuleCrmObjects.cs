using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.BlObjects
{
    public class phone_number
    {
        public long id { get; set; }
        public string type { get; set; }
        public string number { get; set; }
    }

    public class email_address
    {
        public long id { get; set; }
        public string type { get; set; }
        public string address { get; set; }
    }

    public class CapsuleCrmParty
    {
        public long id { get; set; }
        public string type { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string title { get; set; }
        public email_address [] emailAddresses { get; set; }
        public phone_number[] phoneNumbers { get; set; }
        
    }
}
