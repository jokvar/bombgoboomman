using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserver : IObserver
    {
        public void Update(ISubject subject)
        {
            int count = (subject as Session).Players.Where(p => p.lives > 0).ToList().Count;

            if (count == 1)
            {
                throw new NotImplementedException();
            }
        }
    }
}
