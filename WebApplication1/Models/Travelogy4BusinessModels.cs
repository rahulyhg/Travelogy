using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BusinessContactViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool FormSubmittedSuccess { get; set; }

        [Display(Name = "First Name (Required)")]
        [Required(ErrorMessage = "Please mention your First Name.")]
        public string FIRST_NAME { get; set; }

        [Display(Name = "Last Name (Required)")]
        [Required(ErrorMessage = "Please mention your First Name.")]
        public string LAST_NAME { get; set; }

        [Display(Name = "Business Email (Required)")]
        [Required(ErrorMessage = "Please enter your Email.")]
        public string EMAIL { get; set; }

        [Display(Name = "Office Telephone Number (Required)")]
        [Required(ErrorMessage = "Please enter your Office Telephone Number.")]
        public string PHONE { get; set; }

        [Display(Name = "Business Name (Required)")]
        [Required(ErrorMessage = "Please enter the name of your Business.")]
        public string BSNS_NAME { get; set; }

        [Display(Name = "Business Website (Required)")]
        [Required(ErrorMessage = "Please enter the Website of your Business.")]
        public string BSNS_WEB { get; set; }

        [Display(Name = "Full Address of your business including country (Required)")]
        [Required(ErrorMessage = "Please enter the Full Address of your Business.")]
        public string BSNS_ADDRESS { get; set; }

        [Display(Name = "Enter Message  (Optional)")]
        public string BSNS_REQUEST { get; set; }
    }
}