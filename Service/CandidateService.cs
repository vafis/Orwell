using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;


namespace Service
{
    public class CandidateService : ICandidateService
    {
        private CandidateDbContext context = new CandidateDbContext();

        public void SaveCandidate(Person person)
        {
            context.Persons.Attach(person);
            context.Entry(person).State = EntityState.Unchanged;
            context.Persons.Add(person);
           
            context.SaveChanges();
        }

        public List<Skill> GetLookupSkills()
        {
            return context.Skills.ToList();
        }

        public List<Person> GetPersonsByDateEntry(DateTime date)
        {
            return context.Persons.Where(x => x.CreatedUtc.Year == date.Year
                                              && x.CreatedUtc.Month == date.Month
                                              && x.CreatedUtc.Day == date.Day).ToList();
        }
    }
}
