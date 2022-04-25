using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using OsdpConnection = OSDP.Net.Connections.SerialPortStreamOsdpConnection;

namespace OSDP.Net.SerialPortStreamOsdpConnection.Tests;

[Collection(Constants.End2End)]
public class SerialPortStreamOsdpConnectionFacts
{
    [Fact(Timeout = 1000)]
    [Trait(Constants.Category, Constants.End2End)]
    public async Task ReadAsync_is_cancelled_by_the_cancellation_token()
    {
        // Given
        var stream = new OsdpConnection(Constants.COM1, 9600);
        stream.Open();
        var buffer = new byte[1024];
        
        // When
        try
        { 
            await stream.ReadAsync(buffer, new CancellationTokenSource(TimeSpan.FromMilliseconds(10)).Token);
            throw new Exception("Expected OperationCanceledException.");
        }
        catch(OperationCanceledException)
        {
            // This is good. We were cancelled.
        }
        finally
        {
            stream.Close();
        }
    }
}
