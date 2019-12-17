using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace NorthShoreSurfApp.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private User existingUser;
        private string fullName;
        private string phoneNo;
        private string age;
        private string gender;
        private ICommand editCommand;
        private ICommand logOutCommand;
        private ICommand deleteAccountCommand;

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
        /// Object for an existing user
        /// </summary>
        public User ExistingUser
        {
            get => existingUser;
            set
            {
                existingUser = value;
                if (existingUser == null)
                    return;

                // Assign values
                FullName = existingUser.FullName;
                PhoneNo = existingUser.PhoneNo;
                Age = existingUser.Age.ToString();
                Gender = existingUser.Gender.LocalizedName;
            }
        }
        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Users's phone no.
        /// </summary>
        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                phoneNo = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// User's age
        /// </summary>
        public string Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// User's gender
        /// </summary>
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Command for edit button
        /// </summary>
        public ICommand EditCommand
        {
            get => editCommand;
            set
            {
                editCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Command for log out button
        /// </summary>
        public ICommand LogOutCommand
        {
            get => logOutCommand;
            set
            {
                logOutCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Command for delete account button
        /// </summary>
        public ICommand DeleteAccountCommand
        {
            get => deleteAccountCommand;
            set
            {
                deleteAccountCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}