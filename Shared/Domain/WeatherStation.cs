namespace Shared.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class WeatherStation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StationId { get; set; }


        public string Name { get; set; }


        public string Location { get; set; }


        public string Description { get; set; }

    }
}