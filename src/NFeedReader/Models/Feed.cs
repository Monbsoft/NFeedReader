using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFeedReader.Models
{
    public class Feed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationDate { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}
