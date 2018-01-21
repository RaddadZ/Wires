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
        public string Text { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public AnswerCode RightAnswer { get; set; }

        [Required]
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        public Guid QuizId { get; set; }
    }
}
