# evercam.net

A .NET wrapper around Evercam API

## Basic Usage
```c#
using Evercam.V1;
```
### User
```c#
// Get list of public cameras of a user "joeyb"
List<Camera> publiccameras = User.GetAllCameras("joeyb");

// Get list of all cameras of a user "joeyb" using OAuth2.0 Authentication
List<Camera> usercameras = User.GetAllCameras("joeyb", new Auth(new OAuth2("accesstoken")));

// Get list of all cameras of a user "joeyb" using Basic Authentication
List<Camera> usercameras = User.GetAllCameras("joeyb", new Auth(new Basic("username", "password")));

```
### Camera
```c#
// Get details of user's public 'publiccamera'
Camera cam = Camera.Get("publiccamera");

// Get details of user's private 'privatecamera'
Camera cam = Camera.Get("privatecamera", new Auth(new Auth(new OAuth2("accesstoken")));

// Get live image data of 'privatecamera'
byte[] imagedata = cam.GetLiveImage();

```
### Vendor
```c#
// Get vendor object by name
Vendor ycam = new Vendor("ycam");
String defaultUsername = ycam.GetFirmware("*").Auth.Basic.UserName;
String defaultPassword = ycam.GetFirmware("*").Auth.Basic.Password;

// Get a list of all camera vendors
List<Vendor> vendors = Vendor.GetAll();

// Get list of vendors by mac address
List<Vendor> vendors = Vendor.GetAllByMac("00:00:00");

```
### Model
```c#
// Get camera model by vendor and model id
var model = Model.Get("ycam", "YCW005");
string username = model.Defaults.Auth.Basic.UserName;
string password = model.Defaults.Auth.Basic.Password;
string jpg = model.Defaults.Snapshots.Jpg;

```
