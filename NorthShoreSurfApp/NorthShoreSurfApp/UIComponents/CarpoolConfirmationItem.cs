using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Title for the header
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Flag for showing header instead of a confirmation
        /// </summary>
        public bool IsTitle { get => Title != null; }
        /// <summary>
        /// User seeing this confirmation
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Flag for own ride
        /// </summary>
        public bool OwnRide { get; set; }
        /// <summary>
        /// Flag for other ride
        /// </summary>
        public bool OtherRide { get; set; }
        /// <summary>
        /// Carpool ride object
        /// </summary>
        public CarpoolRide CarpoolRide { get; set; }
        /// <summary>
        /// All the unconfirmed confirmations for the carpool ride
        /// </summary>
        public List<CarpoolConfirmation> UnconfirmedCarpoolConfirmations
        {
            get
            {
                if (CarpoolRide == null)
                    return null;

                if (OwnRide)
                {
                    return CarpoolRide.CarpoolConfirmations
                                      .Where(x => !x.IsConfirmed)
                                      .GroupBy(x => x.PassengerId)
                                      .SelectMany(x =>
                                      {
                                          if (x.Count() > 1)
                                              return x.Where(x => x.IsRequest);
                                          else
                                              return x;
                                      })
                                      .OrderByDescending(x => x.IsInvite)
                                      .ToList();
                }
                else if (OtherRide)
                {
                    return CarpoolRide.CarpoolConfirmations
                                      .Where(x => !x.IsConfirmed && x.PassengerId == UserId)
                                      .GroupBy(x => x.PassengerId)
                                      .SelectMany(x =>
                                      {
                                          if (x.Count() > 1)
                                              return x.Where(x => x.IsInvite);
                                          else
                                              return x;
                                      })
                                      .OrderBy(x => x.IsInvite)
                                      .ToList();
                }

                return null;
            }
        }
        /// <summary>
        /// Items source for showing a single carpool ride
        /// </summary>
        public List<CarpoolRide> CarpoolRideItemsSource
        {
            get
            {
                var list = new List<CarpoolRide>();
                list.Add(CarpoolRide);
                return list;
            }
        }
        /// <summary>
        /// Item template for the carpool ride
        /// </summary>
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
        /// <summary>
        /// Accept confirmatio command
        /// </summary>
        public ICommand AcceptConfirmationCommand
        {
            get => acceptConfirmationCommand;
            set
            {
                acceptConfirmationCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Deny confirmation command
        /// </summary>
        public ICommand DenyConfirmationCommand
        {
            get => denyConfirmationCommand;
            set
            {
                denyConfirmationCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Item template for a confirmation
        /// </summary>
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
