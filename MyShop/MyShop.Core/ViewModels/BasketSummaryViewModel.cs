﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class BasketSummaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        public BasketSummaryViewModel()
        {
        }

        public BasketSummaryViewModel(int baskeCount, decimal basketTotal)
        {
            this.BasketCount = baskeCount;
            this.BasketTotal = basketTotal;
        }
    }
}
