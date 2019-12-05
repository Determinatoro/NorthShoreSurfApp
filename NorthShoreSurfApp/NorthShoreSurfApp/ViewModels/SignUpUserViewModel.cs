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
    public class SignUpUserViewModel : INotifyPropertyChanged
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

        public event EventHandler<TextChangedEventArgs> PhoneNoChanged;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SignUpUserViewModel()
        {
            
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
        /// <summary>
        /// Set page type for the page
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="existingUser"></param>
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
        /// Current content site in the page
        /// </summary>
        public SignUpUserPageContentSite CurrentContentSite { get; set; }
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
                // Get old value
                var oldValue = phoneNo;

                if (phoneNo != value)
                {
                    phoneNo = value;
                    OnPropertyChanged();

                    if (PageType == SignUpUserPageType.EditInformation)
                    {
                        PhoneNoChanged?.Invoke(this, new TextChangedEventArgs(oldValue, value));
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
