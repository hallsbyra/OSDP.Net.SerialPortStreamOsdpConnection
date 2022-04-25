using RJCP.IO.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OSDP.Net.SerialPortStreamOsdpConnection.Tests;

[Collection(Constants.End2End)]
public class SerialPortStreamExploratoryTests
{    
    // The cancellation token passed to ReadAsync is not respected.
    // If this test fails, it means ReadAsync has started respecting the passed CancellationToken.
    [Fact]
    [Trait(Constants.Category, Constants.End2End)]
    public async Task ReadAsync_is_not_cancelled_by_the_cancellation_token()
    {
        // Given
        var stream = new SerialPortStream(Constants.COM1);
        var buffer = new byte[1024];            
        stream.Open();
        stream.ReadTimeout = 1000;
        var readTimeOutMaxTime = DateTime.Now + TimeSpan.FromMilliseconds(1000);
        
        // When
        try
        { 
            await stream.ReadAsync(buffer, 0, buffer.Length, new CancellationTokenSource(TimeSpan.FromMilliseconds(10)).Token);
            if(DateTime.Now < readTimeOutMaxTime)
                throw new Exception("ReadAsync returned long before the ReadTimeout. This is good! It means ReadAsync now respects the passed CancellationToken by returning early.");
        }
        catch(OperationCanceledException)
        {
            throw new Exception("OperationCancelledException was thrown. This is good! It means ReadAsync now respects the passed CancellationToken by throwing OperationCancelledException.");
        }
        finally
        {
            stream.Close();            
        }
    }
}
