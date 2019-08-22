using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleAuthen.Models
{
    public class ClientMasterRepository : IDisposable
    {
        AuthenDBEntities ctx = new AuthenDBEntities();
        //https://dotnettutorials.net/lesson/client-validation-using-basic-authentication-web-api/
        public void Dispose()
        {
            ctx.Dispose();
        }

        //This method is used to check and validate the Client credentials
        public ClientMaster ValidateClient(string ClientID, string ClientSecret)
        {
            var client =
                ctx.ClientMasters.FirstOrDefault(
                    x => x.ClientSecret.Equals(ClientSecret) && x.ClientId.Equals(ClientID));

            return client;
        }
    }
}