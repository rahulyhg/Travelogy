using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DomingoBL.EmailManagement
{
    public class EmailTemplate
    {
        private string _alias;

        [XmlAttribute("alias")]
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        private string _subject;
        [XmlElement("subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _fromName;
        [XmlElement("fromName")]
        public string FromName
        {
            get { return _fromName; }
            set { _fromName = value; }
        }

        private string _fromAddress;
        [XmlElement("fromAddress")]
        public string FromAddress
        {
            get { return _fromAddress; }
            set { _fromAddress = value; }
        }

        private string _body;
        [XmlElement("body")]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public int HtmlEmailTemplateId { get; set; }

    }
}
