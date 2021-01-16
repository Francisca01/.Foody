using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class OrderProductService
    {
        public static object VerifyOrderProduct(int[] userLogin, Produto produto, bool editar)
        {
            if (userLogin != null && userLogin[1] == 2)
            {
                produto.idUtilizador = userLogin[0];

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

                        //valida se o nome do produto introduzido já exista na empresa
                        for (int i = 0; i < nomeProdutos.Count; i++)
                        {
                            if (nomeProdutos[i] == produto.nome)
                            {
                                return MessageService.CustomMessage("O Produto com o nome: " + produto.nome + " já existe na sua empresa!");
                            }
                        }
                    }

                    using (var db = new DbHelper())
                    {
                        if (editar == true)
                        {
                            db.produto.Update(produto);
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

        public static bool VerifyOrderProductAccess(int userId, int accessOrderProductId)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    var orderProduct = db.encomendaProduto.Find(accessOrderProductId);

                    if (orderProduct != null)
                    {
                        foreach (var product in db.produto.ToArray())
                        {
                            if (product.idProduto == orderProduct.idProduto)
                            {
                                if (userId == product.idUtilizador)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
