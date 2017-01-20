﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.EmailManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAlias"></param>
        /// <param name="deliveryAddress"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DomingoBlError SendEmail(string emailAlias, MailAddress deliveryAddress, Dictionary<String, String> parameters)
        {
            try
            {
                List<MailAddress> deliveryAddresses = new List<MailAddress>();
                deliveryAddresses.Add(deliveryAddress);
                return SendEmail(emailAlias, deliveryAddresses, parameters);
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAlias"></param>
        /// <param name="deliveryAddress"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DomingoBlError SendEmail(string emailAlias, List<MailAddress> deliveryAddresses, Dictionary<String, String> parameters)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            EmailTemplate emailTemplate = new EmailTemplate();

            EmailTemplate processedTemplate = ProcessTemplate(emailTemplate, parameters);
            MailMessage objMessage = new MailMessage();

            try
            {
                objMessage.From = new MailAddress(processedTemplate.FromAddress, processedTemplate.FromName);

                foreach (MailAddress address in deliveryAddresses)
                {
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }

            // exception part
            catch (FormatException ex)
            {
                if (processedTemplate != null)
                {
                    return new DomingoBlError()
                    {
                        ErrorCode = 100,
                        ErrorMessage = string.Format("From address is not a valid email address: {0} -- {1}", processedTemplate.FromAddress, ex)
                    };
                }
                else
                {
                    return new DomingoBlError()
                    {
                        ErrorCode = 100,
                        ErrorMessage = string.Format("From address is not a valid email address : {0}", ex)
                    };
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected EmailTemplate ProcessTemplate(EmailTemplate template, Dictionary<String, String> parameters)
        {
            EmailTemplate processedTemplate = new EmailTemplate();                

            processedTemplate.Alias = template.Alias;
            processedTemplate.Body = ReplaceFields(template.Body, parameters);
            processedTemplate.FromAddress = ReplaceFields(template.FromAddress, parameters).ToLower();
            processedTemplate.FromName = ReplaceFields(template.FromName, parameters);
            processedTemplate.Subject = ReplaceFields(template.Subject, parameters);

            return processedTemplate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ReplaceFields(string field, Dictionary<String, String> parameters)
        {
            foreach (KeyValuePair<string, string> param in parameters)
                field = field.Replace("[" + param.Key + "]", param.Value);
            return field;
        }
    }
}
