using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Wires.Services;

namespace Wires.Services
{
    public static class HtmlNodeExtensions
    {
        public static HtmlNode FindByClass(this HtmlNode parentNode, string cls)
        {
            return parentNode.Descendants().Where(n => n.GetAttributeValue("class", "")
                       .Equals(cls)).FirstOrDefault();
        }
        public static HtmlNode FindById(this HtmlNode parentNode, string id)
        {
            return parentNode.Descendants().Where(n => n.GetAttributeValue("id", "")
                       .Equals(id)).SingleOrDefault();
        }
        public static HtmlNode FindByAttributeValue(this HtmlNode parentNode,string attribute, string value)
        {
            return parentNode.Descendants().Where(n => n.GetAttributeValue(attribute, "")
                       .Equals(value)).FirstOrDefault();
        }
        public static HtmlNode FindByTag(this HtmlNode parentNode, string tag)
        {
            return parentNode.Descendants(tag).FirstOrDefault();
        }
    }
}
