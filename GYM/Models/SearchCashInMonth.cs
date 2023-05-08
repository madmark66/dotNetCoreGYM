using Microsoft.EntityFrameworkCore;

namespace GYM.Models
{
    [Keyless]
    public class SearchCashInMonth
    {
        public decimal total { get; set; }  //配合SQL查詢結果

    }
}
