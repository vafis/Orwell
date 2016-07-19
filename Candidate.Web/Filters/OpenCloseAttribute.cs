using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;


namespace Candidates.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OpenCloseAttribute : FilterAttribute, IActionFilter
    {
        public Settings settings { set; get; }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext){}

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var settings = DependencyResolver.Current.GetService<Settings>();

            bool valid = false;
            var Url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(Url.Action("Closed", "Application"));


            TimeSpan start = settings.StartOpenTime; //20:00
            TimeSpan end = settings.CloseOpenTime;  //12:00
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (start <= end)
            {
                // start and stop times are in the same day
                if (now >= start && now <= end)
                {
                    filterContext.Result = null;// new HttpStatusCodeResult(HttpStatusCode.n);
                }
            }
            else
            {
                // start and stop times are in different days
                if (now >= start || now <= end)
                {
                    filterContext.Result = null; //new HttpStatusCodeResult(HttpStatusCode.Accepted);
                }
            }

            
        }
    }
}