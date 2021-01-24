using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foody.Utils
{
    public class OrderService
    {
        public static Message VerifyOrder(int[] userLoggedIn, OrderProduct orderProduct, bool edit, int orderId, int idProduct)
        {
            //verifica se é cliente
            if (userLoggedIn[1] == 0)
            {
                if (orderProduct != null)
                {
                    using (DbHelper db = new DbHelper())
                    {
                        if (edit == true)
                        {
                            if (VerifyOrderAccess(userLoggedIn[0], orderId))
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
                            order.idClient = userLoggedIn[0];
                            order.state = 0;

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

        public static Message ChangePayment(int[] userLoggedIn, int accessOrderId)
        {
            if (VerifyOrderAccess(userLoggedIn[0], accessOrderId))
            {
                using (DbHelper db = new DbHelper())
                {
                    var orderDB = db.order.Find(accessOrderId);

                    if (orderDB != null)
                    {
                        orderDB.state = 1;

                        db.order.Update(orderDB);
                        db.SaveChanges();

                        //após o pagamento feito é crio Delivery
                        Delivery delivery = new Delivery();
                        delivery.idOrder = orderDB.idOrder;

                        DeliveryService.CreateEditDelivery(userLoggedIn[1], delivery, -1);

                        return MessageService.Custom("Pagamento feito!");
                    }
                    else
                    {
                        return MessageService.WithoutResults();
                    }
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        public static List<object> GetOrdersUserId()
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

        public static Message DeleteOrder(int userLoggedIn, int idOrder, int idProduct)
        {
            bool eliminarProduto = false;
            using (DbHelper db = new DbHelper())
            {
                if (VerifyOrderAccess(userLoggedIn, idOrder))
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
                        eliminarProduto = true;
                        var orderProductDBt = db.orderProduct.Find(idOrder, idProduct);
                        db.orderProduct.Remove(orderProductDBt);
                    }

                    db.SaveChanges();

                    if (eliminarProduto)
                    {
                        return MessageService.Custom("Produto Eliminado de Encomenda");
                    }
                    else
                    {
                        return MessageService.Custom("Encomenda Eliminada");
                    }
                }
                else
                {
                    return MessageService.AccessDenied();
                }
            }
        }
    }
}
