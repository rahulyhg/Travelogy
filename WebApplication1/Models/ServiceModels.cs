using DomingoBL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MessageViewModel
    {
        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please add a Subject.")]
        public string Subject { get; set; }

        [Display(Name = "Body")]
        [Required(ErrorMessage = "Please write something.")]
        public string Body { get; set; }

        public DateTime CreateDate { get; set; } 
        
        public int TripId { get; set; }       
    }

    public class MessageListModel
    {
        public List<MessageCollection> AllMessages { get; set; }

        public List<MessageCollection> TripMessages { get; set; }

        public List<MessageCollection> RecentMessages { get; set; }

        public List<MessageCollection> OldMessages { get; set; }
    }

    /// <summary>
    /// Used for the partial view for one message : subject + messages
    /// </summary>
    public class MessageListItemViewModel
    {
        public MessageCollection MessageThread { get; set; }

        public int TripId { get; set; }

        public int AdminId { get; set; }

        public bool ShowTripViewLink { get; set; }
    }
}