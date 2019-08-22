using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.Infrastructure;
using SimpleAuthen.Models;

namespace SimpleAuthen.App_Start
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        //http://www.advancesharp.com/blog/1236/asp-net-web-api-2-owin-oauth-bearer-token-refresh-token-with-custom-database
        public void Create(AuthenticationTokenCreateContext context)
        {
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            //Get the client ID from the Ticket properties
            var clientid = context.Ticket.Properties.Dictionary["client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            //Generating a Uniqure Refresh Token ID
            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthenticationRepository _repo = new AuthenticationRepository())
            {
                // Getting the Refesh Token Life Time From the Owin Context
                var refreshTokenLifeTime = context.OwinContext.Get<string>("ta:clientRefreshTokenLifeTime");

                //Creating the Refresh Token object
                var token = new RefreshToken()
                {
                    //storing the RefreshTokenId in hash format
                    ID = Helper.GetHash(refreshTokenId),
                    ClientID = clientid,
                    UserName = context.Ticket.Identity.Name,
                    IssuedTime = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                //Setting the Issued and Expired time of the Refresh Token
                context.Ticket.Properties.IssuedUtc = token.IssuedTime;
                context.Ticket.Properties.ExpiresUtc = token.ExpiredTime;

                token.ProtectedTicket = context.SerializeTicket();

                var result = _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("ta:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            using (AuthenticationRepository _repo = new AuthenticationRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshTokenByID(hashedTokenId);
                }
            }
        }
    }
}