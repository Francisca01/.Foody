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
    public class EntregasController : ControllerBase
    {
        // GET: api/<EntregasController>
        [HttpGet]
        public Entrega[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.entrega.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EntregasController>/5
        [HttpGet("{idEntrega}")]
        public Entrega Get(int id)
        {

            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.ToArray();

                for (int i = 0; i <= entregasDB.Length; i++)
                {

                    if (entregasDB[i].idEntrega == id)
                    {
                        return entregasDB[i];
                    }
                }

                return null;
            }
        }

        // POST api/<EntregasController>
        [HttpPost]
        public string Post([FromBody] Entrega novaEntrega)
        {
            if (novaEntrega != null && !string.IsNullOrEmpty(novaEntrega.Estado))
            {
                using (var db = new DbHelper())
                {
                    var entregasDB = db.entrega.ToArray();

                    for (int i = 0; i < entregasDB.Length; i++)
                    {
                        if (novaEntrega.idEntrega == entregasDB[i].idEntrega)
                        {
                            return "Já existe";
                        }
                    }

                    db.entrega.Add(novaEntrega);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Não foi recebido qualquer tipo de dados!";
            }
        }

        // PUT api/<EntregasController>/5
        [HttpPut("{idEntrega}")]
        public void Put(int idUtilizador, [FromBody] Entrega entregaUpdate)
        {
            if (entregaUpdate != null && entregaUpdate.idEntrega == idUtilizador)
            {
                using (var db = new DbHelper())
                {
                    var entregasDB = db.entrega.Find(entregaUpdate.idEntrega);

                    if (entregasDB == null)
                    {

                        Post(entregaUpdate);
                    }
                    else
                    {
                        entregasDB.idEntrega = entregaUpdate.idEntrega;

                        db.entrega.Update(entregasDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        /*
        // DELETE api/<EntregasController>/5
        [HttpDelete("{idEntrega}")]
        public string Delete(int idEntrega)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.Find(idEntrega);

                if (entregasDB != null)
                {
                    db.entrega.Remove(entregasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A entrega com o id: " + idEntrega + " não foi encontrada";
                }
            }
        }*/
    }
}
