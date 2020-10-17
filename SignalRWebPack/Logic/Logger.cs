using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Logic
{
    public interface ISpecializedLogger
    {
        void Log(string message);
    }

    public class SpecializedLogger : ISpecializedLogger
    {
        private readonly ILogger _logger;
        public SpecializedLogger(ILogger<GameLogic> logger)
        {
            _logger = logger;
        }
        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
