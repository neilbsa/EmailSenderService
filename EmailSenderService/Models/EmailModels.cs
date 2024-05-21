using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSenderService.Models
{


    public class EmailMessage
    {




        public EmailMessage(EmailPostRequest req)
        {
            ContactTO = req.TO?.Where(z=>z!=null).Select(z => new MailboxAddress("Notification",z?.Trim())).ToList();
            ContactCC = req.CC?.Where(z => z != null).Select(z => new MailboxAddress("Notification", z?.Trim())).ToList();
            ContactBCC = req.BCC?.Where(z => z != null).Select(z => new MailboxAddress("Notification", z?.Trim())).ToList();
            EmailSubject = req.Subject;
            EmailBody = req.Body;
            IsBodyHtml = req.IsHtml;
            Attachments = req?.Attachments;
            Config = req.EmailConfiguration;
        }





        public List<MailboxAddress> ContactTO { get; set; }
        public List<MailboxAddress> ContactCC { get; set; }
        public List<MailboxAddress> ContactBCC { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool IsBodyHtml { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public EmailConfiguration Config { get; set; }
    






    }



}
