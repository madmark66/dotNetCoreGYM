using System;
using System.Collections.Generic;

namespace GYM.Models
{
    public partial class Member
    {
        public Member()
        {
            ClassRecords = new HashSet<ClassRecord>();
            Payments = new HashSet<Payment>();
            //SearchRemainClasses = new HashSet<SearchRemainClass>();
        }

        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public string MemberPassword { get; set; }

        public int? MemberRank { get; set; }

        public string? MemberApproved { get; set; }

        public virtual ICollection<ClassRecord> ClassRecords { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        //public virtual ICollection<SearchRemainClass> SearchRemainClasses { get; set; }
    }
}
