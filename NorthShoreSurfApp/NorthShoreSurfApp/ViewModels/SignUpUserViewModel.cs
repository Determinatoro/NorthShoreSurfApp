using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class SignUpUserViewModel : INotifyPropertyChanged, IFirebaseServiceCallBack
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private SignUpUserPage page;
        private string firstName;
        private string lastName;
        private string phoneNo;
        private string gender;
        private string smsCode;
        private User existingUser;
        private string age;
        private int genderId;
        private ICommand nextCommand;
        private ICommand approveCommand;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SignUpUserViewModel()
        {
            NextCommand = new Command(async () =>
            {
                if (AllDataGiven)
                {
                    switch (PageType)
                    {
                        case SignUpUserPageType.SignUp:
                            // Check if phone no. already exist
                            App.DataService.GetData(
                                Resources.AppResources.checking_phone_no_please_wait,
                                true,
                                () => App.DataService.CheckIfPhoneIsNotUsedAlready(PhoneNo),
                                async (response) =>
                                {
                                    if (response.Success)
                                    {
                                        await App.FirebaseService.VerifyPhoneNo(this, PhoneNo);
                                        page.SetCurrentContentSite(SignUpUserPageContentSite.EnterSMSCode);
                                    }
                                    else
                                    {
                                        CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                        await PopupNavigation.Instance.PushAsync(customDialog);
                                    }
                                });
                            break;
                        case SignUpUserPageType.Login:
                            // Check if phone no. already exist
                            App.DataService.GetData(
                                Resources.AppResources.checking_phone_no_please_wait,
                                true,
                                () => App.DataService.CheckLogin(PhoneNo),
                                async (response) =>
                                {
                                    if (response.Success)
                                    {
                                        // Save the user
                                        ExistingUser = response.Result;
                                        // Make the user verify phone no.
                                        await App.FirebaseService.VerifyPhoneNo(this, PhoneNo);
                                        Page.SetCurrentContentSite(SignUpUserPageContentSite.EnterSMSCode);
                                    }
                                    else
                                    {
                                        CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                        await PopupNavigation.Instance.PushAsync(customDialog);
                                    }
                                });

                            break;
                        case SignUpUserPageType.EditInformation:
                            
                            break;
                    }
                }
                else
                {
                    // Tell the user to fill out all fields on the page
                    await PopupNavigation.Instance.PushAsync(
                        new CustomDialog(
                            CustomDialogType.Message,
                            NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields
                            )
                        );
                }
            });
            // Approve command
            ApproveCommand = new Command(async () =>
            {
                var smsCode = SMSCode;

                if (smsCode != null && smsCode != string.Empty && smsCode.Length == 6)
                {
                    var verId = App.LocalDataService.GetValue(nameof(LocalDataKeys.FirebaseAuthVerificationId));
                    await App.FirebaseService.SignIn(this, verId, smsCode);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, Resources.AppResources.please_enter_sms_code));
                }
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
        public void SetPageType(SignUpUserPageType pageType, User existingUser = null)
        {
            PageType = pageType;

            switch (PageType)
            {
                case SignUpUserPageType.SignUp:
                    break;
                case SignUpUserPageType.Login:
                    break;
                case SignUpUserPageType.EditInformation:
                    if (existingUser == null)
                        throw new NullReferenceException("Existing user cannot be null when using the page for editing information");
                    ExistingUser = existingUser;
                    break;
            }

            OnPropertyChanged(nameof(PageTitle));
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        // OnVerificationFailed
        public async void OnVerificationFailed(string errorMessage)
        {
            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, errorMessage));
        }
        // OnCodeSent
        public void OnCodeSent(string verificationId)
        {

        }
        // OnCodeAutoRetrievalTimeout
        public void OnCodeAutoRetrievalTimeout(string verificationId)
        {

        }
        // SignedIn
        public async void SignedIn()
        {
            switch (PageType)
            {
                case SignUpUserPageType.SignUp:
                    {
                        App.DataService.GetData(
                        Resources.AppResources.creating_account_please_wait,
                        true,
                        () => App.DataService.SignUpUser(FirstName, LastName, PhoneNo, AgeValue, GenderId),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                // Get newly created user
                                User user = response.Result;
                                // Save user id
                                AppValuesService.SaveValue(LocalDataKeys.UserId, user.Id.ToString());
                                // Go to home page
                                App.Current.MainPage = new RootTabbedPage();
                            }
                            else
                            {
                                // Show error
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                        });

                        break;
                    }
                case SignUpUserPageType.Login:
                    {
                        // Go to home page
                        App.Current.MainPage = new RootTabbedPage();
                        break;
                    }
                case SignUpUserPageType.EditInformation:
                    {
                        // Pop this page
                        await Page.Navigation.PopAsync();
                        break;
                    }
            }
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Title shown on top of the page
        /// </summary>
        public string PageTitle
        {
            get
            {
                switch (PageType)
                {
                    case SignUpUserPageType.SignUp:
                        return Resources.AppResources.sign_up;
                    case SignUpUserPageType.Login:
                        return Resources.AppResources.log_in;
                    case SignUpUserPageType.EditInformation:
                        return Resources.AppResources.edit_information;
                }

                return null;
            }
        }
        
        /// <summary>
        /// Page using this view model
        /// </summary>
        public SignUpUserPage Page
        {
            get { return page; }
            set
            {
                page = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Set type for the page
        /// </summary>
        public SignUpUserPageType PageType { get; private set; }
        /// <summary>
        /// Flag for all data is given in the different content sites
        /// </summary>
        public bool AllDataGiven
        {
            get
            {
                switch (PageType)
                {
                    case SignUpUserPageType.EditInformation:
                    case SignUpUserPageType.SignUp:
                        return
                            firstName != null && firstName != string.Empty &&
                            lastName != null && lastName != string.Empty &&
                            age != null && age != string.Empty &&
                            phoneNo != null && phoneNo != string.Empty &&
                            genderId != 0;
                    case SignUpUserPageType.Login:
                        return phoneNo != null && phoneNo != string.Empty;
                }

                return false;
            }
        }
        /// <summary>
        /// Check if phone number has changed
        /// </summary>
        public bool HasPhoneNumberChanged { get => PhoneNo != ExistingUser.PhoneNo; }
        /// <summary>
        /// Set existing user to fill out data on the page
        /// </summary>
        public User ExistingUser
        {
            get => existingUser;
            set
            {
                existingUser = value;
                FirstName = existingUser.FirstName;
                LastName = existingUser.LastName;
                Age = existingUser.Age.ToString();
                PhoneNo = existingUser.PhoneNo;
                Gender = existingUser.Gender?.Name;
                GenderId = existingUser.GenderId;
            }
        }
        /// <summary>
        /// Gender list for the picker on the page
        /// </summary>
        public List<Gender> Genders
        {
            get
            {
                return new List<Gender>(ModelComponents.Gender.Genders);
            }
        }
        /// <summary>
        /// First name input
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Last name input
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Phone no. input
        /// </summary>
        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                if (phoneNo != value)
                {
                    phoneNo = value;
                    OnPropertyChanged();

                    if (PageType == SignUpUserPageType.EditInformation)
                    {
                        Page.SetCurrentContentSite(
                            phoneNo != ExistingUser.PhoneNo ? 
                            SignUpUserPageContentSite.EnterData : 
                            SignUpUserPageContentSite.EditInformation);
                    }
                }
            }
        }
        /// <summary>
        /// Selected gender id
        /// </summary>
        public int GenderId
        {
            get { return genderId; }
            set
            {
                if (genderId != value)
                {
                    genderId = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gender input
        /// </summary>
        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged();
                }
            }
        }
        // Age input
        public string Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Age input as integer
        /// </summary>
        public int AgeValue
        {
            get
            {
                if (int.TryParse(Age, out int value))
                    return value;
                return -1;
            }
        }
        /// <summary>
        /// SMS code input
        /// </summary>
        public string SMSCode
        {
            get { return smsCode; }
            set
            {
                if (smsCode != value)
                {
                    smsCode = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Template for the gender picker items
        /// </summary>
        public DataTemplate GenderPickerItemTemplate
        {
            get { return new DataTemplate(() => new GenderCustomViewCell()); }
        }
        /// <summary>
        /// Next button click
        /// </summary>
        public ICommand NextCommand
        {
            get { return nextCommand; }
            set
            {
                if (nextCommand != value)
                {
                    nextCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Approve button click
        /// </summary>
        public ICommand ApproveCommand
        {
            get { return approveCommand; }
            set
            {
                if (approveCommand != value)
                {
                    approveCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
