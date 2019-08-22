using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SimpleAuthen.Controllers
{
    public class TestController : ApiController
    {
        //This resource only need Authorize
        [Authorize]
        [HttpGet]
        [Route("api/test/resource1")]
        public IHttpActionResult GetResource1()
        {
            var identity = (ClaimsIdentity)User.Identity;

            var ClientID = identity.Claims.FirstOrDefault(x => x.Type == "ClientID")?.Value;
            var ClientName = identity.Claims.FirstOrDefault(x => x.Type == "ClientName")?.Value;
            var ClientSecret = identity.Claims.FirstOrDefault(x => x.Type == "ClientSecret")?.Value;

            return Ok("Hello: " + identity.Name +", Client Name is " + ClientName);
        }


        //This resource is only need Authorize
        [Authorize]
        [HttpGet]
        [Route("api/test/resource2")]
        public IHttpActionResult GetResource2()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var Email = identity.Claims
                .FirstOrDefault(c => c.Type == "Email").Value;

            var UserName = identity.Name;

            #region Get Client Info
            var ClientID = identity.Claims.FirstOrDefault(x => x.Type == "ClientID")?.Value;
            var ClientName = identity.Claims.FirstOrDefault(x => x.Type == "ClientName")?.Value;
            var ClientSecret = identity.Claims.FirstOrDefault(x => x.Type == "ClientSecret")?.Value;
            #endregion

            return Ok("Hello " + UserName + ", Your Email ID is :" + Email + ", ClientID is " + ClientID + ", ClientName is " + ClientName + ", ClientSecret is " + ClientSecret);
        }

        //This resource is only For SuperAdmin role
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("api/test/resource3")]
        public IHttpActionResult GetResource3()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return Ok("Hello " + identity.Name + "Your Role(s) are: " + string.Join(",", roles.ToList()));
        }
    }
}