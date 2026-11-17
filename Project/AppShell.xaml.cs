namespace Project;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // רישום הנתיבים למסכים
        Routing.RegisterRoute(nameof(view.SignInPage), typeof(view.SignInPage));
        Routing.RegisterRoute(nameof(view.SignUpPage), typeof(view.SignUpPage));
    }
}