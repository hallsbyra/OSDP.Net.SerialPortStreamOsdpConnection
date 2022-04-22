# OSDP.Net.SerialPortStreamOsdpConnection

[![NuGet](https://img.shields.io/nuget/v/OSDP.Net.SerialPortStreamOsdpConnection?style=flat)](https://www.nuget.org/packages/OSDP.Net.SerialPortStreamOsdpConnection/)

A plug-in to [OSDP.Net](https://github.com/bytedreamer/OSDP.Net) with a drop-in replacement of the `SerialPortOsdpConnection` that uses [SerialPortStream](https://github.com/jcurl/RJCP.DLL.SerialPortStream) instead of the built-in `System.IO.Ports.SerialPort`.

`System.IO.Ports.SerialPort` is wrought with problems. If you experience stability issues with serial ports in OSDP.Net, it might be a good idea to test this connection instead.

## Getting Started

Install the nuget.

```
dotnet add package OSDP.Net.SerialPortStreamOsdpConnection
```

Use it in code.

```cs
var controlPanel = new ControlPanel();
controlPanel.StartConnection(new SerialPortStreamOsdpConnection(port, baudRate));
```

