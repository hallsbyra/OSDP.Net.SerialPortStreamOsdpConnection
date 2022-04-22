using RJCP.IO.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSDP.Net.Connections
{
    public class SerialPortStreamOsdpConnection : IOsdpConnection
    {
        readonly SerialPortStream serialPort;

        public SerialPortStreamOsdpConnection(string portName, int baudRate)
        {
            serialPort = new SerialPortStream(portName, baudRate);
        }

        public int BaudRate => serialPort.BaudRate;

        public bool IsOpen => serialPort.IsOpen;

        public TimeSpan ReplyTimeout { get; set; } = TimeSpan.FromMilliseconds(200);

        public void Close()
        {
            serialPort.Close();
        }

        public void Open()
        {
            serialPort.Open();
        }

        public async Task<int> ReadAsync(byte[] buffer, CancellationToken token)
        {
            return await serialPort.ReadAsync(buffer, 0, buffer.Length, token);
        }

        public async Task WriteAsync(byte[] buffer)
        {
            await serialPort.WriteAsync(buffer, 0, buffer.Length);                
        }
    }
}