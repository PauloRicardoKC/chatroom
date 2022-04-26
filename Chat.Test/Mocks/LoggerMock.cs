using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Chat.Test.Mocks
{
    [ExcludeFromCodeCoverage]
    public class LoggerMock<T> : ILogger<T>, IDisposable
    {
        public LogLevel LastLogLevel { get; private set; }
        public string LogMessage { get; private set; } = string.Empty;
        private ITestOutputHelper _output;

        public LoggerMock(ITestOutputHelper output)
        {
            _output = output;   
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine(state.ToString());
            LogMessage = LogMessage + " " + state.ToString();
            LastLogLevel = logLevel;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {            
        }
    }
}