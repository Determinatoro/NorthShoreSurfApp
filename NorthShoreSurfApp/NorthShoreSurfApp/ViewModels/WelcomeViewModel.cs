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
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum WelcomePageContentSite
    {
        Welcome,
        UserNotLoggedIn
    }

    #endregion

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
