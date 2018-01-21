using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Entities;

namespace Wires.Models.PanelViewModels
{
    public class ListViewModel
    {
        public List<Article> LatestArticles { get; set; }
        public bool IsUpdated { get; set; }
    }
}
