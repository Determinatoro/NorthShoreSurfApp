using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // CLASSES
    /*****************************************************************/
    #region Classes

    public class FirebaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public FirebaseResponse(bool success)
        {
            Success = success;
        }

        public FirebaseResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    public class WhiteListedPhoneNo
    {
        public string PhoneNo { get; set; }
        public string Code { get; set; }

        public WhiteListedPhoneNo(string phoneNo, string code)
        {
            PhoneNo = phoneNo;
            Code = code;
        }
    }

    public class FirebaseService
    {
        public static WhiteListedPhoneNo CheckForTestPhoneNo(string phoneNo)
        {
            var phoneNos = new WhiteListedPhoneNo[]
            {
                new WhiteListedPhoneNo("+4512345678", "220196"),
                new WhiteListedPhoneNo("+4511111111", "123456")
            };

            return phoneNos.FirstOrDefault(x => x.PhoneNo == phoneNo);
        }
    }

    #endregion

    /*****************************************************************/
    // INTERFACES
    /*****************************************************************/
    #region Interfaces

    public interface IFirebaseService
    {
        Task<FirebaseResponse> VerifyPhoneNo(IFirebaseServiceCallBack callBack, string phoneNo);
        Task<FirebaseResponse> SignIn(IFirebaseServiceCallBack callBack, string verificationId, string code);
        Task<FirebaseResponse> SignOut(IFirebaseServiceCallBack callBack);
    }

    public interface IFirebaseServiceCallBack
    {
        void OnVerificationFailed(string errorMessage);
        void OnCodeSent(string verificationId);
        void OnCodeAutoRetrievalTimeout(string verificationId);
        void SignedIn();
    }

    #endregion
}
