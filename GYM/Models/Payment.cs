using System;
using System.Collections.Generic;

namespace GYM.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        public int LessonId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
