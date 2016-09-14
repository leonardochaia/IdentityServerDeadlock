using System.Web.Http;

namespace SampleAspNetWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class IdentityController : ApiController
    {
        [Route(""), HttpGet]
        public string Get()
        {
            return "Response from Web Api 2";
        }
    }
}