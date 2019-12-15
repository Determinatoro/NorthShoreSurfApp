using DurianCode.PlacesSearchBar;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPlacesBar : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty PlaceSelectedCommandProperty = BindableProperty.Create(nameof(PlaceSelectedCommand), typeof(ICommand), typeof(CustomPlacesBar), null);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomPlacesBar), null);
        public static readonly BindableProperty AutoCompletePredictionsProperty = BindableProperty.Create(nameof(AutoCompletePredictions), typeof(List<AutoCompletePrediction>), typeof(CustomPlacesBar), null);
        public static readonly BindableProperty ApiKeyProperty = BindableProperty.Create(nameof(ApiKey), typeof(string), typeof(CustomPlacesBar), null);
        public static readonly BindableProperty SelectedPlaceProperty = BindableProperty.Create(nameof(SelectedPlace), typeof(Place), typeof(CustomPlacesBar), null, defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (obj, oldValue, newValue) =>
            {
                var bar = obj as CustomPlacesBar;

                if (newValue != null)
                {
                    var place = newValue as Place;

                    // If there is no address components use manually defined values
                    if (place.AddressComponents == null)
                    {
                        if (string.IsNullOrEmpty(place.Address))
                        {
                            bar.Text = string.Format("{0} {1}",
                                place.ZipCode,
                                place.City);
                        }
                        else
                        {
                            bar.Text = string.Format("{0} {1}, {2}",
                                place.ZipCode,
                                place.City,
                                place.Address);
                        }
                    }
                    // Use values from the Google Places API
                    else
                    {
                        bar.Text = string.Format("{0} {1}, {2} {3}",
                        place.PostalCode,
                        place.Locality,
                        place.StreetName,
                        place.StreetNumber);
                    }

                }
            });

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomPlacesBar()
        {
            InitializeComponent();

            // Setup the places bar
            placesBar.Type = PlaceType.Address;
            placesBar.Components = new Components("country:dk");
            placesBar.PlacesRetrieved += PlacesBar_PlacesRetrieved;
            placesBar.MinimumSearchText = 2;
            placesBar.Language = GoogleAPILanguage.Danish;

            // Place selected
            PlaceSelectedCommand = new Command((parameter) =>
            {
                var prediction = ((Func<AutoCompletePrediction>)parameter).Invoke();

                SetSelectedPlace(prediction.Place_ID);
            });
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Set selected place from ID
        /// </summary>
        /// <param name="placeId">ID to get data about a specific place from the Google Places API</param>
        public async void SetSelectedPlace(string placeId)
        {
            var place = await Places.GetPlace(placeId, ApiKey, sessionToken: placesBar.SessionToken);

            if (place != null)
            {
                // Disable search and set the selected place
                placesBar.SessionToken = null;
                placesBar.DisableSearch = true;
                SelectedPlace = place;
                placesBar.DisableSearch = false;
                AutoCompletePredictions = null;
            }
        }

        /// <summary>
        /// Reset predictions to remove the 
        /// </summary>
        public void ResetPredictions()
        {
            placesBar.SessionToken = null;
            AutoCompletePredictions = null;
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        void PlacesBar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            AutoCompletePredictions = result.AutoCompletePlaces;
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public bool DisableSearch
        {
            get => placesBar.DisableSearch;
            set => placesBar.DisableSearch = value;
        }
        /// <summary>
        /// Session token for Google Places API
        /// </summary>
        public string SessionToken
        {
            get
            {
                return placesBar.GetSessionToken();
            }
        }
        /// <summary>
        /// Selected place object
        /// </summary>
        public Place SelectedPlace
        {
            get { return (Place)GetValue(SelectedPlaceProperty); }
            set { SetValue(SelectedPlaceProperty, value); }
        }
        /// <summary>
        /// API key for the Google Places API
        /// </summary>
        public string ApiKey
        {
            get { return (string)GetValue(ApiKeyProperty); }
            set { SetValue(ApiKeyProperty, value); }
        }
        /// <summary>
        /// Place selected command
        /// </summary>
        public ICommand PlaceSelectedCommand
        {
            get { return (ICommand)GetValue(PlaceSelectedCommandProperty); }
            set { SetValue(PlaceSelectedCommandProperty, value); }
        }
        /// <summary>
        /// Text shown in places bar
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>
        /// Auto complete predictions objects
        /// </summary>
        public List<AutoCompletePrediction> AutoCompletePredictions
        {
            get { return (List<AutoCompletePrediction>)GetValue(AutoCompletePredictionsProperty); }
            set { SetValue(AutoCompletePredictionsProperty, value); }
        }
        /// <summary>
        /// Item template for the select car picker
        /// </summary>
        public DataTemplate AutoCompletionItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    var view = new AutoCompletionView();
                    view.SelectedCommand = PlaceSelectedCommand;
                    return view;
                });
            }
        }

        #endregion
    }
}