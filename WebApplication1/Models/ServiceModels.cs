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
        public string Subject { get; set; }

        [Display(Name = "Body")]
        public string Body { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public class MessageListModel
    {
        [Display(Name = "Header")]
        public string Header { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<MessageCollection> AllMessages { get; set; }
    }
}