using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Configuration;

namespace Candidates.Web
{
    public sealed class Settings
    {
        private static  Settings _instance= new Settings();
        private static object _syncLock = new object();

        public TimeSpan StartOpenTime { get; private set; }
        public TimeSpan CloseOpenTime { get; private set; }
        public string FilePath { get; private set; }
        public TimeSpan SendEmailTime { get; private set; }

        private Settings()
        {
            string[] startTime = ConfigurationManager.AppSettings["StartOpenTime"].Split(':');
            string[] closeTime = ConfigurationManager.AppSettings["CloseOpenTime"].Split(':');
            string[] sendEmailTime = ConfigurationManager.AppSettings["SendEmailTime"].Split(':');
            StartOpenTime = new TimeSpan(int.Parse(startTime[0]), int.Parse(startTime[1]),0);
            CloseOpenTime = new TimeSpan(int.Parse(closeTime[0]), int.Parse(closeTime[1]), 0);
            SendEmailTime = new TimeSpan(int.Parse(sendEmailTime[0]), int.Parse(sendEmailTime[1]), 0);
            FilePath = ConfigurationManager.AppSettings["FilePath"];
        }

        public static Settings Instance
        {
            get
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new Settings();
                }
                return _instance;
            }
        }

    }
}