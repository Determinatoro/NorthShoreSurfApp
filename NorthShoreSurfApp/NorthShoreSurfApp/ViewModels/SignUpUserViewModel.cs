using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
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

        private string firstName;
        private string lastName;
        private string phoneNo;
        private string age;
        private string smsCode;
        private int genderId;
        private string gender;
        private User existingUser;

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void InsertExistingData(User user)
        {

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
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public Func<Task<DataResponse<User>>> SignUpUserTask
        {
            get => () => App.DataService.SignUpUser(FirstName, LastName, PhoneNo, AgeValue, GenderId);
        }

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
        public bool HasPhoneNumberChanged { get => PhoneNo != ExistingUser.PhoneNo; }
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
        public SignUpUserPageType PageType { get; private set; }
        public List<Gender> Genders
        {
            get
            {
                var genders = new List<Gender>();
                genders.Add(new Gender()
                {
                    Id = 1,
                    Name = NorthShoreSurfApp.Resources.AppResources.male
                });
                genders.Add(new Gender()
                {
                    Id = 2,
                    Name = NorthShoreSurfApp.Resources.AppResources.female
                });
                genders.Add(new Gender()
                {
                    Id = 3,
                    Name = NorthShoreSurfApp.Resources.AppResources.other
                });
                return genders;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }
        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                if (phoneNo != value)
                {
                    phoneNo = value;
                    OnPropertyChanged(nameof(PhoneNo));
                }
            }
        }
        public int GenderId
        {
            get { return genderId; }
            set
            {
                if (genderId != value)
                {
                    genderId = value;
                    OnPropertyChanged(nameof(GenderId));
                }
            }
        }
        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }
        public string Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }
        public int AgeValue
        {
            get
            {
                if (int.TryParse(Age, out int value))
                    return value;
                return -1;
            }
        }
        public string SMSCode
        {
            get { return smsCode; }
            set
            {
                if (smsCode != value)
                {
                    smsCode = value;
                    OnPropertyChanged(nameof(SMSCode));
                }
            }
        }
        public DataTemplate GenderPickerItemTemplate
        {
            get { return new DataTemplate(() => new GenderCustomViewCell()); }
        }

        #endregion
    }
}
