using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRApiForSql.DAL;
using SignalRApiForSql.Hubs;

namespace SignalRApiForSql.Models
{
    public class VisitorService
    {
        private readonly Context _context;
        private readonly IHubContext<VisitorHub> _hubContext;

        public VisitorService(Context context, IHubContext<VisitorHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Visitor> GetList()
        {
            return _context.Visitors.AsQueryable();
        }

        public async Task SaveVisitor(Visitor visitor)
        {
            await _context.Visitors.AddAsync(visitor);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveVisitorList", GetVisitorChartList());
        }

        public List<VisitorChart> GetVisitorChartList()
        {
            var visitors = _context.Visitors.OrderBy(x => x.VisitDate).ToList();

            var visitorCharts = new List<VisitorChart>();

            for (int i = 0, j = 0; i < visitors.Count; i++)
            {
                if (i == 0)
                {
                    visitorCharts.Add(new VisitorChart() { Id = j, VisitDate = visitors[i].VisitDate.Date.ToString("dd/MM/yyyy") });
                    j++;

                }
                else if (visitors[i].VisitDate.Date == visitors[i - 1].VisitDate.Date)
                {
                    visitorCharts[j - 1].Counts[(int)visitors[i].City - 1] += visitors[i].CityVisitCount;
                }
                else
                {
                    visitorCharts.Add(new VisitorChart() { Id = j, VisitDate = visitors[i].VisitDate.Date.ToString("dd/MM/yyyy") });
                    j++;
                }
            }

            return visitorCharts;
            
        }
    }
}
