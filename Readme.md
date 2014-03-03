# evercam.net

A .NET wrapper around Evercam API

## Basic Usage
Right click on EvercamV1.dll (link above) and Save Link As...
```c#
using EvercamV1;
...

// Instantiate Evercam API
Evercam evercam = new Evercam();

```
### Authentication (OAuth2.0)
```c#
// Present client's credentials
evercam.Client = new EvercamClient("client_id", "client_secret", "redirect_uri");

// First time exchange of auth. code to get acccess_token and refresh_token
// It pupulates OAuth2 with new access_token for future use
ResponseToken access_token = evercam.GetAccessToken("auth_code");

// OR if access_token is already obtained BUT expired, then refreshes access_token
// It pupulates OAuth2 with new access_token for future use
ResponseToken refresh_token = evercam.GetRefreshToken("refresh_token");

```
### User
```c#
// Get basic profile details of authenticated user "joeyb"
// Uses OAuth2 credentials for making request to such authenticated resources
User user = evercam.GetUser("joeyb");

// Get list of cameras of a user "joeyb"
List<Camera> cameras = evercam.GetCameras("joeyb");

```
### Camera
```c#
// Create new camera
Camera c = new Camera()
{
    ID = "test",
    Name = "Test Camera",
    Owner = "joeyb",
    IsPublic = true,
    Timezone = "Europe/London",
    Vendor = "tplink",
    Endpoints = new List<string> { "http://123.123.123.123:8080" },
    Snapshots = new Snapshots() { Jpg = "/jpg/image.jpg" },
    Auth = new Auth(new Basic("user", "pass"))
};
evercam.CreateCamera(c);

// Get details of camera 'test'
Camera camera = evercam.GetCamera("test");

// Get live image data of camera 'test'
byte[] imagedata = camera.GetLiveImage();

```
### Vendor
```c#
// Get a list of all camera vendors
List<Vendor> vendors = evercam.GetAllVendors();

// Get list of vendors by name
List<Vendor> vendors = evercam.GetVendorsByName("TP-Link Technologies");

// Get list of vendors by mac address
List<Vendor> vendors = evercam.GetVendorsByMac("54:E6:FC");

```
### Model
```c#
// Get camera model by vendor and model id
Model model = evercam.GetModel("tplink", "*");
string username = model.Defaults.Auth.Basic.UserName;
string password = model.Defaults.Auth.Basic.Password;
string jpg = model.Defaults.Snapshots.Jpg;

```
