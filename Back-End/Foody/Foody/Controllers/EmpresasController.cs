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
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.empresa.ToArray();
            }
        }

        // GET api/<EmpresasController>/5
        [HttpGet("{id}")]
        public Empresa Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.empresa.Find(id);

                //ou: maneira mais complexa
                /*  var empresa = db.empresa.ToArray();

                for (int i = 0; i <= empresa.Length; i++)
                {

                    if (empresa[i].idEmpresa == id)
                    {
                        return empresa[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<EmpresasController>
        [HttpPost]
        public string Post([FromBody] Empresa novaEmpresa)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var empresa = db.empresa.ToArray();

                // verifica se id de empresa já existe na BD
                for (int i = 0; i < empresa.Length; i++)
                {
                    if (novaEmpresa.idEmpresa == empresa[i].idEmpresa)
                    {
                        return "Já existe!";
                    }
                }

                // se não existir, adiciona um novo cliente
                db.empresa.Add(novaEmpresa);
                db.SaveChanges();

                return "Criado!";
            }
        }

        // PUT api/<EmpresasController>/5
        [HttpPut("{id}")]
        public void Put(int idEmpresa, [FromBody] Empresa empresaUpdate)
        {
            // verificar se utilizado logado é empresa
            if (empresaUpdate != null && empresaUpdate.idEmpresa == idEmpresa)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var empresaDB = db.empresa.Find(idEmpresa);

                    // se empresa não existir, cria uma nova
                    if (empresaDB == null)
                    {
                        Post(empresaUpdate);
                    }

                    // se empresa existir, atualizar os dados
                    else
                    {
                        empresaDB.idEmpresa = idEmpresa;

                        db.empresa.Update(empresaDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        /*  // PUT api/<EmpresasController>/5
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
        }   */

        // DELETE api/<EmpresasController>/5
        [HttpDelete("{idEmpresa}")]
        public string Delete(int idEmpresa, int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id da empresa
                var empresaDB = db.empresa.Find(id);

                // se id encontrado (diferente de nulo), 
                // remove o empresa associada
                if (empresaDB != null)
                {
                    // os produtos associados à empresa tem de ser eliminados
                    // adiciona os dados dos produtos a um array
                    var produto = db.produto.ToArray();

                    // percorre os produtos
                    for (int i = 0; i < produto.Length; i++)
                    {
                        // procura o id dos produtos
                        var produtosDB = db.produto.Find(id);

                        // verifica se produto está associado à empresa
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

                    db.empresa.Remove(empresaDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A empresa com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
