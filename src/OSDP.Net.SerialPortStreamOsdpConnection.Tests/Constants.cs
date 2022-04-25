using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSDP.Net.SerialPortStreamOsdpConnection.Tests;

static class Constants
{
    // The end2end tests in this suite require a pair of connected serial ports (0-modem).
    // Writing to one port will cause the other to receive the data and vice versa.
    // Such ports can be emulated by e.g. "com0com".
    // These constants hold the names of these virtual ports.
    public const string COM1 = "COM10";
    public const string COM2 = "COM11";

    // Constants for XUnit traits.
    public const string Category = "Category";
    public const string End2End = "end2end";
}
