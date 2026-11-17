using Microsoft.Extensions.DependencyInjection;
using Project.view;

namespace Project
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
           // return new Window(new AppShell());
           return new Window(new SignInPage());
        }
    }
}