using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using Newtonsoft.Json;
using NorthShoreSurfApp;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IFirebaseService))]
namespace NorthShoreSurfApp.iOS.Services
{
    public class IOSFacebookService : IFacebookService
    {
        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public bool LoggedIn => AccessToken.CurrentAccessToken != null;
        public FacebookResult FacebookResult { get; set; }
        private IFacebookLoginCallback FacebookLoginCallback { get; set; }
        private IFacebookDataCallback FacebookDataCallback { get; set; }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Login to facebook
        /// </summary>
        /// <param name="callback"></param>
        public void LogIn(IFacebookLoginCallback callback)
        {
            this.FacebookLoginCallback = callback;

            new LoginManager().LogIn(FacebookService.Permissions, null, (result, error) =>
            {
                if (result.IsCancelled)
                {
                    FacebookLoginCallback.OnCancel();
                }
                else if (error != null)
                {
                    FacebookLoginCallback.OnError(error.ToString());
                }
                else
                {
                    FacebookLoginCallback.OnSuccess();
                }
            });
        }

        /// <summary>
        /// Log out from facebook
        /// </summary>
        public void LogOut()
        {
            new LoginManager().LogOut();
        }

        /// <summary>
        /// Get user data from facebook
        /// </summary>
        /// <param name="callback"></param>
        public void GetUserData(IFacebookDataCallback callback)
        {
            this.FacebookDataCallback = callback;

            var accessToken = AccessToken.CurrentAccessToken;

            FacebookResult = new FacebookResult();
            FacebookResult.Token = accessToken.TokenString;

            NSMutableDictionary keyValues = new NSMutableDictionary();
            keyValues.Add(new NSString("fields"), new NSString(FacebookService.Fields));

            GraphRequest graphRequest = new GraphRequest("me", keyValues, AccessToken.CurrentAccessToken.TokenString, null, HttpMethod.Get);
            GraphRequestConnection graphRequestConnection = new GraphRequestConnection();
            graphRequestConnection.AddRequest(graphRequest, (connection, result, error) =>
            {
                if (error != null)
                {
                    FacebookDataCallback.OnDataReceivedError(error.ToString());
                    return;
                }

                JsonConvert.PopulateObject(result.ToString(), FacebookResult);
                FacebookDataCallback.OnUserDataReceived(FacebookResult);
            });
        }

        #endregion
    }
}