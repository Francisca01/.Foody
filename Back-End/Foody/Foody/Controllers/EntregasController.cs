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
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.entrega.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EntregasController>/5
        [HttpGet("{idEntrega}")]
        public Entrega Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.entrega.Find(id);

                // ou: maneira mais complexa
                /* var entregasDB = db.entrega.ToArray();

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

        // POST api/<EntregasController>
        [HttpPost]
        public string Post([FromBody] Entrega novaEntrega)
        {
            // verificar se valores da nova entrega são nulos,
            // e se 
            if (novaEntrega != null && !string.IsNullOrEmpty(novaEntrega.Estado))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // converte-os (dados) num array
                    var entregasDB = db.entrega.ToArray();

                    // verifica se id de entrega já existe na BD
                    for (int i = 0; i < entregasDB.Length; i++)
                    {
                        if (novaEntrega.idEntrega == entregasDB[i].idEntrega)
                        {
                            return "Já existe";
                        }
                    }

                    // se não existir, adiciona um novo entrega
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
            // verificar se utilizado logado é entrega
            if (entregaUpdate != null && entregaUpdate.idEntrega == idUtilizador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var entregasDB = db.entrega.Find(entregaUpdate.idEntrega);

                    // se entrega não existir, criar novo
                    if (entregasDB == null)
                    {

                        Post(entregaUpdate);
                    }
                    // se entrega existir, atualizar dados
                    else
                    {
                        entregasDB.idEntrega = entregaUpdate.idEntrega;

                        db.entrega.Update(entregasDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        //NOTA: Podem ser eliminadas, mas após entrega ser concluida
        // Adicionar um status à entrega, quando concluido, eliminada


        // Por questões de política e privacidade de dados: Entregas não podem ser eliminadas

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
