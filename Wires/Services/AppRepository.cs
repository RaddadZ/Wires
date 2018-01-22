using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Data;
using Wires.Entities;

namespace Wires.Services
{
    public class AppRepository : IAppRepository
    {
        private ApplicationDbContext _context;

        public AppRepository(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void AddArticle(Article article)
        {
            article.Id = Guid.NewGuid();
            _context.Articles.Add(article);
        }

        public void AddQuestion(Question question)
        {
            question.Id = Guid.NewGuid();
            _context.Questions.Add(question);
        }

        public void AddQuiz(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid();
            _context.Quizzes.Add(quiz);
        }

        public bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(a => a.Id == id);
        }
        public bool ArticleExistsByLink(string link)
        {
            return _context.Articles.Any(a => a.Link.CompareTo(link) == 0);
        }
        public void DeleteArticle(Article article)
        {
            _context.Articles.Remove(article);
        }

        public void DeleteQuestion(Question question)
        {
            _context.Questions.Remove(question);
        }

        public void DeleteQuiz(Quiz quiz)
        {
            _context.Quizzes.Remove(quiz);
        }

        public Article GetArticle(Guid id)
        {
            return _context.Articles.Where(a => a.Id == id).FirstOrDefault();
        }

        public Article GetArticleByLink(string link)
        {
            return _context.Articles.Where(a => a.Link == link).FirstOrDefault();
        }

        public IEnumerable<Article> GetArticles(int? limit)
        {
            var query = _context.Articles.OrderByDescending(a => a.PublishedDate);
            if (limit.HasValue)
                return query.Take(limit.Value).ToList();
            return query.ToList();
        }

        public IEnumerable<Article> GetArticlesWithQuizzes()
        {
            var qr = from quiz in _context.Quizzes
                     join article in _context.Articles on quiz.ArticleId equals article.Id
                     orderby article.PublishedDate descending
                     select article;
            var x = qr.ToList();
            var qry = _context.Articles
                .Join(_context.Quizzes,
                article => article.Id,
                quiz => quiz.ArticleId,
                (article, quiz) => article
                ).OrderByDescending(article => article.PublishedDate);
            var y = qry.ToList();
            return qry.ToList();
        }


        public Question GetQuestion(Guid id)
        {
            return _context.Questions.Where(a => a.Id == id).FirstOrDefault();
        }

        public IEnumerable<Question> GetQuestions()
        {
            return _context.Questions.ToList();
        }

        public IEnumerable<Question> GetQuestionsForQuiz(Guid id)
        {
            return _context.Questions.Where(q => q.QuizId == id).ToList();
        }

        public Quiz GetQuiz(Guid id)
        {
            return _context.Quizzes.Where(a => a.Id == id).FirstOrDefault();
        }
        
        public IEnumerable<Quiz> GetQuizzes()
        {
            return _context.Quizzes.ToList();
        }
        public IEnumerable<Quiz> GetQuizzesIncludeArticles()
        {
            return _context.Quizzes.Include(q => q.Article).OrderByDescending(q => q.CreatedDate).ToList();
        }
        public Quiz GetQuizForArticle(Guid id)
        {
            return _context.Quizzes
                .SingleOrDefault(q => q.ArticleId == id);
        }

        public bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(a => a.Id == id);
        }

        public bool QuizExists(Guid id)
        {
            return _context.Quizzes.Any(a => a.Id == id);
        }

        public void UpdateArticle(Article article)
        {
            // no code needed
        }

        public void UpdateQuestion(Question question)
        {
            // no code needed
        }

        public void UpdateQuiz(Quiz quiz)
        {
            // no code needed
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
