using Project.ViewModels;

namespace Project.view;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();

        // הגדרת ה-ViewModel כמקור הנתונים (BindingContext) של הדף
        BindingContext = new SignUpViewModel();
    }
}