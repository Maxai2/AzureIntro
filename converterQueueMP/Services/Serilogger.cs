using ConverterApp.Interfaces;
using Serilog;
using Serilog.Core;

namespace ConverterApp.Services {
    public class Serilogger : ConverterApp.Interfaces.ILogger {
        private Logger _logger;
        public Serilogger () {
            _logger = new LoggerConfiguration ().WriteTo.Console ().CreateLogger ();
        }

        public void Debug (string text) {
            _logger.Debug (text);
        }

        public void Error (string text) {
            _logger.Error (text);
        }

        public void Information (string text) {
            _logger.Information (text);
        }
    }
}