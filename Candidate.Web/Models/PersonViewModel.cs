using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Candidates.Web.Models
{
    public class PersonViewModel: Person
    {
        //public List<CheckedSkill> SelectedSkills =new List<CheckedSkill>();
        public List<Skill> SelectedSkills { get; set; }
    }


}