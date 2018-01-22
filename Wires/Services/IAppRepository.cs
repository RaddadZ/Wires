using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Entities;

namespace Wires.Services
{
    public interface IAppRepository
    {
        IEnumerable<Article> GetArticles(int? limit);
        IEnumerable<Article> GetArticlesWithQuizzes();
        Article GetArticle(Guid id);
        Article GetArticleByLink(string link);
        void AddArticle(Article article);
        void DeleteArticle(Article article);
        void UpdateArticle(Article article);
        bool ArticleExists(Guid id);
        bool ArticleExistsByLink(string link);

        IEnumerable<Quiz> GetQuizzes();
        IEnumerable<Quiz> GetQuizzesIncludeArticles();
        Quiz GetQuizForArticle(Guid id);
        Quiz GetQuiz(Guid id);
        void AddQuiz(Quiz quiz);
        void DeleteQuiz(Quiz quiz);
        void UpdateQuiz(Quiz quiz);
        bool QuizExists(Guid id);

        IEnumerable<Question> GetQuestions();
        IEnumerable<Question> GetQuestionsForQuiz(Guid id);
        Question GetQuestion(Guid id);
        void AddQuestion(Question question);
        void DeleteQuestion(Question question);
        void UpdateQuestion(Question question);
        bool QuestionExists(Guid id);

        int Save();
    }
}
