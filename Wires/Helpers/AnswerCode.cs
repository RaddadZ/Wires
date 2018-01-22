using System.ComponentModel.DataAnnotations;

namespace Wires.Helpers
{
    public enum AnswerCode
    {
        [Display(Name = "A")]
        A = 0,
        [Display(Name = "B")]
        B = 1,
        [Display(Name = "C")]
        C = 2,
        [Display(Name = "D")]
        D = 3
    }
}
