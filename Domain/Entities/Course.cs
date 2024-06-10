using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get ; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Название курса обязательно")]
        [Display(Name = "Название курса")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required(ErrorMessage = "Картинка обязательна")]
        [Display(Name = "Лого курса")]
        public string ImgPath { get; set; }
    }
}
