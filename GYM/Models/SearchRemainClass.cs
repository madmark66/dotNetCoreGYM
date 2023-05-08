using Microsoft.EntityFrameworkCore;

namespace GYM.Models
{

    [Keyless]
    public class SearchRemainClass
    {

        public string? member_name { get; set; }
        public string? lesson_name { get; set; }

        public decimal RemainingClass { get; set; }  //配合SQL查詢結果

        //public virtual Lesson Lesson { get; set; } = null!;
        //public virtual Member Member { get; set; } = null!;
    }
}
