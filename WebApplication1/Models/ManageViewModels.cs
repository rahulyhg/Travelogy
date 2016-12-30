using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DomingoDAL;
//using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ProfileViewModel
    {
        //public AspNetUser User { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City or Town")]
        public string City { get; set; }

        [Display(Name = "Postal Code / Zip / PIN")]
        public string PostCode { get; set; }

        [Display(Name = "Country of residence")]
        public string Country { get; set; }

        [Display(Name = "What best describes how you travel:")]
        public string TravelGroupSize { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ListOfCountries { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> TravelGroupSizes
        {
            get
            {
                var dropdownItems = new List<System.Web.Mvc.SelectListItem>();
                dropdownItems.AddRange(new[]{
                            new System.Web.Mvc.SelectListItem() { Text = "Travel alone", Value = "Travel alone" },
                            new System.Web.Mvc.SelectListItem() { Text = "Travel with family", Value = "Travel with family" },
                            new System.Web.Mvc.SelectListItem() { Text = "Travel with a group", Value = "Travel with a group" },
                            new System.Web.Mvc.SelectListItem() { Text = "Young couple", Value = "Young couple" },
                            new System.Web.Mvc.SelectListItem() { Text = "Mature couple", Value = "Mature couple" }});

                return dropdownItems;
            }
        }

    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}