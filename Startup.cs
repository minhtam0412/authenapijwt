using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleAuthen.App_Start;

[assembly: OwinStartup(typeof(SimpleAuthen.Startup))]

namespace SimpleAuthen
{
    public class Startup
    {
        //https://dotnettutorials.net/lesson/token-based-authentication-web-api/
        public void Configuration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                //The Path For generating the Toekn
                TokenEndpointPath = new PathString("/token"),
                //Setting the Token Expired Time (30 minutes)
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                //MyAuthorizationServerProvider class will validate the user credentials
                Provider = new MyAuthorizationServerProvider(),
                //For creating the refresh token and regenerate the new access token
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
