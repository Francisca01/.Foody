using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class ProductService
    {
        public static object VerifyProduct(int[] userLogin, Produto produto, bool editar, int idProduto)
        {
            if (userLogin != null && userLogin[1] == 2)
            {
                //valida os campos de produto
                if (produto != null && !string.IsNullOrEmpty(produto.nome) &&
                    produto.precoUnitario > 0.00)
                {
                    //lista para guardar o nome de todos os produtos da empresa
                    List<string> nomeProdutos = new List<string>();

                    int j = 0;

                    using (var db = new DbHelper())
                    {
                        //array de produtos da base de dados
                        var produtos = db.produto.ToArray();

                        //criação do array dos produtos da empresa
                        for (int i = 0; i < produtos.Length; i++)
                        {
                            if (produtos[i].idUtilizador == userLogin[0])
                            {
                                nomeProdutos.Add(produtos[i].nome);
                                j++;
                            }
                        }
                    }

                    //valida se o nome do produto introduzido já exista na empresa
                    for (int i = 0; i < nomeProdutos.Count; i++)
                    {
                        if (nomeProdutos[i] == produto.nome)
                        {
                            return MessageService.CustomMessage("O Produto com o nome: " + produto.nome + " já existe na sua empresa!");
                        }
                    }

                    using (var db = new DbHelper())
                    {
                        if (editar == true)
                        {
                            var produtoUpdate = db.produto.Find(idProduto);

                            produtoUpdate.nome = produto.nome;
                            produtoUpdate.ingredientes = produto.ingredientes;
                            produtoUpdate.precoUnitario = produto.precoUnitario;
                            produtoUpdate.categoria = produto.categoria;

                            db.produto.Update(produtoUpdate);
                            db.SaveChanges();

                            return MessageService.CustomMessage("Produto Editado!");
                        }
                        else
                        {
                            db.produto.Add(produto);
                            db.SaveChanges();

                            return MessageService.CustomMessage("Produto Criado!");
                        }
                    }
                }
                else
                {
                    return MessageService.CustomMessage("Os campos obrigatórios não foram preenchidos ou são inválidos");
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }

    }
}
