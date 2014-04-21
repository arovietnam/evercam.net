# evercam.net

A .NET wrapper around Evercam API

## Basic Usage
Right click on EvercamV1.dll (link above) and Save Link As... Then include the .dll in your .Net (v4.5 or higher) project.

```c#
using EvercamV1;
...

// Instantiate Evercam API
Evercam evercam = new Evercam();
// OR
Evercam evercam = new Evercam("oauth2_access_token");
// OR (provide redirect_uri as "empty string" if there isn't a registered one)
Evercam evercam = new Evercam("api_id", "api_secret", "redirect_uri");


```
### Test
```c#
// A simple endpoint that can be used to test whether an Evercam API id and key pair are valid
string response = evercam.TestCredentials();


```
### Public
```c#

// Get list of publicly discoverable cameras from within the Evercam system
List<Camera> ccc = evercam.GetCameras();

```
### Authentication and Authorization
```c#
// Evercam uses Client or OAuth2 credentials for making request to restricted/private resources

// Present client's credentials
evercam.Client = new EvercamClient("client_id", "client_secret", "");
// OR
evercam.Client = new EvercamClient("client_id", "client_secret", "redirect_uri");

// Provide OAuth2 access token
evercam.Auth = new Auth(new OAuth2("oauth2_access_token", "bearer"));

```
### User
```c#
// Get profile details of user "joeyb"
User user = evercam.GetUser("joeyb");

// Update user information for 'joeyb'
UserInfo u = new UserInfo()
{
    ID = "joeyb",
    Email = "joeyb@newemail.com",
    Country = "US"
};
User user = evercam.UpdateUser(u);

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
### Snapshots
```c#
// Fetches a snapshot from the camera and stores it using the current timestamp
Snapshot snap = evercam.CreateSnapshot("test");

// Get the snapshot stored for this camera closest to the given timestamp
Snapshot snap = evercam.GetSnapshot("test", "2220913");

// Get the list of all snapshots currently stored for camera 'test'
Snapshot snaps = evercam.GetSnapshots("test");

// Get latest snapshot stored for camera 'test', response will also contain image data as base64 encoded JSON
Snapshot snap = evercam.GetLatestSnapshot("test", true);

// Get list of specific days in a given month which contains any snapshots
List<int> days = evercam.GetSnapshotDays("test", 2014, 1);

// Get list of specific hours in a given day which contains any snapshots
List<int> hours = evercam.GetSnapshotDays("test", 2014, 1, 1);

// Get list of snapshots between two timestamps (and additional paramerts like numer of snapshots and page index)
List<Snapshot> snaps = evercam.GetSnapshotDays("test", 2220913, 2220923, true, 10, 0);

```
### Shares
```c#
// Share a camera 'test' with another Evercam user (with registered email 'alice@email.com')
// Currently a user can share his/her public cameras ONLY with other Evercam users
CameraShareInfo cs = new CameraShareInfo() {
    ID = "test",
    Email = "alice@email.com",
    Rights = "list,view,edit,delete,snapshot"
};
CameraShare css = evercam.CreateCameraShare(cs);

// Get the list of shares for a camera 'test'
List<CameraShare> shares = evercam.GetCameraShares("test");

// Get the list of shares currently granted to a user 'joeyb'
List<CameraShare> shares = evercam.GetUserShares("joeyb");

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
