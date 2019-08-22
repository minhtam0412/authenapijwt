using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleAuthen.Models
{
    public class UserMasterRepository : IDisposable
    {
        private readonly AuthenDBEntities _ctx = new AuthenDBEntities();
        public SimpleAuthen.UserMaster ValidateUser(string username, string password)
        {
            var user = _ctx.UserMasters.FirstOrDefault(x => x.UserName.Equals(username) && x.UserPassword.Equals(password));
            return user;
        }
        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}