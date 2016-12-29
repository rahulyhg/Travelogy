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
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _thread = new Thread()
                    {
                        AuthorUserId = tm.TravellerId,
                        CreatedDate = DateTime.Now,                        
                        MostRecentPostDate = DateTime.Now,
                        Title = title,
                        Tags = "message"
                    };
                    context.Threads.Add(_thread);
                    context.SaveChanges();

                    context.ThreadMessages.Add(tm);
                    tm.ThreadId = _thread.Id;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };                
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        public static DomingoBlError AddToThread(ThreadMessage tm)
        {
            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspnetUserId"></param>
        /// <param name="_messageList"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllMessages(string AspnetUserId, out List<ThreadMessage> _messageList)
        {
            _messageList = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var messages = context.ThreadMessages.Where(p => p.AspnetUserId == AspnetUserId);
                    if(messages != null)
                    {
                        _messageList = messages.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }
    }
}
