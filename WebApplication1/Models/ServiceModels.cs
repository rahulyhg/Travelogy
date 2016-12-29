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
    }
}