using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Helpers;

namespace Wires.Models.Dto
{
    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public AnswerCode RightAnswer { get; set; }
        public AnswerCode GivenAnswer { get; set; }
        public bool Result { get; set; }
    }
}
