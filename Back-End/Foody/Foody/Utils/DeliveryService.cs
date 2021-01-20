using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Foody.Models;

namespace Foody.Utils
{
    public class DeliveryService
    {
        public static Message CreateEditDelivery(int userLoggedInType, Delivery delivery, int idDelivery)
        {
            // verificar se valores da nova delivery são nulos
            if (delivery != null)
            {
                //acede a base de dados
                using (DbHelper db = new DbHelper())
                {
                    if (userLoggedInType == 0)
                    {
                        List<User> drivers = new List<User>();
                        foreach (var user in db.user.ToArray())
                        {
                            if (user.userType == 1)
                            {
                                drivers.Add(user);
                            }
                        }

                        //escolhe um condutor á sorte
                        Random rnd = new Random();
                        int driver = rnd.Next(1, drivers.Count);

                        delivery.idDriver = drivers[driver - 1].idUser;
                        delivery.state = "Em andamento";

                        //cria entrega
                        db.delivery.Add(delivery);
                        db.SaveChanges();

                        return MessageService.Custom("Entrega criadada!");
                    }

                    else
                    {
                        return MessageService.AccessDenied();
                    }
                }

            }
            else
            {
                return MessageService.WithoutResults();
            }
        }


        public static Message ChangeDeliveryState(int userLoggedInId, Delivery delivery, int idDelivery)
        {
            // verificar se valores da nova delivery são nulos
            if (delivery != null)
            {
                //acede a base de dados
                using (DbHelper db = new DbHelper())
                {
                    //vai buscar os dados da Delivery
                    var deliveryDB = db.delivery.Find(idDelivery);

                    if (deliveryDB != null)
                    {
                        //verifica se o Driver que está a aceder é o mesmo da Order
                        if (deliveryDB.idDriver == userLoggedInId)
                        {
                            if (!string.IsNullOrEmpty(delivery.state))
                            {
                                deliveryDB.state = delivery.state;

                                db.delivery.Update(deliveryDB);
                                db.SaveChanges();

                                return MessageService.Custom("Entrega editada!");
                            }
                            else
                            {
                                return MessageService.Custom("Preencha todos os dados!");
                            }
                        }
                        else
                        {
                            return MessageService.AccessDenied();
                        }
                    }
                    else
                    {
                        return MessageService.WithoutResults();
                    }
                }
            }
            else
            {
                return MessageService.Custom("Não foi redebido nenhum dado!");
            }
        }
        public static bool VerifyDeliveryAccess(string token, int accessDeliveryId)
        {
            try
            {
                if (int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[0].Value, out var iduserLoggedIn) &&
                int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[1].Value, out var userTypeLogin))
                {
                    if ((iduserLoggedIn == accessDeliveryId && userTypeLogin == 0) || userTypeLogin == 1)
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
            catch (Exception)
            {
                return false;
            }
        }

        #region Get Deliveries
        public static List<object> GetDeliveries(int[] userLoggedIn)
        {
            if (userLoggedIn[1] == 3)
            {
                List<object> deliveries = new List<object>();

                using (DbHelper db = new DbHelper())
                {
                    foreach (var delivery in db.delivery.ToList())
                    {
                        deliveries.Add(delivery);
                    }
                }
                return deliveries;
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }
        #endregion

        #region Get Delivery Id
        public static object GetDeliveryId(string token, int idDelivery)
        {
            if (VerifyDeliveryAccess(token, idDelivery))
            {
                using (DbHelper db = new DbHelper())
                {
                    var delivery = db.delivery.Find(idDelivery);

                    if (delivery != null)
                    {
                        return delivery;
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
        #endregion                
    }
}
