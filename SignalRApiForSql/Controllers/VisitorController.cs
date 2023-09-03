using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRApiForSql.DAL;
using SignalRApiForSql.Models;

namespace SignalRApiForSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly VisitorService _visitorService;

        public VisitorController(VisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateVisitor()
        {
            Random random = new Random();
            for (int i = 1; i < 10; i++)
            { 
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                { 
                    var newVisitor = new Visitor
                    {
                        City = item,
                        CityVisitCount = random.Next(100, 2000),
                        VisitDate = DateTime.Now.AddDays(i)

                    };
                    await _visitorService.SaveVisitor(newVisitor);
                   
                }
            };
            return Ok("Ziyaretçiler başarılı bir şekilde eklendi.");
        }
    }
}
