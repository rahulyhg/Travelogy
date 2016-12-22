using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoDAL
{
    public class DomingoDalException : Exception
    {
        public string DalDescription { get; set; }
    }
}
