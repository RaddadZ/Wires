﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wires.Entities;

namespace Wires.Models.PanelViewModels
{
    public class NewExamForArticleViewModel
    {
        public Article Article { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
