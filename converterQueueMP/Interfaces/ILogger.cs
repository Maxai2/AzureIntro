namespace ConverterApp.Interfaces {
    public interface ILogger {
        void Error (string text);
        void Information (string text);
        void Debug (string text);
    }
}