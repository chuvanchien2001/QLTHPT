namespace thpt.ThachBan.v2.Models.Middleware
{
    /// <summary>
    /// Kiểm tra xem người dùng đã đăng nhập thành công chưa
    /// </summary>
    public class CheckSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// Nếu session chưa lưu thông tin người đăng nhập thì chuyển đến trang đăng nhập
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            string UrlPath = context.Request.Path.ToString();
            var tam = context.Session.GetString("UserInfor");
            if (tam==null && !context.Request.Path.Value.Contains("/Login/"))
            {
                context.Response.Redirect("/Login/Login");
                return;
            }

            await _next(context);
        }
    }
}
