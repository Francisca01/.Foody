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
    public class ProdutosController : ControllerBase
    {
        // GET: api/<ProdutosController>
        [HttpGet]
        public Produto[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.produto.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ProdutosController>/5
        [HttpGet("{id}")]
        public Produto Get(int id)
        {

            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.ToArray();

                for (int i = 0; i <= produtosDB.Length; i++)
                {

                    if (produtosDB[i].idProduto == id)
                    {
                        return produtosDB[i];
                    }
                }

                return null;
            }
        }
        
        // POST api/<ProdutosController>
        [HttpPost]
        public string Post(int idEmpresa, [FromBody] Produto produto, bool editar)
        {
            using (var db = new DbHelper())
            {
                //valores aceites para o nome
                var regexNome = new Regex("^[a-zA-Z ]*$");

                //array de produtos da base de dados
                var produtos = db.produto.ToArray();

                //valida os campos de produto
                if (produto != null && produto.nome != null && regexNome.IsMatch(produto.nome) &&
                    produto.precoUnitario > 0.00)
                {

                    //array para guardar o nome de todos os produtos da empresa
                    List<string> nomeProdutos = new List<string>();

                    int j = 0;

                    //criação do array dos produtos da empresa
                    for (int i = 0; i < produtos.Length; i++)
                    {
                        if (produtos[i].idEmpresa == idEmpresa)
                        {
                            nomeProdutos[j] = produtos[i].nome;
                            j++;
                        }
                    }

                    //valida se o nome do produto introduzido já exista na empresa
                    for (int i = 0; i < nomeProdutos.Count; i++)
                    {
                        if (nomeProdutos[i] == produto.nome)
                        {
                            return "O Produto com o nome: " + produto.nome + " já existe na sua empresa!";
                        }
                    }

                    if (editar == true)
                    {
                        db.produto.Update(produto);
                        db.SaveChanges();

                        return "Editado";
                    }
                    else
                    {
                        produto.idEmpresa = idEmpresa;

                        db.produto.Add(produto);
                        db.SaveChanges();

                        return "Produto Criado!";
                    }
                }
                else
                {
                    return "Os campos obrigatórios não foram preenchidos";
                }
            }
        }

        // PUT api/<ProdutosController>/5
        [HttpPut("{idProduto}")]
        public string Put(int idEmpresa, [FromBody] Produto editarProduto)
        {
            using (var db = new DbHelper())
            {
                //procura pelo produto na base de dados
                var produtoDB = db.produto.Find(editarProduto.idProduto);

                if (produtoDB != null)
                {
                    if (Post(idEmpresa, editarProduto, true) == "Ok")
                    {
                        return "Produto Alterado";
                    }
                    else
                    {
                        return "Não foi possivel criar o produto";
                    }
                }
                else
                {
                    return "Produto não encontrado!";
                }
            }
        }

        // DELETE api/<ProdutosController>/5
        [HttpDelete("{id}")]
        public string Delete(int idEmpresa, int id)
        {
            using (var db = new DbHelper())
            {
                //procura pelo produto na base de dados
                var produtosDB = db.produto.Find(id);

                if (produtosDB != null && produtosDB.idEmpresa == idEmpresa)
                {
                    db.produto.Remove(produtosDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O produto com o id: " + id + " não foi encontrado!";
                }
            }
        }
    }
}
