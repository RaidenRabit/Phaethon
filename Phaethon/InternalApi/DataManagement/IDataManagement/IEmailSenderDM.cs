using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalApi.DataManagement.IDataManagement
{
    public interface IEmailSenderDM
    {
        void SendEmail(string customerEmail, string customerName, string jobName, string jobDescription);
    }
}
