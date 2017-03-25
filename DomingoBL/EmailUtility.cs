using DomingoDAL;
using mailinblue;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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
        /// <param name="emailAddress"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DomingoBlError> SendEmail(string emailAlias, string emailAddress, Dictionary<String, String> parameters)
        {
            try
            {                
                MailAddress deliveryAddress = new MailAddress(emailAddress);                
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
        /// <param name="emailAddress"></param>
        /// <param name="parameters"></param>
        /// <param name="attachmentPath"></param>
        /// <returns></returns>
        public async Task<DomingoBlError> SendEmailWithAttachment(string emailAlias, string emailAddress, Dictionary<String, String> parameters, string attachmentPath)
        {
            try
            {
                MailAddress deliveryAddress = new MailAddress(emailAddress);
                List<MailAddress> deliveryAddresses = new List<MailAddress>();
                deliveryAddresses.Add(deliveryAddress);
                return SendEmailWithAttachments(emailAlias, deliveryAddresses, parameters, attachmentPath);
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        public DomingoBlError SendEmailWithAttachments(string emailAlias, List<MailAddress> deliveryAddresses, Dictionary<String, String> parameters, string attachmentPath)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // get the xml template from DB
                    HtmlEmailTemplate emailTemplate = context.HtmlEmailTemplates.Where(p => p.Alias == emailAlias).FirstOrDefault();
                    if (emailTemplate == null)
                    {
                        return new DomingoBlError() { ErrorCode = 200, ErrorMessage = "Invalid template alias" };
                    }

                    // replace the parameters into the template
                    HtmlEmail processedTemplate = ProcessTemplate(emailTemplate, parameters);
                    MailMessage objMessage = new MailMessage();
                    objMessage.From = new MailAddress(processedTemplate.FromAddress, processedTemplate.FromName);

                    foreach (MailAddress address in deliveryAddresses)
                    {
                        processedTemplate.ToAddress = address.Address;

                        // construct the SMTP mail and send
                        objMessage.From = new MailAddress(processedTemplate.FromAddress, processedTemplate.FromName);
                        objMessage.Subject = processedTemplate.EmailSubject;
                        objMessage.IsBodyHtml = true;
                        objMessage.Body = processedTemplate.EmailText;
                        objMessage.To.Clear();
                        objMessage.To.Add(address);

                        var sendinBlue = new API("ICdw29ZamvD0WXcJ"); // sendinblue access key from the portal
                        Dictionary<string, Object> data = new Dictionary<string, Object>();
                        Dictionary<string, string> to = new Dictionary<string, string>();
                        to.Add(address.Address, address.DisplayName);
                        List<string> from_name = new List<string>();
                        from_name.Add(processedTemplate.FromAddress);
                        from_name.Add(processedTemplate.FromName);
                        List<string> attachment = new List<string>();
                        attachment.Add(attachmentPath);                        

                        data.Add("to", to);
                        data.Add("from", from_name);
                        data.Add("subject", processedTemplate.EmailSubject);
                        data.Add("html", processedTemplate.EmailText);
                        data.Add("attachment", attachment);

                        Object sendEmail = sendinBlue.send_email(data);
                        Console.WriteLine(sendEmail);

                        // save the mail to DB 
                        // to save the mail on DB                        
                        context.HtmlEmails.Add(processedTemplate);
                        context.SaveChanges();
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }

            // exception part
            catch (FormatException ex)
            {
                return new DomingoBlError()
                {
                    ErrorCode = 100,
                    ErrorMessage = string.Format("Error : {0}", ex)
                };
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
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // get the xml template from DB
                    HtmlEmailTemplate emailTemplate = context.HtmlEmailTemplates.Where(p => p.Alias == emailAlias).FirstOrDefault();
                    if(emailTemplate == null)
                    {
                        return new DomingoBlError() { ErrorCode = 200, ErrorMessage = "Invalid template alias" };
                    }

                    // replace the parameters into the template
                    HtmlEmail processedTemplate = ProcessTemplate(emailTemplate, parameters);
                    MailMessage objMessage = new MailMessage();
                    objMessage.From = new MailAddress(processedTemplate.FromAddress, processedTemplate.FromName);

                    foreach (MailAddress address in deliveryAddresses)
                    {
                        processedTemplate.ToAddress = address.Address;

                        // construct the SMTP mail and send
                        objMessage.From = new MailAddress(processedTemplate.FromAddress, processedTemplate.FromName);
                        objMessage.Subject = processedTemplate.EmailSubject;
                        objMessage.IsBodyHtml = true;
                        objMessage.Body = processedTemplate.EmailText;
                        objMessage.To.Clear();
                        objMessage.To.Add(address);

                        var sendinBlue = new API("ICdw29ZamvD0WXcJ"); // sendinblue access key from the portal
                        Dictionary<string, Object> data = new Dictionary<string, Object>();
                        Dictionary<string, string> to = new Dictionary<string, string>();
                        to.Add(address.Address, address.DisplayName);
                        List<string> from_name = new List<string>();
                        from_name.Add(processedTemplate.FromAddress);
                        from_name.Add(processedTemplate.FromName);
                        
                        data.Add("to", to);
                        data.Add("from", from_name);
                        data.Add("subject", processedTemplate.EmailSubject);
                        data.Add("html", processedTemplate.EmailText);                        

                        Object sendEmail = sendinBlue.send_email(data);
                        Console.WriteLine(sendEmail);

                        // save the mail to DB 
                        // to save the mail on DB                        
                        context.HtmlEmails.Add(processedTemplate);
                        context.SaveChanges();
                    }
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }

            // exception part
            catch (FormatException ex)
            {
                return new DomingoBlError()
                {
                    ErrorCode = 100,
                    ErrorMessage = string.Format("Error : {0}", ex)
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected HtmlEmail ProcessTemplate(HtmlEmailTemplate template, Dictionary<String, String> parameters)
        {
            HtmlEmail processedTemplate = new HtmlEmail();
            processedTemplate.EmailSubject = ReplaceFields(template.Subject, parameters);
            processedTemplate.EmailText = ReplaceFields(template.Body, parameters);
            processedTemplate.FromAddress = template.FromAddress;
            processedTemplate.FromName = template.FromName;

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
            {
                field = field.Replace("[" + param.Key + "]", param.Value);
            }

            return field;
        }
    }
}
