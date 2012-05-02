using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Claims;

namespace MVC3MixedAuthenticationSample.Models
{
    public class IdentityClaim
    {
        private string _identityProvider;
        private string _identityValue;
        public const string ACSProviderClaim = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        public IdentityClaim(IClaimsIdentity identity)
        {

            if (identity != null)
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.ClaimType == ClaimTypes.NameIdentifier)
                    {
                        _identityValue = claim.Value;
                    }
                    if (claim.ClaimType == ACSProviderClaim)
                    {
                        _identityProvider = claim.Value;
                    }

                }
            }

        }

        public IdentityClaim()
        {
            _identityProvider = HttpContext.Current.Session["IdentityProvider"] as string;
            _identityValue = HttpContext.Current.Session["IdentityValue"] as string;
        }

        public bool HasIdentity
        {
            get
            {
                return (!string.IsNullOrEmpty(_identityProvider) && (!string.IsNullOrEmpty(_identityValue)));
            }
        }

        public string IdentityProvider
        {
            get
            {
                return _identityProvider;
            }
        }

        public string IdentityValue
        {
            get
            {
                return _identityValue;
            }
        }

        internal void SaveToSession()
        {
            HttpContext.Current.Session["IdentityProvider"] = _identityProvider;
            HttpContext.Current.Session["IdentityValue"] = _identityValue;
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Remove("IdentityProvider");
            HttpContext.Current.Session.Remove("IdentityValue");
        }

        public static string ProviderNiceName(string identityProivder)
        {
            if (identityProivder.ToLower().Contains("windowslive"))
                return "Windows Live";

            if (identityProivder.ToLower().Contains("facebook"))
                return "Facebook";

            if (identityProivder.ToLower().Contains("yahoo"))
                return "Yahoo";

            if (identityProivder.ToLower().Contains("google"))
                return "Google";

            return identityProivder;
        }
    }

}