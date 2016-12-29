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
        public static DomingoBlError CreateThread(ThreadMessage tm, string title)
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var _thread = new Thread()
                {
                    AuthorUserId = tm.AuthorUserId,
                    CreatedDate = DateTime.Now,
                    Title = title
                };
                context.Threads.Add(_thread);
                context.ThreadMessages.Add(tm);
                context.SaveChanges();

                tm.ThreadId = _thread.Id;
                context.SaveChanges();
            }

            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }

        public static DomingoBlError AddToThread(ThreadMessage tm)
        {
            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }
    }
}
