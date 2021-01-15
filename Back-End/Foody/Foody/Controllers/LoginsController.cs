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
        public IDictionary<string, string> Post([FromBody] Utilizador utilizador)
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array 
                var utilizadorDB = db.utilizador.ToArray();

                Dictionary<string, string> token = new Dictionary<string, string>
                {
                    {"Message", MessageService.CustomMessage("Dados inválidos").text},
                };

                if (utilizador.telemovel != 0)
                {
                    for (int i = 0; i < utilizadorDB.Length; i++)
                    {
                        // verificar se email inserido corresposnde ao da BD,
                        // e se password inserida (encriptada) corresponde à da BD
                        if (utilizador.telemovel == utilizadorDB[i].telemovel && utilizador.tipoUtilizador == utilizadorDB[i].tipoUtilizador &&
                            HashPassword.VerifyHash(utilizador.password, utilizadorDB[i].password))
                        {
                            token = new Dictionary<string, string>
                            {
                                {"Token", TokenManager.GenerateToken(utilizadorDB[i].email, utilizadorDB[i].tipoUtilizador, utilizadorDB[i].idUtilizador)},
                            };
                            return token;
                        }
                    }
                }

                //valida o email
                try
                {
                    System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(utilizador.email);
                }
                catch (Exception)
                {
                    Dictionary<string, string> msg = new Dictionary<string, string>
                    {
                        {"Message", MessageService.CustomMessage("Formato de Email inválido").text},
                    };
                    return msg;
                }

                // percorre todos os id's de utilizadores
                for (int i = 0; i < utilizadorDB.Length; i++)
                {
                    // verificar se email inserido corresposnde ao da BD,
                    // e se password inserida (encriptada) corresponde à da BD
                    if (utilizador.email == utilizadorDB[i].email &&
                        HashPassword.VerifyHash(utilizador.password, utilizadorDB[i].password))
                    {
                        token = new Dictionary<string, string>
                        {
                            {"Token", TokenManager.GenerateToken(utilizadorDB[i].email, utilizadorDB[i].tipoUtilizador, utilizadorDB[i].idUtilizador)},
                        };
                    }
                }
                return token;
            }
        }
    }
}
