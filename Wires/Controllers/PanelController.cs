using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Entities;
using Wires.Models.PanelViewModels;
using Wires.Services;

namespace Wires.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PanelController : Controller
    {
        private readonly IAppRepository _appRepository;
        private readonly IWiredParser _wiredParser;
        private readonly IConfiguration _configuration;

        public PanelController(IConfiguration configuration,IAppRepository appRepository, IWiredParser wiredParser)
        {
            _appRepository = appRepository;
            _wiredParser = wiredParser;
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<ActionResult> List()
        {
            
            ViewData["ReturnUrl"] = $"/{RouteData.Values["controller"].ToString()}/{RouteData.Values["action"].ToString()}";
            var b = await UpdateLatesetArticles() > 0;
            //var x = _appRepository.GetArticles(_configuration.GetValue<int>("Defaults:LatestArticlesLimit",5)).ToList();
            var x = _appRepository.GetArticles().ToList();
            ListViewModel vm = new ListViewModel()
            {
                IsUpdated = b,
                LatestArticles = x
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult QuizList()
        {
            ViewData["ReturnUrl"] = $"/{RouteData.Values["controller"].ToString()}/{RouteData.Values["action"].ToString()}";
            List<Quiz> quizList = _appRepository.GetQuizzesIncludeArticles().ToList();
            return View(quizList);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult QuizForArticle(Guid id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!_appRepository.ArticleExists(id))
                return NotFound();

            var newQuestionsList = new List<Question>();
            for (int i = 0; i < _configuration.GetValue<int>("Defaults:QuestionsPerQuiz", 4); i++)
            {
                newQuestionsList.Add(new Question());
            }

            var prevQuiz = _appRepository.GetQuizForArticle(id);

            QuizForArticleViewModel vm = new QuizForArticleViewModel()
            {
                Article = _appRepository.GetArticle(id),
                Quiz = prevQuiz,
                Questions = prevQuiz == null ? newQuestionsList : _appRepository.GetQuestionsForQuiz(prevQuiz.Id).ToList()
            };
            
            return View(vm);
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult QuizForArticle(QuizForArticleViewModel vm, Guid id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!_appRepository.ArticleExists(id))
                return NotFound();

            if (vm == null)
                return BadRequest();

            if (_appRepository.GetQuizForArticle(id) != null)
            {
                ModelState.AddModelError("Quiz", "This article already has a quiz.");
            }

            foreach (var q in vm.Questions)
            {
                TryValidateModel(q);
            }

            if (ModelState.IsValid)
            {
                Quiz quiz = new Quiz()
                {
                    ArticleId = id,
                    CreatedDate = DateTime.Now,
                    Questions = vm.Questions
                };

                _appRepository.AddQuiz(quiz);

                if (_appRepository.Save() < 0)
                {
                    throw new Exception("error saving quiz.");
                }

                return RedirectToLocal(returnUrl);
            }
            vm.Article = _appRepository.GetArticle(id);
            return View(vm);
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult RemoveQuiz(Guid id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var quiz = _appRepository.GetQuiz(id);
            if (quiz == null)
                return NotFound();

            _appRepository.DeleteQuiz(quiz);

            if (_appRepository.Save() < 0)
                throw new Exception("error deleting quiz.");

            return RedirectToLocal(returnUrl);
        }

        private async Task<int> UpdateLatesetArticles()
        {
            // get links and descriptions
            List<Article> latestArticles = (await _wiredParser.GetLatestArticles()).ToList();
            
            for (int i = 0; i < latestArticles.Count(); i++)
            {
                if (_appRepository.ArticleExistsByLink(latestArticles[i].Link))
                    continue;

                latestArticles[i].FetchedDate = DateTime.Now;
                // collect all info about article
                var tempArticle = await _wiredParser.GetArticle(latestArticles[i].Link);
                if(tempArticle != null)
                {
                    latestArticles[i].Image = tempArticle.Image;
                    latestArticles[i].PublishedDate = tempArticle.PublishedDate;
                    latestArticles[i].Text = tempArticle.Text;
                }

                _appRepository.AddArticle(latestArticles[i]);
            }
            
            return _appRepository.Save();
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
