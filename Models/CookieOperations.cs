using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TapShoesCanada.Models
{
    public class CookieOperations
    {
        public static void Set(IResponseCookies cookies,string key, string value)
        {
            CookieOptions  cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(ConfigParams.COOKIE_EXPIRY_TIME);
            cookies.Append(key, value,cookieOptions);
        }

        public static void Remove(IResponseCookies cookies, string key)
        {
            cookies.Delete(key);
        }

        public static String Read(IRequestCookieCollection cookies, string key)
        {
            return cookies[key];
        }
    }
}
