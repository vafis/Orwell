using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Domain;
using Domain.Entities;

namespace Candidates.Web
{
    public class EntitiesContextInitializer : DropCreateDatabaseIfModelChanges<CandidateDbContext>
    {
        protected override void Seed(CandidateDbContext context)
        {
            var skills = new List<Skill>()
            {
                new Skill() {Id = Guid.NewGuid(), Name = "C#"},
                new Skill() {Id = Guid.NewGuid(), Name = "SQL"},
                new Skill() {Id = Guid.NewGuid(), Name = "TDD"},
            };

            skills.ForEach(x => context.Skills.Add(x));
            context.SaveChanges();
        }
    }
}
//Database.SetInitializer(new EntitiesContextInitializer());
//http://stackoverflow.com/questions/22401290/how-do-i-map-to-a-lookup-table-using-entity-framework-code-first-with-fluent-api