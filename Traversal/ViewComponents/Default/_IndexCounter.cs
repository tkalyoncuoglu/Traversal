using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Traversal.ViewComponents.Default
{
    public class _IndexCounter:ViewComponent
    {
        private Context c;

        public _IndexCounter(Context c)
        {
            this.c = c;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.v1 = c.Destinations.Count();
            ViewBag.v2 = c.Guides.Count();
            ViewBag.v3 = c.Testimonials.Count();
            return View();
        }
    }
}
