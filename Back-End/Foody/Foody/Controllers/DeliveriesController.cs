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
    public class DeliveriesController : ControllerBase
    {
        // GET: api/<DeliveriesController>
        [HttpGet]
        public Delivery[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.delivery.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<DeliveriesController>/5
        [HttpGet("{idEntrega}")]
        public Delivery Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.delivery.Find(id);

                // ou: maneira mais complexa
                /* var entregasDB = db.delivery.ToArray();

                for (int i = 0; i <= entregasDB.Length; i++)
                {

                    if (entregasDB[i].idEntrega == id)
                    {
                        return entregasDB[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<DeliveriesController>
        [HttpPost]
        public string Post([FromBody] Delivery novaEntrega)
        {
            // verificar se valores da nova delivery são nulos,
            // e se 
            if (novaEntrega != null && !string.IsNullOrEmpty(novaEntrega.Estado))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // converte-os (dados) num array
                    var entregasDB = db.delivery.ToArray();

                    // verifica se id de delivery já existe na BD
                    for (int i = 0; i < entregasDB.Length; i++)
                    {
                        if (novaEntrega.idEntrega == entregasDB[i].idEntrega)
                        {
                            return "Já existe";
                        }
                    }

                    // se não existir, adiciona um novo delivery
                    db.delivery.Add(novaEntrega);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Não foi recebido qualquer tipo de dados!";
            }
        }

        // PUT api/<DeliveriesController>/5
        [HttpPut("{idEntrega}")]
        public void Put(int idUtilizador, [FromBody] Delivery entregaUpdate)
        {
            // verificar se utilizado logado é delivery
            if (entregaUpdate != null && entregaUpdate.idEntrega == idUtilizador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var entregasDB = db.delivery.Find(entregaUpdate.idEntrega);

                    // se delivery não existir, criar novo
                    if (entregasDB == null)
                    {

                        Post(entregaUpdate);
                    }
                    // se delivery existir, atualizar dados
                    else
                    {
                        entregasDB.idEntrega = entregaUpdate.idEntrega;

                        db.delivery.Update(entregasDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        //NOTA: Podem ser eliminadas, mas após delivery ser concluida
        // Adicionar um status à delivery, quando concluido, eliminada


        // Por questões de política e privacidade de dados: Entregas não podem ser eliminadas

        /*
        // DELETE api/<DeliveriesController>/5
        [HttpDelete("{idEntrega}")]
        public string Delete(int idEntrega)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.delivery.Find(idEntrega);

                if (entregasDB != null)
                {
                    db.delivery.Remove(entregasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A delivery com o id: " + idEntrega + " não foi encontrada";
                }
            }
        }*/
    }
}
