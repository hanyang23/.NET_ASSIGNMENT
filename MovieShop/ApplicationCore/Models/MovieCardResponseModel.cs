using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MovieCardResponseModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string posterUrl { get; set; }
        public DateTime releaseDate { get; set; }
    }
}