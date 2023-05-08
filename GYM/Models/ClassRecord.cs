using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace GYM.Models
{
    public partial class ClassRecord
    {
        public int ClassRecordId { get; set; }
        public int MemberId { get; set; }
        public int LessonId { get; set; }
        public DateTime ClassDate { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
