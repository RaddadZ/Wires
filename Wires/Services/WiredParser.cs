using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wires.Entities;
using Wires.Services;

namespace Wires.Services
{
    public class WiredParser : IWiredParser
    {
        private readonly IConfiguration _configuration;
        public WiredParser(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Article> GetArticle(string link)
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync(link);
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument html = new HtmlDocument();
            html.Load(stream);

            var root = html.DocumentNode;
            var articleNode = root.Descendants()
                .Where(n => n.GetAttributeValue("class", "")
                .Equals("article-main-component__content"))?
                .SingleOrDefault();



            Article article = null; 
            if (articleNode != null)
            {
                article = new Article();
                try
                {
                    article.Image = articleNode.FindByClass("article-lede-component__photo")?.FindByTag("img")?.GetAttributeValue("src", "");
                    article.Link = link;
                    article.Author = System.Web.HttpUtility.HtmlDecode(articleNode.FindByAttributeValue("itemprop", "author").FirstChild.InnerText);
                    article.Title = System.Web.HttpUtility.HtmlDecode(articleNode.FindById("articleTitleFull").InnerText);
                    article.PublishedDate = DateTime.ParseExact(articleNode.FindByClass("date-mdy").InnerText + " " + articleNode.FindByClass("date-gia").InnerText
                        , "MM.dd.yy hh:mm tt", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")
                        , DateTimeStyles.AdjustToUniversal);
                    article.Text = System.Web.HttpUtility.HtmlDecode(articleNode.FindByTag("article").FirstChild.InnerHtml);
                }
                catch (Exception e)
                {
                    article = null;
                    // log it
                }

            }

            return article;
        }

        public async Task<IEnumerable<Article>> GetLatestArticles()
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = null;
            try
            {
                result = await hc.GetAsync("https://www.wired.com/most-recent");
            }
            catch(Exception e)
            {
                // log
                return new List<Article>();
            }
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument html = new HtmlDocument();
            html.Load(stream);

            var root = html.DocumentNode;
            var articleList = root.Descendants()
                .Where(n => n.GetAttributeValue("class", "")
                .Equals("archive-list-component__items"))
                .Single().ChildNodes;

            List<Article> articles = new List<Article>();

            foreach (var node in articleList)
            {

                Article article = new Article();
                try
                {
                    article.Thumbnail = node.FindByTag("img").GetAttributeValue("src", "");
                    article.Link = "https://www.wired.com" +
                        node.FindByClass("archive-item-component__link").GetAttributeValue("href", "");
                    article.Author = System.Web.HttpUtility.HtmlDecode(node.FindByClass("byline-component__link").InnerText);
                    article.Title = System.Web.HttpUtility.HtmlDecode(node.FindByClass("archive-item-component__title").InnerText);
                    article.Description = System.Web.HttpUtility.HtmlDecode(node.FindByClass("archive-item-component__desc").InnerText);
                    article.PublishedDate = DateTime.ParseExact(node.FindByTag("time").InnerText, "MMMM dd, yyyy", 
                        CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"),
                        DateTimeStyles.AdjustToUniversal);
                    articles.Add(article);
                    if (articles.Count > _configuration.GetValue<int>("Defaults:LatestArticlesLimit")-1)
                        break;
                }
                catch (Exception e)
                {
                    // log it
                }
            }

            return articles;
        }
        
    }
}
