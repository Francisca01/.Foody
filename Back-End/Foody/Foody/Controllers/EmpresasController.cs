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
    public class EmpresasController : ControllerBase
    {
        // GET: api/<EmpresasController>
        [HttpGet]
        public Empresa[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.empresas.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EmpresasController>/5
        [HttpGet("{id}")]
        public Empresa Get(int id)
        {

            using (var db = new DbHelper())
            {
                var empresas = db.empresas.ToArray();

                for (int i = 0; i <= empresas.Length; i++)
                {

                    if (empresas[i].idEmpresa == id)
                    {
                        return empresas[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Empresa Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.empresas.Find(id);
            }
        }
         */

        // POST api/<EmpresasController>
        [HttpPost]
        public string Post([FromBody] Empresa novaEmpresa)
        {
            using (var db = new DbHelper())
            {
                var empresas = db.empresas.ToArray();

                for (int i = 0; i < empresas.Length; i++)
                {

                    if (novaEmpresa.idEmpresa == empresas[i].idEmpresa)
                    {
                        return "Já existe";
                    }
                }

                db.empresas.Add(novaEmpresa);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Empresa novaEmpresa)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.empresas.Add(novaEmpresa);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<EmpresasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Empresa empresaUpdate)
        {
            using (var db = new DbHelper())
            {
                var criadorDB = db.empresas.Find(id);

                if (criadorDB == null)
                {
                    Post(empresaUpdate);
                }
                else
                {
                    criadorDB.idEmpresa = id;

                    db.empresas.Update(criadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<EmpresasController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var criadorDB = db.empresas.Find(id);

                if (criadorDB != null)
                {
                    db.empresas.Remove(criadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Empresa com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
