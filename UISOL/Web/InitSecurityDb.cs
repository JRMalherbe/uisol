using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using Web.Models;
using WebMatrix.WebData;

namespace Web.Seed
{
    public class InitSecurityDb : DropCreateDatabaseAlways<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {

            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
               "UserProfile", "UserId", "UserName", autoCreateTables: true);
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (membership.GetUser("test", false) == null)
            {
                membership.CreateUserAndAccount("test", "test");
            }
            if (!roles.GetRolesForUser("test").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "test" }, new[] { "admin" });
            }
            if (membership.GetUser("joe", false) == null)
            {
                membership.CreateUserAndAccount("joe", "test");
            }

        }
    }
}