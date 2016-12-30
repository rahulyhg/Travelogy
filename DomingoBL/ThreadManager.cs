using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    /// <summary>
    /// Thread + ThreadMessage collection
    /// </summary>
    public class MessageCollection
    {
        public Thread Thread { get; set; }

        public List<ThreadMessage> Messages { get; set; }
    }

    public class ThreadManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="title"></param>
        /// <returns></returns>
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
                        Tags = "message",
                        AspnetUserId = tm.AspnetUserId
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static DomingoBlError AddToThread(ThreadMessage tm)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var thread = context.Threads.FirstOrDefault(p => p.Id == tm.ThreadId);
                    thread.MostRecentPostDate = DateTime.Now;
                    context.ThreadMessages.Add(tm);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspnetUserId"></param>
        /// <param name="_messageList"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllMessages(string AspnetUserId, out List<MessageCollection> _messageList)
        {
            _messageList = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var threads = context.Threads.Where(p => p.AspnetUserId == AspnetUserId).OrderByDescending(p => p.MostRecentPostDate);
                    if(threads != null)
                    {
                        _messageList = new List<MessageCollection>();

                        foreach (var thread in threads)
                        {
                            var messages = context.ThreadMessages.Where(p => p.ThreadId == thread.Id).OrderBy(p => p.CreatedDate);
                            if(messages != null)
                            {
                                var _message = new MessageCollection() { Thread = thread, Messages = messages.ToList() };                                
                                _messageList.Add(_message);
                            }
                        }
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
