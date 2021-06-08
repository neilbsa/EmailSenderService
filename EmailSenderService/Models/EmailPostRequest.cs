using Microsoft.AspNetCore.Http;

namespace EmailSenderService.Models
{
    public class EmailPostRequest
    {

        public string[] TO { get; set; }
        public string[] CC { get; set; }
        public string[] BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public EmailConfiguration EmailConfiguration { get; set; }


    }



}
