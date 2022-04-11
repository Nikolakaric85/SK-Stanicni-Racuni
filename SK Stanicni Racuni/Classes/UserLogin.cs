using Microsoft.AspNetCore.Http;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Classes
{
    public class UserLogin
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContext;

        public UserLogin(AppDbContext context, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.httpContext = httpContext;
        }

        public bool LoggedInUserCheck()
        {
            var UserId = httpContext.HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();

            return user != null ? true : false;

        }

        public UserTab LoggedInUser()
        {
            var UserId = httpContext.HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            return user;
        }

    

    }
}
