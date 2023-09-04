namespace SignalRApiForSql.Models
{
    public class VisitorChart
    {
        public int Id { get; set; }
        public string VisitDate { get; set; }
        public List<int> Counts { get; set; } = Enumerable.Repeat(0, 5).ToList(); 
    }
}
