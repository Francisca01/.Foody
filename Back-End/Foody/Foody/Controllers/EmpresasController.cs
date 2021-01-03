using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                return db.empresa.ToArray();
            }
        }

        // GET api/<EmpresasController>/5
        [HttpGet("{id}")]
        public Empresa Get(int id)
        {
            using (var db = new DbHelper())
            {
                var empresa = db.empresa.ToArray();

                for (int i = 0; i <= empresa.Length; i++)
                {

                    if (empresa[i].idEmpresa == id)
                    {
                        return empresa[i];
                    }
                }

                return null;
            }
        }

        // PUT api/<EmpresasController>/5
        [HttpPut("{idProduto}")]
        public void Put(int idEmpresa, [FromBody] Produto editarProduto)
        {
            if (editarProduto != null && editarProduto.idEmpresa == idEmpresa)
            {
                using (var db = new DbHelper())
                {
                    //procura pelo produto na base de dados
                    var produtoDB = db.produto.Find(editarProduto.idProduto);

                    if (produtoDB != null)
                    {
                        //if (CreateProduct(idEmpresa, editarProduto, true) == "Ok")
                        //{
                        //    return "Produto Alterado";
                        //}
                        //else
                        //{
                        //    return "Não foi possivel criar o produto";
                        //}
                    }
                    else
                    {

                    }
                }
            }
        }

        // DELETE api/<EmpresasController>/5
        [HttpDelete("{idEmpresa}")]
        public string Delete(int idEmpresa, int id)
        {
            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.Find(id);

                if (produtosDB != null && produtosDB.idEmpresa == idEmpresa)
                {
                    db.produto.Remove(produtosDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O produto com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
