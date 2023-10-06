using thpt.ThachBan.DTO.Models;
using NuGet.Protocol;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace thpt.ThachBan.v2.Models.UnititiesModel
{
    public class SessionManager
    {
        public static void SetSessionInfor(Account account, HttpContext httpContext)
        {
            httpContext.Session.SetString("UserInfor", account.ToJson());            
        }
        public static Guid GetId(HttpContext httpContext)
        {
            dynamic session = JsonConvert.DeserializeObject(httpContext.Session.GetString("UserInfor"));
            return session.AccountId;
        }

        public static string GetAccountCode(HttpContext httpContext)
        {
            dynamic session = JsonConvert.DeserializeObject(httpContext.Session.GetString("UserInfor"));
            return session.AccountCode;
        }
    }
}
