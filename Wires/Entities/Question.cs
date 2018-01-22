using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Wires.Helpers;

namespace Wires.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
        [Required]
        public string AnswerA { get; set; }
        [Required]
        public string AnswerB { get; set; }
        [Required]
        public string AnswerC { get; set; }
        [Required]
        public string AnswerD { get; set; }
        [Required]
        public AnswerCode RightAnswer { get; set; }
        
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        public Guid QuizId { get; set; }
    }
}
