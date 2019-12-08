using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public enum WelcomePageContentSite
    {
        Welcome,
        UserNotLoggedIn
    }

    public class WelcomeViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private WelcomePageContentSite welcomePageContentSite;
        private ICommand signUpCommand;
        private ICommand logInCommand;
        private ICommand continueAsGuestCommand;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public WelcomeViewModel()
        {
            // Open pages as modal if it is used in the tabbed page else as navigation page
            Action<Page> openPage = async (page) =>
            {
                switch (welcomePageContentSite)
                {
                    case WelcomePageContentSite.Welcome:
                        {
                            await Page.Navigation.PushAsync(page);
                            break;
                        }
                    case WelcomePageContentSite.UserNotLoggedIn:
                        {
                            await Page.Navigation.PushModalAsync(page);
                            break;
                        }
                }
            };
            
            SignUpCommand = new Command(() =>
            {
                openPage(new SignUpUserPage(SignUpUserPageType.SignUp));
            });
            LogInCommand = new Command(() =>
            {
                openPage(new SignUpUserPage(SignUpUserPageType.Login));
            });
            ContinueAsGuestCommand = new Command(() =>
            {
                AppValuesService.SaveValue(LocalDataKeys.UserId, null);
                AppValuesService.SaveValue(LocalDataKeys.IsGuest, bool.TrueString);
                App.Current.MainPage = new RootTabbedPage();
            });
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties 

        /// <summary>
        /// Page using the view model
        /// </summary>
        public Page Page { get; set; }
        /// <summary>
        /// Content site enum to show different content for different purposes
        /// </summary>
        public WelcomePageContentSite WelcomePageContentSite
        {
            get { return welcomePageContentSite; }
            set
            {
                welcomePageContentSite = value;
                OnPropertyChanged(nameof(ShowWelcome));
                OnPropertyChanged(nameof(ShowUserNotLoggedIn));
            }
        }
        /// <summary>
        /// Flag for showing elements for the welcome page
        /// </summary>
        public bool ShowWelcome
        {
            get => WelcomePageContentSite == WelcomePageContentSite.Welcome;            
        }
        /// <summary>
        /// Flag for showing elements for the user not logged in page
        /// </summary>
        public bool ShowUserNotLoggedIn
        {
            get => WelcomePageContentSite == WelcomePageContentSite.UserNotLoggedIn;            
        }
        /// <summary>
        /// Sign up button click
        /// </summary>
        public ICommand SignUpCommand
        {
            get => signUpCommand; set
            {
                signUpCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Login button click
        /// </summary>
        public ICommand LogInCommand
        {
            get => logInCommand; set
            {
                logInCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Continue as guest button click
        /// </summary>
        public ICommand ContinueAsGuestCommand
        {
            get => continueAsGuestCommand; set
            {
                continueAsGuestCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion



    }
}
