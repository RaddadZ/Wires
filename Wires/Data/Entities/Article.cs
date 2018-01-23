using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wires.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(20000)]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        public DateTime FetchedDate { get; set; }
        public String Author { get; set; }
        public String Link { get; set; }
        public String Thumbnail { get; set; }
        public String Image { get; set; }
    }
}
