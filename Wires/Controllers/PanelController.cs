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
            var b = await UpdateLatesetArticles() > 0;
            var x = _appRepository.GetArticles(_configuration.GetValue<int>("Defaults:LatestArticlesLimit",5)).ToList();
            ListViewModel vm = new ListViewModel()
            {
                IsUpdated = b,
                LatestArticles = x
            };
            return View(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> NewExamForArticle(Guid id)
        {
            if (!_appRepository.ArticleExists(id))
                return NotFound();

            var q = new List<Question>();
            for (int i = 0; i < _configuration.GetValue<int>("Defaults:QuestionsPerQuiz", 4); i++)
            {
                q.Add(new Question());
            }
            NewExamForArticleViewModel vm = new NewExamForArticleViewModel()
            {
                Article = _appRepository.GetArticle(id),
                Questions = q
            };
            
            return View(vm);
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
    }
}
