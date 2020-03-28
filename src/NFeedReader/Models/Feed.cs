using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Models
{
    public class Feed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
    }
}
