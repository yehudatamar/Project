using Android.Service.Chooser;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Project.view;

public partial class SignUpPic : ContentPage
{
    private string? _userImageBase64;
    public string? UserImageBase64
    {
        get => _userImageBase64;
        set
        {
            if(_userImageBase64 != value)
            {
                _userImageBase64 = value;
                OnPropertyChanged(nameof(UserImageBase64));
            }
        }
    }
    public ImageSource UserImageSource => ImageSource.FromStream(() =>
    {
        byte[] bytes = Convert.FromBase64String(_userImageBase64!);
        return new MemoryStream(bytes);
    });
    public SignUpPic()
	{
		InitializeComponent();
        BindingContext=this;
	}

    Image Mathods; 

    public async Task InitializerPageAsync()
    {

    }
	public async void GetImage (object sender , EventArgs e)
	{
		//string action = await Application.Current.MainPage.DisplayActionSheet()
		string? action = await DisplayActionSheet("Profile Photo", "Cencel", null, "Take Photo", "Choose from Gallery");

        var hasPermission = await RequestCameraPermissionAsync();
        if (!hasPermission)
        {
        	await Toast.Make("Camera and Gallery permissions are required to change a photo! The App will not work properly!", ToastDuration.Short, 14).Show();
        	return;
        }

        switch (action)
        {
            case "Take Photo":
                await TakePhotoAsync();
                break;

            case "בחר תמונה קיימת":
                await SelectPhotoAsync();
                break;

            case "Cancle":
                // No action needed, just dismiss the action sheet
                break;

            default:
                await Toast.Make($"Unknown action", ToastDuration.Short, 14).Show();
                break;

        }
    }

    private async Task SelectPhotoAsync()
    {
        throw new NotImplementedException();
    }

    private async Task TakePhotoAsync()
    {
        try
        {
            string base64 = await CaptureImageAsBase64Async();
            if (!string.IsNullOrEmpty(base64))
            {
                UserImageBase64 = base64;
                OnPropertyChanged(nameof(UserImageSource));
            }
        }
        catch (Exception ex)
        {
            await Toast.Make($"Error taking photo: {ex.Message}", ToastDuration.Short, 14).Show();
        }
    }
    public async Task<string> CaptureImageAsBase64Async()
    {
        if (MediaPicker.IsCaptureSupported)
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }

            return null;
        }
        else
        {
            await Toast.Make("Camera is not supported on this device.", ToastDuration.Short, 14).Show();
            return null;
        }
    }

    public async Task<bool> RequestCameraPermissionAsync()
    {
        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
        var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        var storageWriteStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

        if (cameraStatus != PermissionStatus.Granted)
            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();

        if (storageReadStatus != PermissionStatus.Granted)
            storageReadStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

        if (storageWriteStatus != PermissionStatus.Granted)
            storageWriteStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

        return cameraStatus == PermissionStatus.Granted &&
                    storageReadStatus == PermissionStatus.Granted &&
                        storageWriteStatus == PermissionStatus.Granted;
    }
}
	 
