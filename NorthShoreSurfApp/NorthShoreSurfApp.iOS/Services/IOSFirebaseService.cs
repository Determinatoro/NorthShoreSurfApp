﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;
using UIKit;

namespace NorthShoreSurfApp.iOS.Services
{
    public class IOSFirebaseService : IFirebaseService
    {
        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        private WhiteListedPhoneNo WhiteListedPhoneNo { get; set; }
        private IFirebaseServiceCallBack FirebaseServiceCallBack { get; set; }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Sign into Firebase with credentials 
        /// </summary>
        /// <param name="phoneAuthCredential">Credentials used to sign into Firebase</param>
        private void SignIn(PhoneAuthCredential phoneAuthCredential)
        {
            Auth.DefaultInstance.SignInWithCredential(phoneAuthCredential, (authResult, error) =>
            {
                // Error occurred
                if (error != null)
                {
                    FirebaseServiceCallBack?.OnVerificationFailed(error.LocalizedDescription);
                    return;
                }

                // User signed in
                FirebaseServiceCallBack?.SignedIn();
            });
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        /// <summary>
        /// Sign into Firebase with phone no.
        /// </summary>
        /// <param name="callBack">Callback for PCL</param>
        /// <param name="verificationId">Verification ID used in the credential object</param>
        /// <param name="code">SMS code used in the credential object</param>
        public Task<FirebaseResponse> SignIn(IFirebaseServiceCallBack callBack, string verificationId, string code)
        {
            try
            {
                // Set callback
                FirebaseServiceCallBack = callBack;
                // Create credential object
                SignIn(PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, code));
                // Return response
                return new Task<FirebaseResponse>(() => new FirebaseResponse(true));
            }
            catch (Exception mes)
            {
                // Return exception
                return new Task<FirebaseResponse>(() => new FirebaseResponse(mes.Message));
            }
        }

        

        /// <summary>
        /// Verify phone no.
        /// </summary>
        /// <param name="callBack">Callback for PCL</param>
        /// <param name="phoneNo">Phone no. to verify</param>
        /// <returns>A firebase response</returns>
        public Task<FirebaseResponse> VerifyPhoneNo(IFirebaseServiceCallBack callBack, string phoneNo)
        {
            this.FirebaseServiceCallBack = callBack;

            try
            {
                // Check if phone no. is white listed
                WhiteListedPhoneNo = FirebaseService.CheckForTestPhoneNo(phoneNo);
                // Verify phone no.
                PhoneAuthProvider.DefaultInstance.VerifyPhoneNumber(
                    phoneNo,
                    null,
                    (verificationId, error) =>
                    {
                        if (error != null)
                        {
                            FirebaseServiceCallBack?.OnVerificationFailed(error.LocalizedDescription);
                            return;
                        }
                        else
                        {
                            // Save verification ID for future use
                            App.LocalDataService.SaveValue(nameof(LocalDataKeys.FirebaseAuthVerificationId), verificationId);

                            if (WhiteListedPhoneNo != null)
                            {
                                var response = SignIn(FirebaseServiceCallBack, verificationId, WhiteListedPhoneNo.Code).Result;
                                if (!response.Success)
                                    FirebaseServiceCallBack?.OnVerificationFailed(response.ErrorMessage);
                            }
                            FirebaseServiceCallBack?.OnCodeSent(verificationId);
                        }
                    }
                    );
                // Return response
                return new Task<FirebaseResponse>(() => new FirebaseResponse(true));
            }
            catch (Exception mes)
            {
                // Return exception
                return new Task<FirebaseResponse>(() => new FirebaseResponse(mes.Message));
            }
        }

        #endregion
    }
}