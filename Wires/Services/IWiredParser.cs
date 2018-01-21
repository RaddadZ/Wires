using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Entities;

namespace Wires.Services
{
    public interface IWiredParser
    {
        Task<IEnumerable<Article>> GetLatestArticles();
        Task<Article> GetArticle(string link);
    }
}
