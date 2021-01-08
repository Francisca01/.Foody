using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class MessageService
    {
        public static object MessagemSemResultados()
        {
            object message = new { message = "Não foi encontrado nenhum resultado!" };

            return message;
        }
        public static object MessagemCustomizada(string msg)
        {
            object message = new { message = msg };

            return message;
        }
    }
}
