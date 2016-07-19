using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Candidates.Web.Filters;
using Candidates.Web.Models;
using Candidates.Web.Models;
using Domain.Entities;
using Microsoft.Ajax.Utilities;
using Service;


namespace Candidates.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private ICandidateService _service;
        private Settings _settings;

        public ApplicationController(ICandidateService service, Settings settings)
        {
            _service = service;
            _settings = settings;
        }
        
        public ActionResult Created()
        {
            return View((Person)TempData["person"]);
        }

        public ActionResult Closed()
        {
            return View();
        }

        [OpenClose]
        public ActionResult Create()
        {
            var model = new PersonViewModel()
            {
                SelectedSkills = _service.GetLookupSkills()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create( PersonViewModel personViewModel)
        {
            Person person;
            if (ModelState.IsValid)
            {
                try
                {
                    person = new Person()
                    {
                        //Id=Guid.NewGuid(),
                        Name = personViewModel.Name,
                        Surname = personViewModel.Surname,
                        Skills = personViewModel.SelectedSkills.Where(x=>x.IsChecked==true).ToList(),
                        CreatedUtc = DateTime.UtcNow
                    };
                   
                    _service.SaveCandidate(person);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(personViewModel);
                }
                TempData["person"] = person;
                return RedirectToAction("Created");
            }

            return View(personViewModel);
        }
    }
}
