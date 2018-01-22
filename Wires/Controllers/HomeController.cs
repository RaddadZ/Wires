using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wires.Models;
using Wires.Services;
using Wires.Entities;
using Wires.Models.PanelViewModels;
using Microsoft.Extensions.Configuration;

namespace Wires.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWiredParser _wiredParser;
        private readonly IConfiguration _configuration;
        private readonly IAppRepository _appRepository;
        public HomeController(IWiredParser wiredPareser, IConfiguration configuration, IAppRepository appRepository)
        {
            _wiredParser = wiredPareser;
            _configuration = configuration;
            _appRepository = appRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Quizzes()
        {
            ViewData["ReturnUrl"] = $"/{RouteData.Values["controller"].ToString()}/{RouteData.Values["action"].ToString()}";
            IEnumerable<Quiz> quizzes = _appRepository.GetQuizzesIncludeArticles().ToList();
            return View(quizzes);
        }
        
        public IActionResult ReadQuiz(Guid id)
        {
            if (!_appRepository.QuizExists(id))
                return NotFound();
            
            var quiz = _appRepository.GetQuiz(id);

            QuizForArticleViewModel vm = new QuizForArticleViewModel()
            {
                Article = _appRepository.GetArticle(quiz.ArticleId),
                Quiz = quiz,
                Questions = _appRepository.GetQuestionsForQuiz(quiz.Id).ToList()
            };

            return View(vm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
