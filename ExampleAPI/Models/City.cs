namespace ExampleAPI.Models
{
    public class City
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string HistoricName { get; set; }

        public int? Population { get; set; }

        public City()
        {
        }

        public City(string name, string historicname, int? population)
        {
            Name = name;
            HistoricName = historicname;
            Population = population;
        }
    }
}
