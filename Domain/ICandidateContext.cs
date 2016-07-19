using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain
{
    public interface ICandidateContext
    {
        IDbSet<Person> Persons { get; set; }
        IDbSet<Skill> Skills { get; set; }
        int Save();
    }
}
