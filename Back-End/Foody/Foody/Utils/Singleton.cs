using Foody.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class Singleton
    {
        private readonly ISingleton _iSingleton;

        //public Singleton(ISingleton iSingleton)
        //{
        //    _iSingleton = iSingleton;
        //}

        public void add(string name)
        {
            _iSingleton.Insert(name);
        }
    }
}
