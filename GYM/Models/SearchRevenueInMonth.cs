using Microsoft.EntityFrameworkCore;

namespace GYM.Models
{
    [Keyless]
    public class SearchRevenueInMonth
    {
        public decimal revenue { get; set; }  //配合SQL查詢結果
    }
}
