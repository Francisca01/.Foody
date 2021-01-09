using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class MessageService
    {
        public static Message WithoutResultsMessage()
        {
            Message message = new Message();
            message.text = "Não foi encontrado nenhum resultado!";

            return message;
        }
        public static Message AccessDeniedMessage()
        {
            Message message = new Message();
            message.text = "Não tem permissões para aceder!";

            return message;
        }

        public static Message CustomMessage(string msg)
        {
            Message message = new Message();
            message.text = msg;

            return message;
        }
    }
}
