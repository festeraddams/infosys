// ------------------------------------------------------------------------
// <copyright file="CaptchaValidatorAttribute.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace MvcReCaptcha
{
    using System.Configuration;
    using System.Web.Mvc;

    /// <summary>
    /// Spam prevention for the web interface.
    /// </summary>
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The challenge field key
        /// </summary>
        private const string ChallengeFieldKey = "recaptcha_challenge_field";

        /// <summary>
        /// The response field key
        /// </summary>
        private const string ResponseFieldKey = "recaptcha_response_field";

        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="filterContext">Information about the current request and action</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var captchaChallengeValue = filterContext.HttpContext.Request.Form[ChallengeFieldKey];
            var captchaResponseValue = filterContext.HttpContext.Request.Form[ResponseFieldKey];
            var captchaValidtor = new Recaptcha.RecaptchaValidator
                                      {
                                          PrivateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"],
                                          RemoteIP = filterContext.HttpContext.Request.UserHostAddress,
                                          Challenge = captchaChallengeValue,
                                          Response = captchaResponseValue
                                      };

            var recaptchaResponse = captchaValidtor.Validate();

            // this will push the result value into a parameter in our Action
            filterContext.ActionParameters["captchaValid"] = recaptchaResponse.IsValid;

            base.OnActionExecuting(filterContext);
            
            // Add string to Trace for testing
            // filterContext.HttpContext.Trace.Write("Log: OnActionExecuting", String.Format("Calling {0}", filterContext.ActionDescriptor.ActionName));
        }
    }
}