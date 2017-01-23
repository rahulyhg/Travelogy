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
        /// <param name="tripId"></param>
        /// <returns></returns>
        public static DomingoBlError CreateThreadforTrip(ThreadMessage tm, string title, int tripId)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // create the Thread
                    var _thread = new Thread()
                    {
                        AuthorUserId = tm.TravellerId,
                        CreatedDate = DateTime.Now,
                        MostRecentPostDate = DateTime.Now,
                        Title = title,
                        Tags = "message",                        
                        AspnetUserId = tm.AspnetUserId
                    };

                    // save it to the DB
                    context.Threads.Add(_thread);
                    context.SaveChanges();

                    // add the message with the ID 
                    context.ThreadMessages.Add(tm);
                    tm.ThreadId = _thread.Id;

                    var trip = context.Trips.FirstOrDefault(p => p.Id == tripId);
                    if(trip != null)
                    {
                        trip.ThreadId = _thread.Id;
                    }

                    // commit
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
        /// <param name="title"></param>
        /// <returns></returns>
        public static DomingoBlError CreateThread(ThreadMessage tm, string title)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // create the Thread
                    var _thread = new Thread()
                    {
                        AuthorUserId = tm.TravellerId,
                        CreatedDate = DateTime.Now,                        
                        MostRecentPostDate = DateTime.Now,
                        Title = title,
                        Tags = "message",
                        AspnetUserId = tm.AspnetUserId
                    };

                    // save it to the DB
                    context.Threads.Add(_thread);
                    context.SaveChanges();

                    // add the message with the ID 
                    context.ThreadMessages.Add(tm);
                    tm.ThreadId = _thread.Id;

                    // commit
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
                    // update the MostRecentPostDate of the parent thread
                    var thread = context.Threads.FirstOrDefault(p => p.Id == tm.ThreadId);
                    thread.MostRecentPostDate = DateTime.Now;

                    // add the message
                    context.ThreadMessages.Add(tm);

                    // commit
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
        /// Gets all messages for an user based on AspnetUserId
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
                    // get all the threads, sorted by the latest one first
                    var threads = context.Threads.Where(p => p.AspnetUserId == AspnetUserId).OrderByDescending(p => p.MostRecentPostDate);
                    if(threads != null)
                    {
                        _messageList = new List<MessageCollection>();

                        // for all threads get the messages, latest one first
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_threadId"></param>
        /// <param name="_thread"></param>
        /// <returns></returns>
        public static DomingoBlError GetMessageThreadById(int _threadId, out MessageCollection _thread)
        {
            _thread = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // get the thread by id
                    var thread = context.Threads.FirstOrDefault(p => p.Id == _threadId);

                    // if found get the messages
                    if(thread != null)
                    {
                        var messages = context.ThreadMessages.Where(p => p.ThreadId == thread.Id).OrderBy(p => p.CreatedDate);

                        if (messages != null)
                        {
                            _thread = new MessageCollection() { Thread = thread, Messages = messages.ToList() };
                            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
                        }                        
                    }                   
                }

                return new DomingoBlError() { ErrorCode = 50, ErrorMessage = "No matching record found." };

            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };                
            }            
        }
    }
}
