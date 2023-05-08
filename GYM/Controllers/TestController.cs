using GYM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Data.SqlTypes;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using GYM.ModelsSelect;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace GYM.Controllers
{
    public class TestController : Controller
    {
        private const string Value = "2023/2/1";
        private readonly GYMContext _db = new GYMContext();

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, GYMContext context)
        {  //                                                                                          ****************************（自己動手加上）
            _logger = logger;
            _db = context;    //*****************************（自己動手加上）
            // https://blog.givemin5.com/asp-net-mvc-core-tao-bi-hen-jiu-di-yi-lai-zhu-ru-dependency-injection/
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Member _member)
        {
            if (ModelState.IsValid)
            {
                // 連結DB的 db_user資料表，請參考 UserDBController（MVC 第三天課程）的 Details動作，共有四種寫法。
                var ListOne = from m in _db.Members
                              where m.MemberName == _member.MemberName && m.MemberPassword == _member.MemberPassword
                              select m;
                //select new { m.UserName, m.Id};
                Member _result = ListOne.FirstOrDefault();  // 執行上面的查詢句，得到結果。

                if (_result == null)
                {   // 找不到這一筆記錄（帳號與密碼有錯，沒有這個會員）
                    //return HttpNotFound();
                    ViewData["ErrorMessage"] = "帳號與密碼有錯";
                    return View();
                }
                else if (_result.MemberRank < 2)
                {   //*************************************************************(start)

                    var claims = new List<Claim>   // 搭配 System.Security.Claims; 命名空間
                    {
                        new Claim(ClaimTypes.Name, _member.MemberName),
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.xmlsoap.org/ws/2008/06/identity/claims/name; CLAIM VALUE: 你輸入的數值」
                        new Claim("SelfDefine_LastName", _result.MemberId.ToString()),    // ID編號
                        // 讀取後的結果是 CLAIM TYPE: SelfDefine_LastName（自定義的「欄位」名稱）;   CLAIM VALUE: 你輸入的「數值」
                        // *** 重點！ _result 才是從 DbUser資料表讀出的數值。
                        //      千萬不能寫成 _User不然就會變成從Login畫面傳來的輸入值 ***

                        new Claim(ClaimTypes.Role, "Admin")   // ***********    
                        // *** 如果要有「群組、角色、權限」，可以加入這一段 *********
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.microsoft.com/ws/2008/06/identity/claims/role; CLAIM VALUE: Administrator」
                    };


                    #region ***** 不使用ASP.NET Core Identity的 cookie 驗證 *****


                    // 底下的 ** 登入 Login ** 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),   // 從現在算起，Cookie何時過期
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    // *** 登入 Login *********
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);
                    #endregion

                    //return Content("<h3>恭喜您，登入成功</h3>");
                    // return LocalRedirect(Url.GetLocalUrl(returnUrl));  // 登入成功後，返回原本的網頁

                    return RedirectToAction("Index2", "Test"); // 登入成功後，自動跳去 Index2網頁

                    // 完成這個範例以後，您可以參考這篇文章 - OWIN Forms authentication（作法很類似）
                    // https://blogs.msdn.microsoft.com/webdev/2013/07/03/understanding-owin-forms-authentication-in-mvc-5/
                }

                else if (_result.MemberRank < 3)
                {   //*************************************************************(start)

                    var claims = new List<Claim>   // 搭配 System.Security.Claims; 命名空間
                    {
                        new Claim(ClaimTypes.Name, _member.MemberName),
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.xmlsoap.org/ws/2008/06/identity/claims/name; CLAIM VALUE: 你輸入的數值」
                        new Claim("SelfDefine_LastName", _result.MemberId.ToString()),    // ID編號
                        // 讀取後的結果是 CLAIM TYPE: SelfDefine_LastName（自定義的「欄位」名稱）;   CLAIM VALUE: 你輸入的「數值」
                        // *** 重點！ _result 才是從 DbUser資料表讀出的數值。
                        //      千萬不能寫成 _User不然就會變成從Login畫面傳來的輸入值 ***

                        new Claim(ClaimTypes.Role, "ActiveStudent")   // ***********    
                        // *** 如果要有「群組、角色、權限」，可以加入這一段 *********
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.microsoft.com/ws/2008/06/identity/claims/role; CLAIM VALUE: Administrator」
                    };


                    #region ***** 不使用ASP.NET Core Identity的 cookie 驗證 *****


                    // 底下的 ** 登入 Login ** 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),   // 從現在算起，Cookie何時過期
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    // *** 登入 Login *********
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);
                    #endregion

                    //return Content("<h3>恭喜您，登入成功</h3>");
                    // return LocalRedirect(Url.GetLocalUrl(returnUrl));  // 登入成功後，返回原本的網頁

                    return RedirectToAction("Index2", "Test"); // 登入成功後，自動跳去 Index2網頁

                    // 完成這個範例以後，您可以參考這篇文章 - OWIN Forms authentication（作法很類似）
                    // https://blogs.msdn.microsoft.com/webdev/2013/07/03/understanding-owin-forms-authentication-in-mvc-5/
                }

                else
                {   //*************************************************************(start)

                    var claims = new List<Claim>   // 搭配 System.Security.Claims; 命名空間
                    {
                        new Claim(ClaimTypes.Name, _member.MemberName),
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.xmlsoap.org/ws/2008/06/identity/claims/name; CLAIM VALUE: 你輸入的數值」
                        new Claim("SelfDefine_LastName", _result.MemberId.ToString()),    // ID編號
                        // 讀取後的結果是 CLAIM TYPE: SelfDefine_LastName（自定義的「欄位」名稱）;   CLAIM VALUE: 你輸入的「數值」
                        // *** 重點！ _result 才是從 DbUser資料表讀出的數值。
                        //      千萬不能寫成 _User不然就會變成從Login畫面傳來的輸入值 ***

                       
                    };


                    #region ***** 不使用ASP.NET Core Identity的 cookie 驗證 *****


                    // 底下的 ** 登入 Login ** 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        
                    };

                    // *** 登入 Login *********
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);
                    #endregion


                    return RedirectToAction("Index2", "Test"); // 登入成功後，自動跳去 Index2網頁

                    
                }

            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // 自己宣告 Microsoft.AspNetCore.Authentication.Cookies; 命名空間
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
            //return RedirectToPage("/Account/SignedOut");   // 登出，跳去另一頁。
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SearchCashInMonth() //No.3 查當月收現
        {

            var query = "SELECT SUM(paymentAmount) AS total FROM payment WHERE MONTH(payment.paymentDate) =  Month(Getdate()) AND YEAR(payment.paymentDate) = YEAR(Getdate())";
            var cash = _db.SearchCashInMonths.FromSqlRaw(query).ToList();
               
            return View(cash);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SearchRevenueInMonth() //No.4 查當月營收(參數為2個選定日期區間)
        {
            return View();
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        
        public IActionResult SearchRevenueIn(DateTime FromDate, DateTime ToDate) //No.4 查當月營收(參數為月/年)
        {
            //var FromDate3 = FromDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"); //SqlNullValueException: Data is Null. This method or property cannot be called on Null values.
            //var ToDate1 = ToDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
            


            //var FromDate1 = FromDate.ToShortDateString; //ArgumentException: No mapping exists from object type System.Func`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]] to a known managed provider native type.
            //var ToDate1 = ToDate.ToShortDateString;

            //var FromDate1 = FromDate.GetDateTimeFormats(); //ArgumentException: No mapping exists from object type System.Func`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]] to a known managed provider native type.
            //var ToDate1 = ToDate.GetDateTimeFormats();

            //var FromDate1 = 2023-01-01; //SqlException: 運算元類型衝突: date 與 int 不相容
            //var ToDate1 = 2023-02-28;

            //var f1 = Convert.ToString(FromDate); //先變字串再想辦法變yyyy-mm-dd的型式(空白起除掉)
            //var t1 = Convert.ToString(ToDate);

            //var FromDate2 = f1.Substring(0, 9);
            //var ToDate2 = t1.Substring(0, 9);

            //FromDate.ToSqlString();
            //ToDate.ToSqlString();




            //var FromDate1 = Convert.ToDateTime(FromDate2);
            //var ToDate1 = Convert.ToDateTime(ToDate2);
            
            //var FromDate1 = Convert.ToDateTime("2023/2/1 "); //成功!!
            //var ToDate1 = Convert.ToDateTime("2023/2/28 ");

            //var FromDate1 = FromDate.ToString("yyyy-mm-dd"); //SqlException: 從字元字串轉換成日期及/或時間時，轉換失敗。
            //var ToDate1 = ToDate.ToString("yyyy-mm-dd");
            //typeof("2023-02-28");
            //var FromDate1 = FromDate.ToString(new CultureInfo("en-us")); //SqlException: 從字元字串轉換成日期及 / 或時間時，轉換失敗。
            //var ToDate1 = ToDate.ToString(new CultureInfo("en-us"));

            //var FromDate1 = FromDate.ToString(new CultureInfo("en-us")); //SqlException: 從字元字串轉換成日期及 / 或時間時，轉換失敗。
            //var ToDate1 = ToDate.ToString(new CultureInfo("en-us"));

            //var FromDate2 = Convert.ToDateTime(FromDate1); //SqlTypeException: SqlDateTime overflow. Must be between 1 / 1 / 1753 12:00:00 AM and 12 / 31 / 9999 11:59:59 PM.
            //var ToDate2 = Convert.ToDateTime(ToDate1);

            //ViewBag.p1 = FromDate.ToString(new CultureInfo("en-us")); //秀出來的是 2023/10/26 上午 12:00:00 變成 2023-10-26 DateTime格式就應該可以

            //DateTime rngMin = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            //DateTime rngMax = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;


            //ViewBag.p1 = FromDate3.GetType().Name; //2023/2/1 上午 12:00:00 不成功 SqlTypeException: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.

            //ViewBag.p2 = FromDate2.GetType().Name; //2023/2/1 string 

            //ViewBag.p3 = FromDate1; //2023/2/1 上午 12:00:00  只有這個成功

            ViewBag.p4 = "2023/2/1".GetType().Name; //2023/2/1  string

            ViewBag.p5 = FromDate;
             
            ViewBag.p6 = ToDate;
            //ViewBag.p3 = f2;

            //var FromDate1 = Convert.ToDateTime(FromDate); //SqlTypeException: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            //var ToDate1 = Convert.ToDateTime(ToDate);

            //var sql_parameter = new SqlParameter("@d", SqlDbType.DateTime2, 1);

            SqlParameter _fromDate = new SqlParameter("@FDate", FromDate); //SqlTypeException: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            SqlParameter _toDate = new SqlParameter("@TDate", ToDate);

            //SqlParameter _fromDate = new SqlParameter("FDate", FromDate2); //SqlTypeException: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            //SqlParameter _toDate = new SqlParameter("TDate", ToDate2);


            //var fromDate = FromDate.ToLongDateString; //SqlException: 接近 '`' 之處的語法不正確。
            //var toDate = ToDate.ToLongDateString;
            //var fromDate = FromDate.ToString("YYYY/MM/dd"); //SqlException: 無效的資料行名稱 'YYYY'。
            //var toDate = ToDate.ToString("YYYY/MM/dd");
            //var fromDate = FromDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"); //SqlException: 接近 '’' 之處的語法不正確。
            //var toDate = ToDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK");

            //var from = FromDate.Date;
            //var to = ToDate.Date;
            //Console.WriteLine(toDate);
            //var query = $"SELECT sum(unit_price) AS revenue FROM lesson L RIGHT JOIN classRecord C ON L.lesson_id = C.lesson_id WHERE C.classDate BETWEEN CONVERT(varchar, {FromDate}, 23) AND CAST({ToDate} AS DATE)";
            //var query = $"SELECT sum(unit_price) AS revenue FROM lesson L RIGHT JOIN classRecord C ON L.lesson_id = C.lesson_id WHERE C.classDate BETWEEN @fromDate AND @toDate";
            //var query = $"SELECT sum(unit_price) AS revenue FROM lesson L RIGHT JOIN classRecord C ON L.lesson_id = C.lesson_id WHERE C.classDate BETWEEN {fromDate} AND {toDate}";
            //var query = $"SELECT L.unit_price AS revenue FROM lesson L WHERE L.lesson_name = '{text}'";


            var revenue = _db.SearchRevenueInMonths.FromSqlRaw("SELECT sum(unit_price) AS revenue FROM lesson L RIGHT JOIN classRecord C ON L.lesson_id = C.lesson_id WHERE C.classDate BETWEEN @FDate AND @TDate", _fromDate, _toDate).ToList();

            //var revenue = _db.SearchRevenueInMonths.FromSqlRaw("SELECT sum(unit_price) AS revenue FROM lesson L RIGHT JOIN classRecord C ON L.lesson_id = C.lesson_id WHERE C.classDate BETWEEN 'FromDate1' AND 'ToDate1'").ToList();


            return View("RevenueResult", revenue);
            //return View();
            //return Content("message from input {0}", FromDate.ToString());
            //return View("RevenueResult");
        }

        [Authorize(Roles = "ActiveStudent, Admin")]
        public IActionResult SearchRemainClass() //No.5 查每人剩下課程數
        {

            var query = "SELECT m.member_name, l.lesson_name, z.RemainingClass \r\nFROM(SELECT X.member_id,X.lesson_id,　FLOOR(X.NumUsage-W.classCount) RemainingClass\r\nFROM(SELECT s.member_id, s.lesson_id, COUNT(*) AS classCount\r\nFROM(SELECT a.member_id, a.lesson_id, a.classDate, t.lastPayDay \r\nFROM classRecord AS a LEFT JOIN(SELECT member_id, lesson_id, MAX(paymentDate) AS lastPayDay \r\nFROM payment GROUP BY member_id, lesson_id) AS t \r\nON (a.lesson_id = t.lesson_id) AND (a.member_id = t.member_id) \r\nWHERE a.classDate >= t.lastPayDay) AS s GROUP BY s.member_id, s.lesson_id) AS W JOIN\r\n(SELECT P.lesson_id,P.member_id,P.paymentAmount,L.unit_price Price, P.paymentAmount/L.unit_price NumUsage\r\nFROM payment P JOIN lesson L ON P.lesson_id=L.lesson_id\r\nGROUP BY P.lesson_id,P.member_id,P.paymentAmount,P.paymentDate,L.unit_price\r\nHAVING　P.paymentDate = MAX(P.paymentDate)) AS X \r\nON W.lesson_id=X.lesson_id AND W.member_id=X.member_id) AS z \r\nJOIN member m ON m.member_id = z.member_id\r\nJOIN lesson l ON l.lesson_id = z.lesson_id";
            var remain = _db.SearchRemainClasses.FromSqlRaw(query).ToList();
            //var remain = _db.SearchRemainClasses.FromSqlRaw(query).ToList();
            return View(remain);
        }


        [Authorize]
        public IActionResult Classlist() //No.9 查所有
        {

            var query = "SELECT * FROM ClassRecord";
            var list = _db.ClassRecords.FromSqlRaw(query).Include("Member").Include("Lesson").ToList();

            return View(list);
        }

        public IActionResult AccessDeny() //
        {
            return View();
        }
        //------------------------------------------------------------ many to many drop down list for AddClassRecord
        [Authorize(Roles = "Admin")]
        public IActionResult AddClassRecord() //新增學員上課紀錄
        {
            var memberQuery = from m in _db.Members
                              orderby m.MemberName
                              select m;

            List<SelectListItem> mNames = new List<SelectListItem>();
            foreach (var mem in memberQuery)
            {
                mNames.Add(new SelectListItem { Value = mem.MemberId.ToString(), Text = mem.MemberName });
            }

            var lessonQuery = from l in _db.Lessons
                              orderby l.LessonName
                              select l;

            List<SelectListItem> lNames = new List<SelectListItem>();
            foreach (var les in lessonQuery)
            {
                lNames.Add(new SelectListItem { Value = les.LessonId.ToString(), Text = les.LessonName });
            }

            var model = new MemberLessonViewModel();
            model.MemberNames = mNames;
            model.MemberId = 1;

            model.LessonNames = lNames;
            model.LessonId = 1;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClassRecord(MemberLessonViewModel _memberLesson) ////新增上課
        {
            if ((_memberLesson != null) && (ModelState.IsValid))   // ModelState.IsValid，通過表單驗證（Server-side validation）需搭配 Model底下類別檔的 [驗證]

            {   // 第一種方法   或 參考 https://medium.com/better-programming/beginners-guide-to-entity-framework-d862c9aaec4

                var classrecord = new ClassRecord
                {
                    LessonId = _memberLesson.LessonId,
                    MemberId = _memberLesson.MemberId,
                    ClassDate = _memberLesson.ClassDate,
                };

                _db.ClassRecords.Add(classrecord);
                _db.SaveChanges();

                return Content(" 新增一筆記錄，成功！");    // 新增成功後，出現訊息（字串）。
            }
            else
            {   // 搭配 ModelState.IsValid，如果驗證沒過，就出現錯誤訊息。
                ModelState.AddModelError("Value1", " 自訂錯誤訊息(1) ");   // 第一個輸入值是 key，第二個是錯誤訊息（字串）
                ModelState.AddModelError("Value2", " 自訂錯誤訊息(2) ");
                return View();   // 將錯誤訊息，返回並呈現在「新增」的檢視畫面上
            }
        }

        //------------------------------------------------------------ many to many drop down list for AddPayment
        [Authorize(Roles = "Admin")]
        public IActionResult AddPayment() //新增學員繳費紀錄
        {
            var memberQuery = from m in _db.Members
                              orderby m.MemberName
                              select m;

            List<SelectListItem> mNames = new List<SelectListItem>();
            foreach (var mem in memberQuery)
            {
                mNames.Add(new SelectListItem { Value = mem.MemberId.ToString(), Text = mem.MemberName });
            }

            var lessonQuery = from l in _db.Lessons
                              orderby l.LessonName
                              select l;

            List<SelectListItem> lNames = new List<SelectListItem>();
            foreach (var les in lessonQuery)
            {
                lNames.Add(new SelectListItem { Value = les.LessonId.ToString(), Text = les.LessonName });
            }

            var model = new MemberLessonViewModel();
            model.MemberNames = mNames;
            model.MemberId = 1;

            model.LessonNames = lNames;
            model.LessonId = 1;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPayment(MemberLessonViewModel _memberLesson) ////新增上課
        {
            if ((_memberLesson != null) && (ModelState.IsValid))   // ModelState.IsValid，通過表單驗證（Server-side validation）需搭配 Model底下類別檔的 [驗證]

            {   // 第一種方法   或 參考 https://medium.com/better-programming/beginners-guide-to-entity-framework-d862c9aaec4

                var payment = new Payment
                {
                    LessonId = _memberLesson.LessonId,
                    MemberId = _memberLesson.MemberId,
                    PaymentDate = _memberLesson.ClassDate,
                    PaymentAmount = _memberLesson.PaymentAmount,                   
                };

                _db.Payments.Add(payment);
                _db.SaveChanges();

                return Content(" 新增一筆記錄，成功！");    // 新增成功後，出現訊息（字串）。
            }
            else
            {   // 搭配 ModelState.IsValid，如果驗證沒過，就出現錯誤訊息。
                ModelState.AddModelError("Value1", " 自訂錯誤訊息(1) ");   // 第一個輸入值是 key，第二個是錯誤訊息（字串）
                ModelState.AddModelError("Value2", " 自訂錯誤訊息(2) ");
                return View();   // 將錯誤訊息，返回並呈現在「新增」的檢視畫面上
            }
        }

        //------------------------------------------------------------ update member rank

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult MemberRankUpdate(int? _ID = 1) //變更學員身分
        {
            if (_ID == null)
            {   // 沒有輸入文章編號（_ID），就會報錯 - Bad Request
                //return new StatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
                return Content("請輸入 _ID 編號");
            }

            // 使用上方 Details動作的程式，先列出這一筆的內容，給使用者確認
            Member mt = _db.Members.Find(_ID);

            if (mt == null)
            {   // 找不到這一筆記錄
                // return NotFound();
                return Content("找不到任何記錄");
            }
            else
            {
                return View(mt);
            }
        }


        [HttpPost, ActionName("MemberRankUpdate")]
        [ValidateAntiForgeryToken]
        public IActionResult MemberRankUpdateConfirm(Member _member) ////新增上課
        {
            // https://docs.microsoft.com/zh-tw/aspnet/core/data/ef-mvc/crud  關於大量指派（overposting / 過多發佈）的安全性注意事項
            if (_member == null)
            {   // 沒有輸入內容，就會報錯 - Bad Request
                return new StatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
            }

            Member mt = _db.Members.Find(_member.MemberId);

            mt.MemberRank = _member.MemberRank;  // 修改後的值

            _db.SaveChanges();

            return Content(" 更新一筆記錄，成功！");


        }

    }
}
