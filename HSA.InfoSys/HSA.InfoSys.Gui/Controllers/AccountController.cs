﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using HSA.InfoSys.Gui.Models;

namespace HSA.InfoSys.Gui.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            //// Testing
            //MembershipUser user = Membership.GetUser("kokoloko");
            //user.Comment = "";
            //Membership.UpdateUser(user);

            ViewData["navid"] = "login";
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            ViewData["navid"] = "login";

            ViewData["invalidLoginMsg"] = "Die Anmeldung war nicht erfolgreich. Korrigieren Sie die Fehler, und wiederholen Sie den Vorgang.";

            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
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
                    ModelState.AddModelError("", "Der angegebene Benutzername oder das angegebene Kennwort ist ungültig.");
                }
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["navid"] = "register";
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [MvcReCaptcha.CaptchaValidator]
        [HttpPost]
        public ActionResult Register(RegisterModel model, bool captchaValid)
        {
            ViewData["navid"] = "register";

            ViewData["invalidRegisterMsg"] = "Das Konto konnte nicht erstellt werden. Korrigieren Sie die Fehler, und wiederholen Sie den Vorgang.";
            
            if (!captchaValid)
            {
                ModelState.AddModelError("", "You did not type the verification word correctly. Please try again.");
            }

            // Sobald dem ModelState ein ModelError geadded wird, ist ModelState nicht mehr Valid
            if (ModelState.IsValid)
            {
                // Versuch, den Benutzer zu registrieren
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["navid"] = "changepw";
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            ViewData["navid"] = "changepw";
            ViewData["invalidMsg"] = "Das Kennwort konnte nicht geändert werden. Korrigieren Sie die Fehler, und wiederholen Sie den Vorgang.";

            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    ViewData["success"] = "Das Kennwort wurde erfolgreich geändert.";
                    return View();
                    //return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Das aktuelle Kennwort ist nicht korrekt, oder das Kennwort ist ungültig.");
                }
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}