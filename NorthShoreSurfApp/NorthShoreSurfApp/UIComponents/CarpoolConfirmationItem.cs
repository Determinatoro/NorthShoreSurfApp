using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.UIComponents
{
    public class CarpoolConfirmationItem : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand acceptConfirmationCommand;
        private ICommand denyConfirmationCommand;

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

        #region Properties

        public string Title { get; set; }
        public bool IsTitle { get => Title != null; }
        public bool IsOwnRide { get; set; }
        public bool IsInvite { get; set; }
        public bool IsJoinRequest { get; set; }
        public CarpoolRide CarpoolRide { get; set; }
        public List<CarpoolRide> CarpoolRideItemsSource
        {
            get
            {
                var list = new List<CarpoolRide>();
                list.Add(CarpoolRide);
                return list;
            }
        }
        public List<CarpoolConfirmation> CarpoolConfirmations { get; set; }
        public DataTemplate CarpoolRideItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    var view = new CarpoolRideViewCell().View;
                    return view;
                });
            }
        }
        public ICommand AcceptConfirmationCommand
        {
            get => acceptConfirmationCommand;
            set
            {
                acceptConfirmationCommand = value;
                OnPropertyChanged();
            }
        }
        public ICommand DenyConfirmationCommand
        {
            get => denyConfirmationCommand;
            set
            {
                denyConfirmationCommand = value;
                OnPropertyChanged();
            }
        }
        public DataTemplate CarpoolConfirmationItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    var viewCell = new CarpoolConfirmationViewCell();
                    viewCell.AcceptCommand = AcceptConfirmationCommand;
                    viewCell.DenyCommand = DenyConfirmationCommand;
                    var view = viewCell.View;

                    var func = new Func<CarpoolConfirmation>(() =>
                    {
                        return view.BindingContext as CarpoolConfirmation;
                    });
                    viewCell.AcceptCommandParameter = func;
                    viewCell.DenyCommandParameter = func;

                    view.Margin = new Thickness(10, 0, 10, 5);
                    return view;
                });
            }
        }

        #endregion
    }
}
