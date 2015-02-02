# evercam.net

A .NET wrapper for Evercam API

## Basic Usage
Right click on EvercamV2.dll (link above) and Save Link As... Then include the .dll in your .Net (v4.5 or higher) project. Also include the dependencies in your project (Newtonsoft.Json.dll, RestSharp.dll, NodaTime.dll).

```c#
using EvercamV2;
...

// Instantiate Evercam API
Evercam evercam = new Evercam();

// Evercam uses Client or OAuth2 credentials for making request to restricted/private resources
// So following constructors are also handy
Evercam evercam = new Evercam("api_id", "api_secret");

// OR (3rd party apps provide their "oauth2_access_token" or "redirect_uri")
Evercam evercam = new Evercam("oauth2_access_token");

// OR
Evercam evercam = new Evercam("api_id", "api_secret", "redirect_uri");

// Alternatively, Client credentials can be set lateron, as
evercam.Client = new EvercamClient("api_id", "api_secret", "");

// OR
evercam.Client = new EvercamClient("api_id", "api_secret", "redirect_uri");

// OR
evercam.Auth = new Auth(new OAuth2("oauth2_access_token", "bearer"));
```
### Authentication
```c#
// Test client credentials (api_id, api_secret)
AuthResult auth = evercam.TestAuth("api_id", "api_secret")
if (auth.Authenticated)
	DoSomething();
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
    Country = "US",
	FirstName = "Joey",
	LastName = "B.",
	UserName = "Joey B."
};
User user = evercam.UpdateUser(u);

```
### Camera
```c#

// Get list of all cameras of a user "joeyb", including shared cameras, without thumbnails
List<Camera> = evercam.GetCameras("", "joeyb", true, false);

// Get list of cameras with their given (comma separated) IDs, against user "joeyb", including shared cameras, with thumbnails
List<Camera> cameras = evercam.GetCameras("testcam, bestcam, nextcam", "joeyb", true, true);

// Create new camera
CameraInfo info = new CameraInfo()
{
    ID = "testcam",
    Name = "Test Camera",
	Vendor = "tplink",
	Model = "tplink101",
    CameraUsername = "user",
    CameraPassword = "pass",
	Timezone = "Etc/UTC"
    IsPublic = false,
	IsDiscoverable = false,
	IsPublic = false,
    ExternalHost = "123.123.123.123", 
    ExternalHttpPort = 8080,
	JpgUrl = "snapshots/image.jpg",
};
Camera camera = evercam.CreateCamera(info);

// Get details of camera 'testcam'
Camera camera = evercam.GetCamera("testcam");

// Updates details of camera 'testcam'
camera.External.Http.Port = 8081;
camera.External.Rtsp.Port = 1554;
camera = evercam.UpdateCamera(camera.GetInfo());

// Get live image of camera 'testcam' (if Non-Public camera)
byte[] data = evercam.GetLiveImage("testcam");

// Get live image of a camera 'testcam' (if Public camera)
byte[] data = evercam.GetProxyImage("testcam");

```
### Snapshots
```c#
// Fetches a snapshot from the camera and stores it using the current timestamp
Snapshot snap = evercam.CreateSnapshot("testcam", "notes_if_any");

// Get the snapshot stored for camera 'testcam' closest to the given timestamp
Snapshot snap = evercam.GetSnapshot("testcam", "2220913", true, 10);

// Store the supplied snapshot image from file against given timestamp for camera 'testcam'
Snapshot snap = evercam.StoreSnapshot("testcam", "2220913", "path_to_snap_file_on_disk", "notes_if_any");

// Store the supplied snapshot image data against given timestamp for camera 'testcam'
Snapshot snap = evercam.StoreSnapshot("testcam", "2220913", image_bytes, "notes_if_any");

// Delete a snapshot at given timestamp
Snapshot snap = evercam.DeleteSnapshot("testcam", "2220913");

// Get list of specific days in a given month which contains any snapshots
List<int> days = evercam.GetSnapshotDays("testcam", 2014, 1);

// Get list of specific hours in a given day which contains any snapshots
List<int> hours = evercam.GetSnapshotHours("testcam", 2014, 1, 1);

// Get latest snapshot stored for camera 'testcam', response includes image data as base64 if specified
Snapshot snap = evercam.GetLatestSnapshot("testcam", true);

// Get list of all snapshots currently stored for camera 'testcam', between given timestamps
List<Snapshot> snaps = evercam.GetSnapshotDays("testcam", "2220913", "2220923", 10, 0);

```
### Camera Shares
```c#
// Share a camera 'testcam' for given rights with another Evercam user (with registered email 'alicek@email.com')
// Currently a user can share his/her public cameras ONLY with other Evercam users
ShareInfo info = new ShareInfo()
{
    CameraID = "testcam",
    Email = "alicek@email.com",
    Rights = "view,edit,delete,list,snapshot",
	Grantor = "joeyb"
};
Share share = evercam.CreateCameraShare(info);

// Update share rights for a camera 'testcam'
Share share = evercam.UpdateCameraShare("testcam", "View,List");

// Delete share for a camera 'testcam' with share id '944'
evercam.DeleteCameraShare("testcam", 944);

// Get the list of shares for a camera 'testcam', optionally provide user_id to check if its share with given user
List<Share> shares = evercam.GetCameraShares("testcam", "alicek");

// Get the list of sharing requests against camera 'testcam' having given status 'PENDING' (or 'USED' or 'CANCELLED')
List<ShareRequest> requests = evercam.GetCameraShareRequests("testcam", "PENDING");

ShareRequest request = evercam.GetCameraShareRequest(info);
// Evercam user 'alicek@email.com' posts a request to 'joeyb' to share camera 'testcam' with him

// Update share rights for camera 'testcam' against an existing request
ShareRequest request = evercam.UpdateCameraShareRequest("testcam", "view,list", "alicek@email.com");

// Cancel's a sharing request from 'alicek@email.com' for a camera 'testcam'
evercam.DeleteCameraShareRequest("testcam", "alicek@email.com");

```
### Camera Webhooks
```c#
// Registers a webhook for camera 'testcam' against url 'http://www.joey.com/camerahook'
WebHook webhook = evercam.CreateCameraWebhook("testcam", "joeyb@email.com", "http://www.joey.com/camerahook");

// Get list of all webshooks registered against given camera id (or one with specified webhook_id)
List<WebHook> webhooks = evercam.GetCameraWebhooks("testcam", "testcam_alert");

// Delete specified webshook
WebHook webhook = evercam.DeleteCameraWebhook("testcam", "testcam_alert");

// Update webshook URL
WebHook webhook = evercam.UpdateCameraWebhook("testcam", "testcam_alert", "http://www.joey.com/camera_alerts");

```
### Logs
```c#
// Get list of logs for given camera as message strings
LogMessages msgs = evercam.GetLogMessages("testcam", 0, 0, 10, 1, "");

// Get list of logs for given camera as log objects entities
LogObjects objs = evercam.GetLogObjects("testcam", 0, 0, 10, 1, "");

```
### Public
```c#
// Get list of publicly discoverable cameras from within the Evercam system (as per given filter criterias - if any)
List<PublicCamera> public_cameras = evercam.GetPublicCameras(records_offset, records_limit, is_case_sensitive, camera_id_starts_with, camera_id_ends_with, camera_id_contains, is_near_to, within_distance, include_thumbnail);

// Get details of any public camera nearest to given location (lat, lng)
PublicCamera nearest_camera = evercam.GetNearestPublicCameras("0,0");

// Get image from any public camera nearest to given location (lat, lng)
byte[] data = evercam.GetNearestPublicCameraSnapshot("0,0");

```
### Vendors
```c#
// Get a list of all camera vendors, searches for given vendor name and mac if specified
List<Vendor> vendors = evercam.GetVendors("TP-Link", "54:E6:FC");

// Get vendor by given ID
Vendor tplink = evercam.GetVendor("tplink");

```
### Models
```c#
// Get a list of all camera models, filters against given criteria if specified
List<Model> models = evercam.GetModels(model_id, model_name, vendor_id, records_limit, records_page);

// Get camera model by model id
Model model = evercam.GetModel("tplink101");

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