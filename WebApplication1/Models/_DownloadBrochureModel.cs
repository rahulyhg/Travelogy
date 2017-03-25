using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class _DownloadBrochureModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string CircuitName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BrochurePath { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class _DownloadBrochureRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string BrochurePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Please enter your name.")]
        [Display(Name = "Name:")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Please enter your Email address.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}