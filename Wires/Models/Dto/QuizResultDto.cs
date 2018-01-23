using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wires.Models.Dto
{
    public class QuizResultDto
    {
        public Guid QuizId { get; set; }
        public IEnumerable<QuestionResultDto> Results { get; set; }
    }
}
