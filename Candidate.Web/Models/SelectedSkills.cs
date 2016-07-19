using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using Microsoft.Ajax.Utilities;

namespace Candidates.Web.Models
{

  public class CheckedSkill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public static implicit operator CheckedSkill(Skill skill)
        {
            return new CheckedSkill() { Id = skill.Id, Name = skill.Name, IsChecked = false };
        }

    }
}