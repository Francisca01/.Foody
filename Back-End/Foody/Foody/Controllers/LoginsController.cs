using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LoginsController : ControllerBase
    {
        // POST api/<LoginController>
        [HttpPost]
        public IDictionary<string, string> Post([FromBody] User user)
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array 
                var userDB = db.user.ToArray();

                Dictionary<string, string> token = new Dictionary<string, string>
                {
                    {"Message", MessageService.Custom("Dados inválidos").text},
                };

                if (user.phone != 0)
                {
                    for (int i = 0; i < userDB.Length; i++)
                    {
                        // verificar se email inserido corresposnde ao da BD,
                        // e se password inserida (encriptada) corresponde à da BD
                        if (user.phone == userDB[i].phone && user.userType == userDB[i].userType &&
                            HashPassword.VerifyHash(user.password, userDB[i].password))
                        {
                            token = new Dictionary<string, string>
                            {
                                {"Token", TokenManager.GenerateToken(userDB[i].email, userDB[i].userType, userDB[i].idUser)},
                            };
                            return token;
                        }
                    }
                }

                //valida o email
                try
                {
                    System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(user.email);
                }
                catch (Exception)
                {
                    Dictionary<string, string> msg = new Dictionary<string, string>
                    {
                        {"Message", MessageService.Custom("Formato de Email inválido").text},
                    };
                    return msg;
                }

                // percorre todos os id's de utilizadores
                for (int i = 0; i < userDB.Length; i++)
                {
                    // verificar se email inserido corresposnde ao da BD,
                    // e se password inserida (encriptada) corresponde à da BD
                    if (user.email == userDB[i].email &&
                        HashPassword.VerifyHash(user.password, userDB[i].password))
                    {
                        token = new Dictionary<string, string>
                        {
                            {"Token", TokenManager.GenerateToken(userDB[i].email, userDB[i].userType, userDB[i].idUser)},
                        };
                    }
                }
                return token;
            }
        }
    }
}
