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
        public Utilizador[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.utilizador.ToArray();
            }
        }

        // GET api/<EmpresasController>/5
        [HttpGet("{id}")]
        public Utilizador Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.utilizador.Find(id);

                //ou: maneira mais complexa
                /*  var utilizador = db.utilizador.ToArray();

                for (int i = 0; i <= utilizador.Length; i++)
                {

                    if (utilizador[i].idUtilizador == id)
                    {
                        return utilizador[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<EmpresasController>
        [HttpPost]
        public string Post([FromBody] Utilizador novaEmpresa)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var utilizador = db.utilizador.ToArray();

                // verifica se id de utilizador já existe na BD
                for (int i = 0; i < utilizador.Length; i++)
                {
                    if (novaEmpresa.idUtilizador == utilizador[i].idUtilizador)
                    {
                        return "Já existe!";
                    }
                }

                // se não existir, adiciona um novo cliente
                db.utilizador.Add(novaEmpresa);
                db.SaveChanges();

                return "Criado!";
            }
        }

        // PUT api/<EmpresasController>/5
        [HttpPut("{id}")]
        public void Put(int idUtilizador, [FromBody] Utilizador empresaUpdate)
        {
            // verificar se utilizado logado é utilizador
            if (empresaUpdate != null && empresaUpdate.idUtilizador == idUtilizador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var empresaDB = db.utilizador.Find(idUtilizador);

                    // se utilizador não existir, cria uma nova
                    if (empresaDB == null)
                    {
                        Post(empresaUpdate);
                    }

                    // se utilizador existir, atualizar os dados
                    else
                    {
                        empresaDB.idUtilizador = idUtilizador;

                        db.utilizador.Update(empresaDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        /*  // PUT api/<EmpresasController>/5
        [HttpPut("{idProduto}")]
        public void Put(int idUtilizador, [FromBody] Produto editarProduto)
        {
            if (editarProduto != null && editarProduto.idUtilizador == idUtilizador)
            {
                using (var db = new DbHelper())
                {
                    //procura pelo produto na base de dados
                    var produtoDB = db.produto.Find(editarProduto.idProduto);

                    if (produtoDB != null)
                    {
                        //if (CreateProduct(idUtilizador, editarProduto, true) == "Ok")
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
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador, int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id da utilizador
                var empresaDB = db.utilizador.Find(id);

                // se id encontrado (diferente de nulo), 
                // remove o utilizador associada
                if (empresaDB != null)
                {
                    // os produtos associados à utilizador tem de ser eliminados
                    // adiciona os dados dos produtos a um array
                    var produto = db.produto.ToArray();

                    // percorre os produtos
                    for (int i = 0; i < produto.Length; i++)
                    {
                        // procura o id dos produtos
                        var produtosDB = db.produto.Find(id);

                        // verifica se produto está associado à utilizador
                        if (produtosDB != null && produtosDB.idEmpresa == idUtilizador)
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

                    db.utilizador.Remove(empresaDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A utilizador com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
