using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Utility
{
    public class EmailOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<string> ToEmails { get; set; }

        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}
