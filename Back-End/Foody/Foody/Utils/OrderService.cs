using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foody.Utils
{
    public class OrderService
    {
        public static Message VerifyOrder(int[] userLogin, OrderProduct orderProduct, bool edit, int orderId, int idProduct)
        {
            //verifica se é cliente
            if (userLogin[1] == 0)
            {
                if (orderProduct != null)
                {
                    using (DbHelper db = new DbHelper())
                    {
                        if (edit == true)
                        {
                            if (VerifyOrderAccess(userLogin[0], orderId))
                            {
                                orderProduct.idOrder = orderId;
                                orderProduct.idProduct = idProduct;
                                OrderProductService.VerifyOrderProduct(orderProduct, true, db);
                                return MessageService.Custom("Encomenda Editada!");
                            }
                            else
                            {
                                return MessageService.AccessDenied();
                            }
                        }
                        else
                        {
                            Order order = new Order();
                            order.idClient = userLogin[0];

                            //verifica se o product é para adicionar a order
                            if (orderProduct.idOrder == 0)
                            {
                                db.order.Add(order);
                                db.SaveChanges();

                                orderProduct.idOrder = order.idOrder;

                                OrderProductService.VerifyOrderProduct(orderProduct, false, db);
                            }

                            //verifica se a order que está a chamar existe
                            if (db.order.Find(orderProduct.idOrder) != null)
                            {
                                return OrderProductService.VerifyOrderProduct(orderProduct, false, db);
                            }

                            return MessageService.WithoutResults();
                        }
                    }
                }
                else
                {
                    return MessageService.Custom("Complete todos os Campos!");
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        public static bool VerifyOrderAccess(int userId, int accessOrderId)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    //verifica se o utilizador logado tem alguma order
                    foreach (var order in db.order.ToArray())
                    {
                        if (order.idClient == userId)
                        {
                            if (order.idOrder == accessOrderId)
                            {
                                return true;
                            }
                            if (accessOrderId == -1)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<object> GetOrdersUserId(int userId)
        {
            using (DbHelper db = new DbHelper())
            {
                List<object> orders = new List<object>();

                foreach (var order in db.order.ToArray())
                {
                    orders.Add(order);
                }

                return orders;
            }
        }

        public static Order GetOrderId(int idOrder)
        {
            using (DbHelper db = new DbHelper())
            {
                return db.order.Find(idOrder);
            }
        }

        public static Message DeleteOrder(int userLogin, int idOrder, int idProduct)
        {
            using (DbHelper db = new DbHelper())
            {
                if (VerifyOrderAccess(userLogin, idOrder))
                {
                    //elimina todos os OrderProducts e o Order
                    if (idProduct == -1)
                    {
                        foreach (var orderProductDB in db.orderProduct.ToArray())
                        {
                            if (orderProductDB.idOrder == idOrder)
                            {
                                db.orderProduct.Remove(orderProductDB);
                                db.SaveChanges();
                            }
                        }

                        var orderDB = db.order.Find(idOrder);
                        db.order.Remove(orderDB);
                    }
                    else //elimina o OrderProduct
                    {
                        var orderProductDBt = db.orderProduct.Find(idOrder, idProduct);
                        db.orderProduct.Remove(orderProductDBt);
                    }

                    db.SaveChanges();

                    return MessageService.Custom("Encomenda Eliminada");
                }
                else
                {
                    return MessageService.AccessDenied();
                }
            }
        }
    }
}
