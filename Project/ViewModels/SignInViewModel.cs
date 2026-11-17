using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Project.Helpers;
using Project.Services;

namespace Project.ViewModels;

public class SignInViewModel : INotifyPropertyChanged
{
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private bool _entryAsPassword = true;
    private string _passwordIconCode = FontHelper.OPEN_EYE_ICON;

    private string _loginMessage = string.Empty;
    private bool _signInMessageVisible;
    private Color _signInMessageColor = Colors.Red;

    private readonly DBMokup _db;

    // --- Commands עבור כפתורים ---
    public ICommand TogglePasswordCommand { get; }
    public ICommand SignInCommand { get; }

    // --- Properties עבור Data Binding ---

    public string UserName
    {
        get => _userName;
        set
        {
            if (_userName != value)
            {
                _userName = value;
                SignInMessageVisible = false; // מסתיר הודעת שגיאה כשהמשתמש מקליד מחדש
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSignInButtonEnabled));
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
                SignInMessageVisible = false; // מסתיר הודעת שגיאה כשהמשתמש מקליד מחדש
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSignInButtonEnabled));
            }
        }
    }

    // בדיקת תקינות אימייל (חייב להכיל @ ונקודה)
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return email.Contains("@") && email.Contains(".");
    }

    // הכפתור יהיה פעיל רק אם המייל בפורמט תקין והסיסמה אינה ריקה
    public bool IsSignInButtonEnabled =>
        IsValidEmail(UserName) && !string.IsNullOrEmpty(UserPassword);

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

    public string LoginMessage
    {
        get => _loginMessage;
        set
        {
            if (_loginMessage != value)
            {
                _loginMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public bool SignInMessageVisible
    {
        get => _signInMessageVisible;
        set
        {
            if (_signInMessageVisible != value)
            {
                _signInMessageVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public Color SignInMessageColor
    {
        get => _signInMessageColor;
        set
        {
            if (_signInMessageColor != value)
            {
                _signInMessageColor = value;
                OnPropertyChanged();
            }
        }
    }

    // --- הבנאי (Constructor) ---

    public SignInViewModel()
    {
        _db = new DBMokup();

        TogglePasswordCommand = new Command(ExecuteTogglePassword);
        SignInCommand = new Command(ExecuteSignIn);
    }

    private void ExecuteTogglePassword()
    {
        if (EntryAsPassword)
            PasswordIconCode = FontHelper.CLOSED_EYE_ICON;
        else
            PasswordIconCode = FontHelper.OPEN_EYE_ICON;

        EntryAsPassword = !EntryAsPassword;
    }

    private async void ExecuteSignIn()
    {
        if (_db.IsExist(UserName, UserPassword))
        {
            LoginMessage = AppMessages.SignInMessageSuccess;
            SignInMessageColor = Colors.Green;
            SignInMessageVisible = true; // הצגת הודעת ההצלחה

            await Task.Delay(500);

            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                Application.Current.MainPage = new MainPage();
            }
        }
        else
        {
            // 1. קודם מרוקנים את הסיסמה (זה יאפס את ה-SignInMessageVisible ל-false זמנית)
            _password = string.Empty;
            OnPropertyChanged(nameof(UserPassword));
            OnPropertyChanged(nameof(IsSignInButtonEnabled));

            // 2. רק עכשיו מציגים את ההודעה האדומה!
            LoginMessage = AppMessages.SignMessageFail;
            SignInMessageColor = Colors.Red;
            SignInMessageVisible = true;
        }
    }

    // --- מימוש INotifyPropertyChanged ---

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}