using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string UserEmail { get; set; }
        public string FullName { get; set; }

        public string Gender { get; set; }

    }
}
