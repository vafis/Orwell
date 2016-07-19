using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain
{
    public class CandidateContext:ICandidateContext 
    {
        public CandidateContext()
        {
            this.Persons = new FakeDbSet<Person>();
            this.Skills = new FakeDbSet<Skill>();
            this.Skills.Create<Skill>()
            this.Skills.Add(new Skill() { Name = "Sql" });
            this.Skills.Add(new Skill() { Name = "TDD" });
            /*
             this.Skills.
                new Skill() {Name = "C#"},
                new Skill() {Name = "Sql"},
                new Skill() {Name = "TDD"}
            };
            */
        }

      public IDbSet<Person> Persons { get;  set; }
      public IDbSet<Skill> Skills { get;  set; }
      public int Save()
      {
          return 0;
      }
    }
}
