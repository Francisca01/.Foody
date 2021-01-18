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
        public static Message VerifyOrderProduct(OrderProduct orderProduct, bool edit, DbHelper db)
        {
            //verifica se o produto que esta a tentar adicionar a order já existe
            var orderProductDB = db.orderProduct.Find(orderProduct.idOrder, orderProduct.idProduct);

            //valida os campos de product
            if (orderProduct != null)
            {
                if (orderProduct.quantity > 0)
                {
                    if (edit)
                    {
                        return Edit(orderProductDB, db, orderProduct);
                    }
                    else
                    {
                        if (db.orderProduct.Find(orderProduct.idOrder, orderProduct.idProduct) != null)
                        {
                            return Edit(orderProductDB, db, orderProduct);
                        }
                        else
                        {
                            db.orderProduct.Add(orderProduct);
                            db.SaveChanges();
                        }

                        return MessageService.Custom("Encomenda Criada");
                    }

                }
                else
                {
                    return MessageService.Custom("Complete todos os campos!");
                }
            }
            else
            {
                return MessageService.Custom("Complete todos os campos!");
            }
        }
        public static Message Edit(OrderProduct orderProductDB, DbHelper db, OrderProduct orderProduct)
        {
            orderProductDB.quantity = orderProduct.quantity;

            db.orderProduct.Update(orderProductDB);
            db.SaveChanges();

            return MessageService.Custom("Encomenda Criada");
        }
    }
}
