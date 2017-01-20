using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public class EmailManager
    {
        static readonly EmailManager _instance = new EmailManager();
        static EmailManager() { }
    }


}
