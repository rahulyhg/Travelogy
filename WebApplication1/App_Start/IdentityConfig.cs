﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using WebApplication1.Helpers;
using DomingoBL;

namespace WebApplication1
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. DomingoUserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspNetUserName"></param>
        /// <returns></returns>
        public static bool UserEmailVerified(string AspNetUserName)
        {
            if (string.IsNullOrEmpty(AspNetUserName))
            {
                return false;
            }

            if (HttpContext.Current.Session["UserEmailVerified"] != null)
            {
                if ((HttpContext.Current.Session["UserEmailVerified"].ToString().ToUpper() == "TRUE"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var emailVerified = DomingoUserManager.IsUserEmailVerified(AspNetUserName);
            if (emailVerified)
            {
                HttpContext.Current.Session.Add("UserEmailVerified", "TRUE");
                return true;
            }
            else
            {
                HttpContext.Current.Session.Add("UserEmailVerified", "FALSE");
                return false;
            }            
        }

        public static void SetUserEmailVerified(string AspNetUserName)
        {
            if (HttpContext.Current.Session["UserEmailVerified"] != null)
            {
                HttpContext.Current.Session["UserEmailVerified"] = true;
            }                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspNetUserName"></param>
        /// <returns></returns>
        public static string GetUserType(string AspNetUserName)
        {
            if (string.IsNullOrEmpty(AspNetUserName))
            {
                return string.Empty;
            }

            if (HttpContext.Current.Session["LoggedInUserType"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["LoggedInUserType"].ToString()))
            {
                return HttpContext.Current.Session["LoggedInUserType"].ToString();
            }

            var userType = DomingoUserManager.GetUserType(AspNetUserName);
            HttpContext.Current.Session.Add("LoggedInUserType", userType);
            return userType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspNetUserId"></param>
        /// <returns></returns>
        //public static bool IsTravelogyAdmin(string AspNetUserName)
        //{
        //    if(string.IsNullOrEmpty(AspNetUserName))
        //    {
        //        return false;
        //    }

        //    if(HttpContext.Current.Session["IsTravelogyAdmin"] != null)                
        //    {
        //        if ((HttpContext.Current.Session["IsTravelogyAdmin"].ToString().ToUpper() == "TRUE"))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    var isAdmin = DomingoUserManager.IsTravelogyAdmin(AspNetUserName);
        //    if(isAdmin)
        //    {
        //        HttpContext.Current.Session.Add("IsTravelogyAdmin", "TRUE");
        //        return true;
        //    }
        //    else
        //    {
        //        HttpContext.Current.Session.Add("IsTravelogyAdmin", "FALSE");
        //        return false;
        //    }
        //}

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
