﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zest.ViewModels.ViewModels
{
    public class CommentViewModel
    {
        public string Publisher { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
