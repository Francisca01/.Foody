using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RegistosController : ControllerBase
    {
        // POST api/<RegistosController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoUtilizador)
        {
            return UserService.ValidateUser(novoUtilizador, false);
        }
    }
}
