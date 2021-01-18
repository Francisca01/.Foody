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
        [HttpGet("{idDelivery}")]
        public Delivery Get(int idDelivery)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.delivery.Find(idDelivery);

                // ou: maneira mais complexa
                /* var entregasDB = db.delivery.ToArray();

                for (int i = 0; i <= entregasDB.Length; i++)
                {

                    if (entregasDB[i].idDelivery == id)
                    {
                        return entregasDB[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<DeliveriesController>
        [HttpPost]
        public string Post([FromBody] Delivery newDelivery)
        {
            // verificar se valores da nova delivery são nulos,
            // e se 
            if (newDelivery != null && !string.IsNullOrEmpty(newDelivery.state))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // converte-os (dados) num array
                    var entregasDB = db.delivery.ToArray();

                    // verifica se id de delivery já existe na BD
                    for (int i = 0; i < entregasDB.Length; i++)
                    {
                        if (newDelivery.idDelivery == entregasDB[i].idDelivery)
                        {
                            return "Já existe";
                        }
                    }

                    // se não existir, adiciona um novo delivery
                    db.delivery.Add(newDelivery);
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
        [HttpPut("{idDelivery}")]
        public void Put(int idUser, [FromBody] Delivery deliveryUpdate)
        {
            // verificar se utilizado logado é delivery
            if (deliveryUpdate != null && deliveryUpdate.idDelivery == idUser)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var entregasDB = db.delivery.Find(deliveryUpdate.idDelivery);

                    // se delivery não existir, criar novo
                    if (entregasDB == null)
                    {

                        Post(deliveryUpdate);
                    }
                    // se delivery existir, atualizar dados
                    else
                    {
                        entregasDB.idDelivery = deliveryUpdate.idDelivery;

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
        [HttpDelete("{idDelivery}")]
        public string Delete(int idDelivery)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.delivery.Find(idDelivery);

                if (entregasDB != null)
                {
                    db.delivery.Remove(entregasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A delivery com o id: " + idDelivery + " não foi encontrada";
                }
            }
        }*/
    }
}
