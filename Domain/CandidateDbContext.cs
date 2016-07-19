using System;
using System.Collections.Generic;
using System.Data.Entity;
using Domain.Entities;

using System.Data.Entity;

namespace Domain
{
    public class CandidateDbContext: DbContext
    {
        public CandidateDbContext() : base("name=CandidateDbContext") { }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Person>().ToTable("Persons");
           // modelBuilder.Entity<Person>().HasKey(x => x.Id);
          //  modelBuilder.Entity<Skill>().ToTable("Skills").
            //modelBuilder.Entity<Skill>().HasKey(x => x.Id);
            modelBuilder.Entity<Person>()
                .HasMany<Skill>(s => s.Skills)
                        .WithMany(c => c.Persons)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("PersonId");
                            cs.MapRightKey("SkillId");
                            cs.ToTable("PersonSkill");
                        });
            base.OnModelCreating(modelBuilder);
        }
    }


}
