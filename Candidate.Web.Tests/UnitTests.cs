using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Candidate.Web;
using Candidates.Web;
using Candidates.Web.Models;
using Domain.Entities;
using Moq;
using Service;
using Xunit;
using Xunit.Extensions;
using  Candidates.Web.Controllers;
using System.Web.Mvc;


namespace Candidates.Web.Tests
{
    public class Fixture : IDisposable
    {
        public Fixture()
        {
             Settings = AutofacWeb.Setup().Resolve<Settings>();
             CandidateService = AutofacWeb.Setup().Resolve<ICandidateService>();
        }

        public Settings Settings { get; private set; }
        public ICandidateService CandidateService { get; private set; }
        //public ICandidateService MockCandidateService { get; private set; }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }




    public class UnitTests : IClassFixture<Fixture>
    {
        private Fixture _fixture ;
        public UnitTests(Fixture fixture )
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_CSV_file()
        {
            var mock = new Mock<ICandidateService>();
            mock.Setup(x => x.GetPersonsByDateEntry(It.IsAny<DateTime>())).Returns(
                new List<Person>()
                {
                    new Person()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kostas",
                        Surname = "Vafeiadakis",
                        CreatedUtc = DateTime.UtcNow
                    },
                    new Person()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kostas2",
                        Surname = "Vafeiadakis2",
                        CreatedUtc = DateTime.UtcNow.AddHours(-1)
                    },
                    new Person()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kostas3",
                        Surname = "Vafeiadakis3",
                        CreatedUtc = DateTime.UtcNow.AddHours(-2)
                    }
                });

            var csvJob = new CsvFileJob(_fixture.Settings, mock.Object, DateTime.UtcNow.AddHours(-1));
            csvJob.Execute();

            Assert.True(File.Exists(_fixture.Settings.FilePath));
        }

        [Fact]
        public void Create_CSV_file_Integration()
        {
            var csvJob = new CsvFileJob(_fixture.Settings, _fixture.CandidateService, DateTime.UtcNow.AddHours(-1));
            csvJob.Execute();
            Assert.True(File.Exists(_fixture.Settings.FilePath));
        }
        [Fact]
        public void Create_Application_without_error()
        {
            var skill = _fixture.CandidateService.GetLookupSkills().FirstOrDefault();
            skill.IsChecked = true;
            var personViewModel = new PersonViewModel
            {
                Name = "Kostas",
                Surname = "Vafeiadakis",
                SelectedSkills = new List<Skill>() { }
            };
            personViewModel.SelectedSkills.Add(skill);

            var mock = new Mock<ICandidateService>();
            mock.Setup(x => x.SaveCandidate(personViewModel));
            
            var application = new ApplicationController(mock.Object, _fixture.Settings);

            ActionResult result = application.Create(personViewModel);
            Assert.Equal(result.GetType(), typeof(RedirectToRouteResult));
            Assert.Equal(((RedirectToRouteResult)result).RouteValues["action"],"Created");
        }

        [Fact]
        public void Create_Application_with_error()
        {
            var skill = _fixture.CandidateService.GetLookupSkills().FirstOrDefault();
            skill.IsChecked = true;
            var personViewModel = new PersonViewModel
            {
                Name = "",
                Surname = "",
                SelectedSkills = new List<Skill>() { }
            };
            personViewModel.SelectedSkills.Add(skill);

            var mock = new Mock<ICandidateService>();
            mock.Setup(x => x.SaveCandidate(personViewModel));

            var application = new ApplicationController(mock.Object, _fixture.Settings);

            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                                  () => personViewModel, personViewModel.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(
                                    new NameValueCollection(), CultureInfo.InvariantCulture)
            };

            var binder = new DefaultModelBinder().BindModel(
                 new ControllerContext(), modelBinder);
            application.ModelState.Clear();
            application.ModelState.Merge(modelBinder.ModelState);
           

            ActionResult result = application.Create(personViewModel);
            Assert.Equal(result.GetType(), typeof(ViewResult));
            Assert.False(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
    
}
