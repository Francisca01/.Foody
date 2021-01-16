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
        public static object VerifyOrderProduct(int[] userLogin, Product product, bool editar)
        {
            if (userLogin != null && userLogin[1] == 2)
            {
                product.idUtilizador = userLogin[0];

                //valida os campos de product
                if (product != null && !string.IsNullOrEmpty(product.nome) &&
                    product.precoUnitario > 0.00)
                {
                    //lista para guardar o nome de todos os produtos da empresa
                    List<string> nomeProdutos = new List<string>();

                    int j = 0;

                    using (var db = new DbHelper())
                    {
                        //array de produtos da base de dados
                        var produtos = db.product.ToArray();

                        //criação do array dos produtos da empresa
                        for (int i = 0; i < produtos.Length; i++)
                        {
                            if (produtos[i].idUtilizador == userLogin[0])
                            {
                                nomeProdutos.Add(produtos[i].nome);
                                j++;
                            }
                        }

                        //valida se o nome do product introduzido já exista na empresa
                        for (int i = 0; i < nomeProdutos.Count; i++)
                        {
                            if (nomeProdutos[i] == product.nome)
                            {
                                return MessageService.Custom("O Product com o nome: " + product.nome + " já existe na sua empresa!");
                            }
                        }
                    }

                    using (var db = new DbHelper())
                    {
                        if (editar == true)
                        {
                            db.product.Update(product);
                            db.SaveChanges();

                            return MessageService.Custom("Product Editado!");
                        }
                        else
                        {
                            db.product.Add(product);
                            db.SaveChanges();

                            return MessageService.Custom("Product Criado!");
                        }
                    }
                }
                else
                {
                    return MessageService.Custom("Os campos obrigatórios não foram preenchidos ou são inválidos");
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        public static bool VerifyOrderProductAccess(int userId, int accessOrderProductId)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    var orderProduct = db.orderProduct.Find(accessOrderProductId);

                    if (orderProduct != null)
                    {
                        foreach (var product in db.product.ToArray())
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
