using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_CLock
{
    [Table("DMI Stations")]
    public class Location
    {
        [Key]
        public string stationId { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }
}
