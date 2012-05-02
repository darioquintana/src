using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MVC3MixedAuthenticationSample.Models;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Protocols.WSFederation;
using ASPNETSimpleMVC3.Hrd;
using ASPNETSimpleMVC3.Models;

namespace MVC3MixedAuthenticationSample.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            HrdClient hrdClient = new HrdClient();

            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            HrdRequest request = new HrdRequest(fam.Issuer, fam.Realm, context: Request.QueryString["ReturnUrl"]);

            IEnumerable<HrdIdentityProvider> hrdIdentityProviders = hrdClient.GetHrdResponse(request);

            var claim = new IdentityClaim();
            if (claim.HasIdentity)
            {
                ViewData["HasClaim"] = true;
            }

            ViewData["Providers"] = hrdIdentityProviders;
            return View();
        }


        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            var claim = new IdentityClaim();

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                   

                    if (claim.HasIdentity)
                    {
                        var db = new IdentityRepository();
                        var user = Membership.GetUser(model.UserName);
                        db.MapIdentity(user.ProviderUserKey.ToString(), claim.IdentityProvider, claim.IdentityValue);
                        IdentityClaim.ClearSession();
                    }

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            HrdClient hrdClient = new HrdClient();

            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            HrdRequest request = new HrdRequest(fam.Issuer, fam.Realm, context: Request.Url.AbsoluteUri);

            IEnumerable<HrdIdentityProvider> hrdIdentityProviders = hrdClient.GetHrdResponse(request);

            if (claim.HasIdentity)
            {
                ViewData["HasClaim"] = true;
            }

            ViewData["Providers"] = hrdIdentityProviders;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var claim = new IdentityClaim();
                    if (claim.HasIdentity)
                    {
                        var db = new IdentityRepository();
                        var user = Membership.GetUser(model.UserName);
                        db.MapIdentity(user.ProviderUserKey.ToString(), claim.IdentityProvider, claim.IdentityValue);
                        IdentityClaim.ClearSession();
                    }

                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// Action used to sign user in from ACS
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SignIn(FormCollection forms)
        {
            //Extract claims
            var principal = HttpContext.User;
            var claim = new IdentityClaim(principal.Identity as IClaimsIdentity);

            //Delete session cookie so the module cannot reset principal
            SessionAuthenticationModule sam = FederatedAuthentication.SessionAuthenticationModule;
            sam.DeleteSessionTokenCookie();

            if (claim.HasIdentity)
            {
                var db = new IdentityRepository();

                var identity = db.FindByProvderAndValue(claim.IdentityProvider, claim.IdentityValue);
                string returnUrl = GetUrlFromContext(forms) ?? Url.Action("Index", "Home", null);
                if (identity != null)
                {
                    var user = Membership.GetUser(identity.UserId);

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        return Redirect(returnUrl);
                    }

                }
                else
                {
                    //Save identity values for processing in the association page
                    claim.SaveToSession();

                    return RedirectToAction("LogOn", new { ReturnUrl = returnUrl });

                }

            }

            return RedirectToAction("LogOn", "Account");
        }

        /// <summary>
        /// Remove pending association from session
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult CancelAssociation()
        {
            IdentityClaim.ClearSession();

            return Json(true);
        }

        /// <summary>
        /// Allows a user to remove an identity association
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult RemoveAssociation(int id)
        {
            var db = new IdentityRepository();
            var user = Membership.GetUser();
            db.DeleteById(id,user.ProviderUserKey.ToString());

            return Json(true);
        }

        /// <summary>
        /// Allows users to manage their associated identites
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Identities()
        {
            var db = new IdentityRepository();
            var user = Membership.GetUser();

            var identities = db.FindByUserId(user.ProviderUserKey.ToString());

            return View(identities);
        }

        /// <summary>
        /// Gets from the form the context
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private static string GetUrlFromContext(FormCollection form)
        {
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(new Uri("http://www.notused.com"), form);
            return (message != null ? message.Context : null);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
