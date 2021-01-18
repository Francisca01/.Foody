using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public User[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.user.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<UsersController>/5
        [HttpGet("{idUser}")]
        public User Get(int idUser)
        {

            using (var db = new DbHelper())
            {
                var user = db.user.ToArray();

                for (int i = 0; i <= user.Length; i++)
                {

                    if (user[i].idUser == idUser)
                    {
                        return user[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public User Get(int idUser)
        {
            using (var db = new DbHelper())
            {
                return db.user.Find(idUser);
            }
        }
         */

        // POST api/<UsersController>
        [HttpPost]
        public string Post([FromBody] User newUser)
        {
            using (var db = new DbHelper())
            {
                var user = db.user.ToArray();

                for (int i = 0; i < user.Length; i++)
                {

                    if (newUser.idUser == user[i].idUser)
                    {
                        return "Já existe";
                    }
                }

                db.user.Add(newUser);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] User novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.user.Add(novoUtilizador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<UsersController>/5
        [HttpPut("{idUser}")]
        public void Put(int idUser, [FromBody] User userUpdate)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.user.Find(idUser);

                if (utilizadorDB == null)
                {
                    Post(userUpdate);
                }
                else
                {
                    utilizadorDB.idUser = idUser;

                    if (userUpdate.email != null)
                    {
                        utilizadorDB.email = userUpdate.email;
                    }

                    if (userUpdate.name != null)
                    {
                        utilizadorDB.name = userUpdate.name;
                    }

                    if (userUpdate.password != null)
                    {
                        utilizadorDB.password = userUpdate.password;
                    }

                    db.user.Update(utilizadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{idUser}")]
        public string Delete(int idUser)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.user.Find(idUser);

                if (utilizadorDB != null)
                {
                    db.user.Remove(utilizadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O User com o idUser: " + idUser + " não foi encontrado";
                }
            }
        }
    }
}
