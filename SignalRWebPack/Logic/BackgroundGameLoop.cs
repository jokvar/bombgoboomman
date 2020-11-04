using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace SignalRWebPack.Logic
{
    public class BackgroundGameLoop : BackgroundService
    {
        private readonly IGameLogic gameLogic;

        public BackgroundGameLoop(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await gameLogic._GameLoop(cancellationToken);
        }
    }
}
