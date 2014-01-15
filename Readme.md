# evercam.net

A .NET wrapper around Evercam API

## Basic Usage
```c#
using Evercam.V1;

// Get vendor object by name
Vendor axis = new Vendor("axis");
String defaultUsername = axis.GetFirmware("*").Auth.Basic.UserName;
String defaultPassword = axis.GetFirmware("*").Auth.Basic.Password;
String defaultJpgUrl = axis.GetFirmware("*").Auth.Snapshots.Jpg;

// Get a list of all camera vendors
List<Vendor> vendors = Vendor.GetAll();

// Get vendor by mac address
List<Vendor> vendors = Vendor.GetAllByMac("00:00:00");
```
