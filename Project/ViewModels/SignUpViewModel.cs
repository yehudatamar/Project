using System.Windows.Input;
using Project.Helpers;
using Project.Services;

namespace Project.ViewModels;

public class SignUpViewModel : ViewModelBase
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _mobile = string.Empty;
    private bool _entryAsPassword = true;
    private string _passwordIconCode = FontHelper.CLOSED_EYE_ICON;

    private readonly DBMokup _db;

    // --- Commands ---
    public ICommand ShowPasswordCommand { get; }
    public ICommand SignUpCommand { get; }
    public ICommand SignInCommand { get; }

    // --- Properties עבור Data Binding ---

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName != value)
            {
                _firstName = value;
                OnPropertyChanged();
                (SignUpCommand as Command)?.ChangeCanExecute(); // מעדכן אם כפתור הרשמה פעיל
            }
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (_lastName != value)
            {
                _lastName = value;
                OnPropertyChanged();
                (SignUpCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public string UserEmail
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged();
                (SignUpCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public string UserPassword
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
                (SignUpCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public string Mobile
    {
        get => _mobile;
        set
        {
            if (_mobile != value)
            {
                _mobile = value;
                OnPropertyChanged();
                (SignUpCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public bool EntryAsPassword
    {
        get => _entryAsPassword;
        set
        {
            if (_entryAsPassword != value)
            {
                _entryAsPassword = value;
                OnPropertyChanged();
            }
        }
    }

    public string PasswordIconCode
    {
        get => _passwordIconCode;
        set
        {
            if (_passwordIconCode != value)
            {
                _passwordIconCode = value;
                OnPropertyChanged();
            }
        }
    }

    // --- הבנאי (Constructor) ---

    public SignUpViewModel()
    {
        _db = new DBMokup();

        ShowPasswordCommand = new Command(TogglePasswordButton);
        SignUpCommand = new Command(SignUp, Validate);
        SignInCommand = new Command(ExecuteSignIn);
    }

    private void TogglePasswordButton()
    {
        if (EntryAsPassword)
            PasswordIconCode = FontHelper.OPEN_EYE_ICON;
        else
            PasswordIconCode = FontHelper.CLOSED_EYE_ICON;

        EntryAsPassword = !EntryAsPassword;
    }

    // בדיקת תקינות כל השדות (תנאי להפעלת כפתור הרשמה)
    private bool Validate()
    {
        var fnameOK = !string.IsNullOrEmpty(FirstName);
        var lnameOK = !string.IsNullOrEmpty(LastName);
        var emailOK = !string.IsNullOrEmpty(UserEmail) && UserEmail.Contains("@") && UserEmail.Contains(".");
        var passOK = !string.IsNullOrEmpty(UserPassword) && UserPassword.Length > 5;
        var mobileOK = !string.IsNullOrEmpty(Mobile) && Mobile.Length == 10;

        return fnameOK && lnameOK && emailOK && passOK && mobileOK;
    }

    private async void SignUp()
    {
        // מעבר למסך הראשי לאחר הרשמה מוצלחת
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            Application.Current.MainPage = new MainPage();
        }
    }

    private async void ExecuteSignIn()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}