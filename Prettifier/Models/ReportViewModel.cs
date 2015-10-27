using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prettifier.Models
{
    public class ReportViewModel
    {
        [Display(Name = "Top Most Used Prettifiered Numbers")]
        public List<string> TopMostUsedPrettifiedNumbers { get; set; }

        [Display(Name = "Prettified Numbers Count By User")]
        public List<UserStats> PrettifiedNumbersByUsers { get; set; }
    }

    public class UserStats
    {
        [Display(Name = "User")]
        public string Email { get; set; }

        [Display(Name = "Total Prettified Numbers Count")]
        public int PrettifiedNumbersCount { get; set; }
    }
}   