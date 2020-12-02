using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        public string Post([FromBody] Utilizador novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                var utilizadores = db.utilizadores.ToArray();

                if (utilizadores != null)
                {
                    for (int i = 0; i < utilizadores.Length; i++)
                    {
                        if (novoUtilizador.email == utilizadores[i].email || novoUtilizador.email == "")
                        {
                            return "O utilizador com o email: " + novoUtilizador.email + " já existe";
                        }
                    }

                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        novoUtilizador.palavraPasse = HashPassword.GetHash(sha256Hash, novoUtilizador.palavraPasse);
                    }

                    db.utilizadores.Add(novoUtilizador);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            return "Erro";
        }
    }
}
