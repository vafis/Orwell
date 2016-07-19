using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Service
{
    public interface ICandidateService
    {
        void SaveCandidate(Person person);
        List<Skill> GetLookupSkills();
        List<Person> GetPersonsByDateEntry(DateTime date);
    }
}
