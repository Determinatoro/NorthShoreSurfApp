using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using NorthShoreSurfApp.Droid.Services;

namespace NorthShoreSurfApp.Droid.Services
{
    public class AndroidFirebaseService : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IFirebaseService, IOnCompleteListener
    {
        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties
        
        public Activity Activity { get; set; }
        private WhiteListedPhoneNo WhiteListedPhoneNo { get; set; }
        private IFirebaseServiceCallBack FirebaseServiceCallBack { get; set; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public AndroidFirebaseService()
        {
            Activity = MainActivity.Instance;
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Verify phone no.
        /// </summary>
        /// <param name="callBack">Callback for PCL</param>
        /// <param name="phoneNo">Phone no. to verify</param>
        /// <returns></returns>
        public Task<FirebaseResponse> VerifyPhoneNo(IFirebaseServiceCallBack callBack, string phoneNo)
        {
            this.FirebaseServiceCallBack = callBack;

            try
            {
                // Check if phone no. is white listed
                WhiteListedPhoneNo = FirebaseService.CheckForTestPhoneNo(phoneNo);
                // Verify phone no.
                PhoneAuthProvider.Instance.VerifyPhoneNumber(
                    phoneNo,
                    60,
                    TimeUnit.Seconds,
                    Activity,
                    this
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
                SignIn(PhoneAuthProvider.GetCredential(verificationId, code));
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
        /// Sign into Firebase with credentials 
        /// </summary>
        /// <param name="phoneAuthCredential">Credentials used to sign into Firebase</param>
        private void SignIn(PhoneAuthCredential phoneAuthCredential)
        {
            FirebaseAuth auth = new FirebaseAuth(FirebaseApp.Instance);
            // Sign into Firebase with callback
            auth.SignInWithCredential(phoneAuthCredential).AddOnCompleteListener(Activity, this);
        }

        /// <summary>
        /// Sign out of firebase
        /// </summary>
        /// <param name="callBack">Firebase callback</param>
        /// <returns>Firebase response</returns>
        public Task<FirebaseResponse> SignOut(IFirebaseServiceCallBack callBack)
        {
            try
            {
                // Set callback
                FirebaseServiceCallBack = callBack;
                // Sign out
                FirebaseAuth.Instance.SignOut();
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

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                FirebaseServiceCallBack?.SignedIn();
            }
            else
            {
                FirebaseServiceCallBack?.OnVerificationFailed(task.Exception?.Message);
            }
        }

        public override void OnCodeSent(string p0, PhoneAuthProvider.ForceResendingToken p1)
        {
            base.OnCodeSent(p0, p1);

            // Save verification ID for future use
            App.LocalDataService.SaveValue(nameof(LocalDataKeys.FirebaseAuthVerificationId), p0);

            if (WhiteListedPhoneNo != null)
            {
                var response = SignIn(FirebaseServiceCallBack, p0, WhiteListedPhoneNo.Code).Result;
                if (!response.Success)
                    FirebaseServiceCallBack?.OnVerificationFailed(response.ErrorMessage);
            }
            else
                FirebaseServiceCallBack?.OnCodeSent(p0);
        }

        public override void OnCodeAutoRetrievalTimeOut(string p0)
        {
            base.OnCodeAutoRetrievalTimeOut(p0);
        }

        public override void OnVerificationCompleted(PhoneAuthCredential p0)
        {
            try
            {
                SignIn(p0);
            }
            catch (Exception mes)
            {
                FirebaseServiceCallBack?.OnVerificationFailed(mes.Message);
            }
        }

        public override void OnVerificationFailed(FirebaseException p0)
        {
            FirebaseServiceCallBack?.OnVerificationFailed(p0.Message);
        }

        #endregion
    }
}