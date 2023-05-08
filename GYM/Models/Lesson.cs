using System;
using System.Collections.Generic;

namespace GYM.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            ClassRecords = new HashSet<ClassRecord>();
            Payments = new HashSet<Payment>();
            //SearchRemainClasses = new HashSet<SearchRemainClass>();
        }

        public int LessonId { get; set; }
        public string LessonName { get; set; } = null!;
        public decimal UnitPrice { get; set; }

        public virtual ICollection<ClassRecord> ClassRecords { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        //public virtual ICollection<SearchRemainClass> SearchRemainClasses { get; set; }

    }
}
