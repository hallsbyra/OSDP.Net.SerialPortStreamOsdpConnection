using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSDP.Net.Connections
{
    public class SerialPortStreamOsdpConnection : IOsdpConnection
    {
        readonly SerialPortStream serialPort;
        readonly ILogger logger;

        public SerialPortStreamOsdpConnection(string portName, int baudRate, ILogger logger = null)
        {
            serialPort = logger == null ? new SerialPortStream() : new SerialPortStream(logger);
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = 100;
            this.logger = logger;
        }

        public int BaudRate => serialPort.BaudRate;

        public bool IsOpen => serialPort.IsOpen;

        public TimeSpan ReplyTimeout { get; set; } = TimeSpan.FromMilliseconds(200);

        public void Close()
        {
            logger?.LogDebug("SerialPortStreamOsdpConnection.Close() Port: {0}", serialPort.PortName);
            serialPort.Close();
        }

        public void Open()
        {
            logger?.LogDebug("SerialPortStreamOsdpConnection.Open() Port: {0}", serialPort.PortName);
            serialPort.Open();
        }

        public async Task<int> ReadAsync(byte[] buffer, CancellationToken token)
        {
            logger?.LogDebug("SerialPortStreamOsdpConnection.ReadAsync() Port: {0}", serialPort.PortName);
            while (true)
            {
                // SerialPortStream ignores the passed CancellationToken. However, the ReadTimeout parameter
                // seems to work, so we use it in a loop to simulate real cancellation.
                var count = await serialPort.ReadAsync(buffer, 0, buffer.Length, token);
                if (count > 0)
                {
                    logger?.LogDebug("SerialPortStreamOsdpConnection.ReadAsync() Port: {0}, Count: {1}", serialPort.PortName, count);
                    return count;                    
                }
                logger?.LogDebug("SerialPortStreamOsdpConnection.ReadAsync() Port: {0} - Timed out", serialPort.PortName);
                token.ThrowIfCancellationRequested();
            }
        }

        public async Task WriteAsync(byte[] buffer)
        {
            logger?.LogDebug("SerialPortStreamOsdpConnection.WriteAsync({0} bytes) Port: {1}", buffer.Length, serialPort.PortName);
            await serialPort.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}