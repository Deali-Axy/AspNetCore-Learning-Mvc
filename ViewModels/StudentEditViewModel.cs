using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.ViewModels
{
    public class StudentEditViewModel : StudentCreateViewModel
    {
        public int Id { get; set; }
        public string ExistedPhotoPath { get; set; }
    }
}
