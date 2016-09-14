using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SampleAspNetWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class IdentityController : ApiController
    {
        static HttpClient httpClient = new HttpClient();
        [Route(""), HttpGet]
        public Task<HttpResponseMessage> Get()
        {
            var principal = User as ClaimsPrincipal;
            httpClient.SetBearerToken(principal.FindFirst(x => x.Type == "token").Value);
            return httpClient.GetAsync("http://localhost/api2");
        }

        //[Route(""), HttpGet]
        //public dynamic Get()
        //{
        //    return "sadpijdsad";
        //}
    }
}