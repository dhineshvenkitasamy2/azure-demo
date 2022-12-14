using AzureDemo.Models;
using AzureDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureDemo.Controllers
{
    [EnableCors]
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
            User userModel = new User();
            List<User> users = new List<User>();
            users = userModel.AddUsers();
            string encodedData = String.Empty;
            //return user.EmailId;
            if ((!string.IsNullOrEmpty(user.EmailId)) && !string.IsNullOrEmpty(user.Password))
            {
                var authenticatedUser = users.Find((user1 => user1.EmailId == user.EmailId && user1.Password == user.Password));
                if (authenticatedUser != null)
                {
                    var data = JsonConvert.SerializeObject(authenticatedUser);

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

        [AllowAnonymous]
        [HttpGet("GetUserId")]
        public IActionResult getUserId([FromQuery] string id)
        {
            var decodedResult = EncodedAndDecodedService.Base64Decode(id);
            string userId = String.Empty;

            if (decodedResult != null)
            {
                var result = JsonConvert.DeserializeObject<User>(decodedResult);
                if (result != null && !String.IsNullOrEmpty(result.UserId))
                    userId = result.UserId;
            }
            return Ok(userId);

        }
        [AllowAnonymous]
        [HttpGet("GetUserIdJson")]
        public IActionResult getUserIdJson([FromQuery] string id)
        {
            var decodedResult = EncodedAndDecodedService.Base64Decode(id);
            User user = new User();
            if (decodedResult != null)
            {
                user = JsonConvert.DeserializeObject<User>(decodedResult);
                if (user != null)
                    return Ok(user);
                //userId = result;
            }
            else
            {
                return NotFound();
            }
            return Ok(user);

        }

        [AllowAnonymous]
        [HttpGet("GetUser")]
        public IActionResult getUser([FromQuery] string userId)
        {
            User userModel = new User();
            List<User> users = new List<User>();
            users = userModel.AddUsers();
            var authenticatedUser = users.Find((user1 => user1.UserId == userId));
            return Ok(authenticatedUser);
        }
    }
}
