using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIS
{
    public class BasicAuthentication
    {
        private readonly RequestDelegate _next;
        private readonly UISContext _db;

        public BasicAuthentication(RequestDelegate next, UISContext db)
        {
            _next = next;
            _db = db;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.ToString().StartsWith("/api/"))
            {
                await _next.Invoke(context);
                return;
            }

            if (context.Request.Path.ToString().StartsWith("/api/Report/"))
            {
                await _next.Invoke(context);
                return;
            }

            if (context.Request.Path.ToString().StartsWith("/api/Register"))
            {
                await _next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Basic"))
            {
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            int seperatorIndex = usernamePassword.IndexOf(':');

            string username = usernamePassword.Substring(0, seperatorIndex);
            string password = usernamePassword.Substring(seperatorIndex + 1);

            var sha2 = System.Security.Cryptography.SHA256.Create();
            var hash = sha2.ComputeHash(Encoding.UTF8.GetBytes(password));
            var pswhash = BitConverter.ToString(hash).Replace("-", "");

            Login login = _db.Login.Find(username);
            if (login == null)
                context.Response.StatusCode = 403;
            else
            {
                if (pswhash == login.Password)
                {
                    context.Request.Headers.Add("UserName", username);
                    context.Request.Headers.Add("UserRole", string.IsNullOrEmpty(login.Role) ? "User" : login.Role);
                    //if (username == "username1" && password == "password2")
                    await _next.Invoke(context);
                }
                else
                    context.Response.StatusCode = 403;
            }
        }
    }
}
