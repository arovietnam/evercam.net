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
### Authentication
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
    Email = "joeyb@email.com",
    Country = "US"
};
User user = evercam.UpdateUser(u);

// Get list of cameras of a user "joeyb", including shared cameras
List<Camera> = evercam.GetUserCameras("joeyb", true);
```
### Camera
```c#
// Create new camera
CameraInfo info = new CameraInfo() { 
    ID = "testcam",
    Name = "Test Camera",
    Username = "user",
    Password = "pass",
    IsPublic = false,
    ExternalHost = "123.123.123.123",
    InternalHost = "192.168.1.123",
    ExternalHttpPort = "8080",
    InternalHttpPort = "80",
    JpegUrl = "snapshots/image.jpg"
};
Camera camera = evercam.CreateCamera(info);

// Get details of camera 'testcam'
Camera camera = evercam.GetCamera("testcam");

// Updates details of camera 'testcam'
camera.ExternalHttpPort = 8080;
camera.InternalHttpPort = 80;
camera = evercam.UpdateCamera(camera.GetInfo());

// Get live image data of camera 'testcam'
LiveImage image = evercam.GetLiveImage("testcam");
byte[] data = Utility.ToBytes(image.Data);

// Get list of cameras with their given (comma separated) IDs
List<Camera> cameras = evercam.GetCameras("testcam, bestcam, nextcam");
```
### Snapshots
```c#
// Fetches a snapshot from the camera and stores it using the current timestamp
Snapshot snap = evercam.CreateSnapshot("testcam");

// Get the snapshot stored for this camera closest to the given timestamp
Snapshot snap = evercam.GetSnapshot("testcam", "2220913");

// Get the list of all snapshots currently stored for camera 'testcam'
Snapshot snaps = evercam.GetSnapshots("testcam");

// Get latest snapshot stored for camera 'testcam', response will also contain image data as base64 encoded JSON
Snapshot snap = evercam.GetLatestSnapshot("testcam", true);

// Get list of specific days in a given month which contains any snapshots
List<int> days = evercam.GetSnapshotDays("testcam", 2014, 1);

// Get list of specific hours in a given day which contains any snapshots
List<int> hours = evercam.GetSnapshotDays("testcam", 2014, 1, 1);

// Get list of snapshots between two timestamps (and additional paramerts like numer of snapshots and page index)
List<Snapshot> snaps = evercam.GetSnapshotDays("testcam", 2220913, 2220923, true, 10, 0);
```
### Shares
```c#
// Share a camera 'testcam' for given rights with another Evercam user (with registered email 'alicek@email.com')
// Currently a user can share his/her public cameras ONLY with other Evercam users
ShareInfo info = new ShareInfo()
{
    CameraID = "testcam",
    Email = "alicek@email.com",
    Rights = "view,edit,delete,list,snapshot"
};
Share share = evercam.CreateCameraShare(info);

// Update share rights for a camera 'testcam'
Share share = evercam.UpdateCameraShare("testcam", "View,List");

// Delete share for a camera 'testcam' with share id '944'
evercam.DeleteCameraShare("testcam", 944);

// Get the list of shares for a camera 'testcam'
List<Share> shares = evercam.GetCameraShares("testcam");

// Get the list of shares currently granted to a user 'joeyb'
List<Share> shares = evercam.GetUserShares("joeyb");

// Evercam user 'alicek@email.com' posts a request to 'joeyb' to share camera 'testcam' with him
ShareRequest info = new ShareRequest()
{
    CameraID = "testcam",
    Email = "alicek@email.com",
    Rights = "view,edit,delete,list",
    UserID = "joeyb"
};
ShareRequest request = evercam.CreateCameraShareRequest(info);

// Update share rights for camera 'testcam' against an existing request
ShareRequest request = evercam.UpdateCameraShareRequest("testcam", "view,list");

// Cancel's a sharing request from 'alicek@email.com' for a camera 'testcam'
evercam.DeleteCameraShareRequest("testcam", "alicek@email.com")

// Get the list of sharing requests against camera 'testcam' having given status 'PENDING' (or 'USED' or 'CANCELLED')
List<ShareRequest> requests = evercam.GetCameraShareRequests("testcam", "PENDING");
```
### Logs
```c#
// Get list of logs for given camera as message strings
LogMessages msgs = evercam.GetLogMessages("testcam", 0, 0, 10, 1, "");
// Get list of logs for given camera as log objects entities
LogObjects objs = evercam.GetLogObjects("testcam", 0, 0, 10, 1, "");
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
### Test
```c#
// A simple endpoint that can be used to test whether an Evercam API id and key pair are valid
Evercam evercam = new Evercam("api_id", "api_secret");
string response = evercam.TestCredentials();
```
### Public
```c#
// Get list of publicly discoverable cameras from within the Evercam system
List<Camera> ccc = evercam.GetCameras();
```
### Utility
This helper class provides common functionality to do basic conversion between Unix and Windows date time and timezone.
```c#
// Convets Windows DateTime to equivalent Unix Timestamp
long timestamp = Utility.ToUnixTimestamp(DateTime.Now);

// Converts Unix Timestamp to equivalent Windows DateTime
DateTime now = Utility.ToWindowsDateTime(timestamp);

// Converts the Unix timezone to Windows, if matches.
string windowszone = Utility.ToWindowsTimezone("Europe/Dublin");

// Converts the Windows timezone to Unix. If the primary zone is a link, it then resolves it to the canonical ID.
string unixzone = Utility.ToUnixTimezone(windowszone);
```