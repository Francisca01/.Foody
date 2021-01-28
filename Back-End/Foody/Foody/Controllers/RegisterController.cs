using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RegisterController : ControllerBase
    {
        // POST api/<RegisterController>
        [HttpPost]
        public Message Post([FromBody] User newUser)
        {
            if (newUser != null)
            {
                return UserService.ValidateUser(newUser, false);
            }
            else
            {
                return MessageService.Custom("Preencha todos os campos");
            }
        }
    }
}
