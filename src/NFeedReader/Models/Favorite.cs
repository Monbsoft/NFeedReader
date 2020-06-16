using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFeedReader.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationDate { get; set; }
    }
}
