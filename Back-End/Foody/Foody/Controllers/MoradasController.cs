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
    public class MoradasController : ControllerBase
    {
        // GET: api/<MoradasController>
        [HttpGet]
        public Morada[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.morada.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<MoradasController>/5
        [HttpGet("{idMorada}")]
        public Morada Get(int idMorada)
        {

            using (var db = new DbHelper())
            {
                var Morada = db.morada.ToArray();

                for (int i = 0; i <= Morada.Length; i++)
                {

                    if (Morada[i].idMorada == idMorada)
                    {
                        return Morada[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Morada Get(int idMorada)
        {
            using (var db = new DbHelper())
            {
                return db.Morada.Find(idMorada);
            }
        }
         */

        // POST api/<MoradasController>
        [HttpPost]
        public string Post([FromBody] Morada novoMorada)
        {
            using (var db = new DbHelper())
            {
                var Morada = db.morada.ToArray();

                for (int i = 0; i < Morada.Length; i++)
                {

                    if (novoMorada.idMorada == Morada[i].idMorada)
                    {
                        return "Já existe";
                    }
                }

                db.morada.Add(novoMorada);
                db.SaveChanges();

                return "Criado";
            }
        }
    }
}
