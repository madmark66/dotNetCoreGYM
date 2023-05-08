using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYM.ModelsSelect
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }


        public string LessonName { get; set; }

        public List<SelectListItem> LessonNames;
    }
}
