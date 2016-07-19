using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Candidates.Web;
using Service;

namespace Candidate.Web
{
    public class CsvFileJob : IJob
    {
        private Settings _settings;
        private ICandidateService _service;
        private DateTime _selectedDate;

        public CsvFileJob(Settings settings, ICandidateService service, DateTime selectedDate)
        {
            _settings = settings;
            _service = service;
            _selectedDate = selectedDate;
        }

        public void Execute()
        {
            string filePath = _settings.FilePath;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            var persons = _service.GetPersonsByDateEntry(_selectedDate);
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            persons.ForEach(x => sb.AppendLine(x.Name + "," + x.Surname));
            sb.AppendLine("Total persons: " + persons.Count);
            File.AppendAllText(filePath, sb.ToString());
        }
    }
}