using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYM.ModelsSelect
{
    public class MemberLessonViewModel
    {
        public int MemberId { get; set; }
        //public string MemberName { get; set; }
        public List<SelectListItem> MemberNames;
        public int LessonId { get; set; }
        //public string LessonName { get; set; } = null!;
        public List<SelectListItem> LessonNames;
        public DateTime ClassDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public int? MemberRank { get; set; }

        public List<SelectListItem>? MemberRanks;

    }
}
