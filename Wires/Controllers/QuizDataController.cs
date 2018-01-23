using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wires.Services;
using Wires.Models.Dto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wires.Controllers
{
    [Route("api/[controller]/[action]")]
    public class QuizDataController : Controller
    {
        private readonly IAppRepository _appRepository;

        public QuizDataController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }
        
        [HttpPost]
        public IActionResult CheckAnswers(QuizResultDto dto)
        {
            foreach (var item in dto.Results)
            {
                var q = _appRepository.GetQuestion(item.QuestionId);
                item.RightAnswer = q.RightAnswer;
                item.Result = q.RightAnswer == item.GivenAnswer;
            }; 
            
            return Json(dto);
        }
    }
}
