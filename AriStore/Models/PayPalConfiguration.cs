using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace AriStore.Models
{
    public class PayPalConfiguration
    {
        /// <summary>
        /// Get properties from the web.config
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();         
        }
        /// <summary>
        /// Get accesstocken from paypal
        /// </summary>
        /// <returns>OAuthTokenCredential</returns>
        private static string GetAccessToken()
        {
            try
            {
                return new OAuthTokenCredential(GetConfig()).GetAccessToken();
            }
            catch (PayPal.PayPalException ex)
            {
                Console.WriteLine(ex);
                return null;
            }        
            
        }      
        /// <summary>
        /// Get APIContext object by invoking it with the GetAccessToken
        /// </summary>
        /// <returns>APIContext</returns>
        public static APIContext GetAPIContext()
        {
            var token = GetAccessToken();
            if (token == null) return null;
            APIContext apiContext = new APIContext(token);
            apiContext.Config = GetConfig();
            return apiContext;
        }
        /// <summary>
        /// Overload GetAPIContext
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ClientSecret"></param>
        /// <returns></returns>
        public static APIContext GetAPIContext(string ClientId, string ClientSecret)
        {
            string accessToken = null;
            try
            {
                accessToken = new OAuthTokenCredential(ClientId, ClientSecret).GetAccessToken();
            }
            catch (PayPal.PayPalException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            
            APIContext apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}