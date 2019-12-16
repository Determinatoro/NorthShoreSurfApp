using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // TRANSLATE EXTENSIONS
    /*****************************************************************/
    #region Translate Extensions

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "NorthShoreSurfApp.Resources.AppResources";
        public string Text { get; set; }
        public bool MakeUpper { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var str = resourceManager.GetString(Text, CultureInfo.CurrentCulture);
            return MakeUpper ? str.ToUpper() : str;
        }
    }

    #endregion

    /*****************************************************************/
    // STRING EXTENSIONS
    /*****************************************************************/
    #region String Extensions

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(this string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(this string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }

    #endregion

    /*****************************************************************/
    // DATE TIME EXTENSIONS
    /*****************************************************************/
    #region DateTime Extensions

    public static class DateTimeExtensions
    {
        public static string ToCarpoolingFormat(this DateTime dateTime)
        {
            if ((dateTime - DateTime.Today).TotalDays < 8)
            {
                return dateTime.ToString("dddd").FirstCharToUpper();
            }
            else
            {
                return dateTime.ToString("dd-MM-yy");
            }
        }
    }

    #endregion

    /*****************************************************************/
    // PAGE EXTENSIONS
    /*****************************************************************/
    #region Page Extensions

    public static class PageExtensions
    {
        public static async void ShowMessage(this Xamarin.Forms.Page page, string errorMessage, string cancelTitle = null)
        {
            CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, errorMessage, cancelTitle == null ? Resources.AppResources.ok : cancelTitle);
            await PopupNavigation.Instance.PushAsync(customDialog);
        }
        public static async void ShowYesNo(this Xamarin.Forms.Page page, string message, Action accepted, Action denied = null)
        {
            CustomDialog customDialog = new CustomDialog(CustomDialogType.YesNo, message);
            customDialog.Accepted += (sender, args) =>
            {
                accepted();
            };
            customDialog.Denied += (sender, args) =>
            {
                denied?.Invoke();
            };
            await PopupNavigation.Instance.PushAsync(customDialog);
        }
        public static void DelayedTask(this Xamarin.Forms.Page page, int waitTimeInMilliseconds, Action action)
        {
            Timer timer = new Timer((state) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    action();
                });
            },
            null,
            // Wait time before timer runs
            waitTimeInMilliseconds,
            // No interval
            Timeout.Infinite);
        }
    }

    #endregion

    /*****************************************************************/
    // VIEW EXTENSIONS
    /*****************************************************************/
    #region View Extensions

    public static class ViewExtensions
    {
        /// <summary>
        /// Set iOS safe area insets
        /// </summary>
        /// <param name="view">The view which should have the margins</param>
        /// <param name="page">The page calling this method</param>
        public static void SetIOSSafeAreaInsets(this View view, Xamarin.Forms.Page page)
        {
            // Use safe area on iOS
            page.On<iOS>().SetUseSafeArea(true);
            // Get safe area margins
            var safeAreaInset = page.On<iOS>().SafeAreaInsets();
            // Set safe area margins
            view.Margin = safeAreaInset;
        }

        /// <summary>
        /// Find a parent view with a given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <param name="view">The view to find a parent for</param>
        /// <returns></returns>
        public static T FindParentWithType<T>(this Element view)
        {
            if (view == null)
                return default(T);

            Element parent = view.Parent;
            while (parent != null)
            {
                if (parent.GetType() == typeof(T))
                    return (T)Convert.ChangeType(parent, typeof(T));
                parent = parent.Parent;
            }

            return default(T);
        }
    }

    #endregion
}
