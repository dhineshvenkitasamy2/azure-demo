using AzureDemo.Models;
using AzureDemo.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzureDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {

        private IConfiguration configuration;
        public AuthenticationController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        [EnableCors]
        [HttpPost]
        public string Authentication([FromBody] User user)
        {
            string encodedData = String.Empty;
            //return user.EmailId;
            if ((!string.IsNullOrEmpty(user.EmailId)) && !string.IsNullOrEmpty(user.Password))
            {

                if ((user.EmailId == "subha@gmail.com") && (user.Password == "Hello"))
                {
                    var data = JsonConvert.SerializeObject(user);

                    return EncodedAndDecodedService.Base64Encode(data);
                }
                else
                {
                    return "Invalid User";
                }
            }
            else
            {
                return "Invalid User";
            }

        }

        [EnableCors]
        [HttpPost("IsValidUser")]

        public bool IsvalidUser([FromBody] string user)
        {

            bool isValidUser = false;
            var decodedResult = EncodedAndDecodedService.Base64Decode(user);


            if (decodedResult != null)
            {
                var result = JsonConvert.DeserializeObject<User>(decodedResult);
                if (result != null)
                {
                    if (((result.EmailId == configuration.GetSection("Email").Value)) && ((result.Password == configuration.GetSection("Password").Value)))
                    {
                        isValidUser = true;
                    }
                }

            }
            return isValidUser;

        }

    }
}
