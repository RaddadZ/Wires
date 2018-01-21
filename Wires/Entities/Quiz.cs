using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wires.Entities
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        
        public ICollection<Question> Questions { get; set; }
            = new List<Question>();

        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }
    }
}
