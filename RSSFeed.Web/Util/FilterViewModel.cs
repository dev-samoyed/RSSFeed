using Microsoft.AspNetCore.Mvc.Rendering;
using RSSFeed.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeed.Web.Util
{
    public class FilterViewModel
    {
        public FilterViewModel(string source, string sortType)
        {
            SelectedSource = source;
            SelectedSortType = sortType;
        }
        
        public string SelectedSource { get; set; }
        public string SelectedSortType { get; private set; }  
    }
}
