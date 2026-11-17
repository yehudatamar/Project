using Project.ViewModels;

namespace Project.view;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
        BindingContext = new SignInViewModel(); // חיבור ה-ViewModel הבלעדי ל-Page
    }
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }
    }
}