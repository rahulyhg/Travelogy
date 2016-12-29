using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public class ThreadManager
    {
        public static DomingoBlError CreateThread(Thread t)
        {
            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }
    }
}
