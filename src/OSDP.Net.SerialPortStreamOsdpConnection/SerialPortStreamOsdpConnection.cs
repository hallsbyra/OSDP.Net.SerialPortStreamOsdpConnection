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
        bool isOpen = false;

        public SerialPortStreamOsdpConnection(string portName, int baudRate, ILogger logger = null)
        {
            serialPort = logger == null ? new SerialPortStream() : new SerialPortStream(logger);
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = 100;
            this.logger = logger;
        }

        public int BaudRate => serialPort.BaudRate;

        // Keep track of the open state ourselves. The SerialPortStream.IsOpen can return false e.g.
        // after an error has occurred. Then OSDP.Net expects to be able to call Open(), but SerialPortStream
        // throws exception saying "Port is already open".
        public bool IsOpen => isOpen;

        public TimeSpan ReplyTimeout { get; set; } = TimeSpan.FromMilliseconds(200);

        public void Close()
        {
            logger?.LogTrace("SerialPortStreamOsdpConnection.Close() Port: {Port}", serialPort.PortName);
            serialPort.Close();
            isOpen = false;
        }

        public void Open()
        {
            logger?.LogTrace("SerialPortStreamOsdpConnection.Open() Port: {Port}", serialPort.PortName);
            serialPort.Open();
            isOpen = true;
        }

        public async Task<int> ReadAsync(byte[] buffer, CancellationToken token)
        {
            logger?.LogTrace("SerialPortStreamOsdpConnection.ReadAsync() Port: {Port}", serialPort.PortName);
            while (true)
            {
                // SerialPortStream ignores the passed CancellationToken. However, the ReadTimeout parameter
                // seems to work, so we use it in a loop to simulate real cancellation.
                var count = await serialPort.ReadAsync(buffer, 0, buffer.Length, token);
                if (count > 0)
                {
                    logger?.LogTrace("SerialPortStreamOsdpConnection.ReadAsync() Port: {Port}, Bytes: {ByteCount}", serialPort.PortName, count);
                    return count;                    
                }
                logger?.LogTrace("SerialPortStreamOsdpConnection.ReadAsync() Port: {Port} - Timed out, retrying.", serialPort.PortName);
                token.ThrowIfCancellationRequested();
            }
        }

        public async Task WriteAsync(byte[] buffer)
        {
            logger?.LogTrace("SerialPortStreamOsdpConnection.WriteAsync({ByteCount} bytes) Port: {Port}", buffer.Length, serialPort.PortName);
            await serialPort.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}