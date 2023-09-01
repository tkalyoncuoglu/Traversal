using BusinessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Traversal.ViewComponents.AdminDashboard
{
    public class _DashboardRightSideCharts:ViewComponent
    {
        private Context c;
        public _DashboardRightSideCharts(Context c) 
        {
            this.c = c;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.destination = c.Destinations.Count();
            ViewBag.guests = c.Users.Count();
;            return View();
        }
    }
}
