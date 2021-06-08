using EmailSenderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSenderService.Services
{
    public interface ISender
    {
        void SendEmail(EmailMessage message);
        void TestMail();
    }
}
