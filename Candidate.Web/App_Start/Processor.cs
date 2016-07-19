using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Candidates.Web;
using Service;

namespace Candidate.Web.App_Start
{
    public class Processor
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static CancellationToken cancellationToken = cancellationTokenSource.Token;

        public static void Setup(IJob csvFileJob, IJob emailJob)
        {
            
            Task task = Task.Factory.StartNew(() =>
            {
                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan sentemailtime = Settings.Instance.SendEmailTime;
                TimeSpan endday=new TimeSpan(23,59,59);
                TimeSpan fire = now >= sentemailtime && sentemailtime <= endday
                    ? endday.Subtract(now) + sentemailtime
                    : sentemailtime.Subtract(now);
      
               
                while (true)
                {
                    bool cancelled = cancellationToken.WaitHandle.WaitOne(fire);
                    Task.Factory.StartNew(csvFileJob.Execute);
                    Task.Factory.StartNew(emailJob.Execute); 
                    fire = new TimeSpan(24,0,0);
                }

            }, cancellationToken);
        }
    }
}