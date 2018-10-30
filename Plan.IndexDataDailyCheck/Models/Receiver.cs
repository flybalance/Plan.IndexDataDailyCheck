using System.Collections.Generic;
using System.Xml.Serialization;

namespace Plan.IndexDataDailyCheck.Models
{
    [XmlRoot("Configuration")]
    public class EmailConfiguration
    {
        [XmlArray("Receivers"), XmlArrayItem("Receiver")]
        public List<Receiver> Receivers { get; set; }

        [XmlElement("Sender")]
        public Sender Sender { get; set; }
    }

    [XmlRoot("Receiver")]
    public class Receiver
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("EmailAddr")]
        public string EmailAddr { get; set; }
    }

    [XmlRoot("Sender")]
    public class Sender
    {
        [XmlElement("Host")]
        public string Host { get; set; }

        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }
    }
}