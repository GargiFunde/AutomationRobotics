using System;
using System.Linq;
using Logger;
using Portable.Licensing;
using Portable.Licensing.Validation;

namespace BOTDesigner
{
    public class LicenseHelper
    {
        /// <summary>
        /// The validate license.
        /// </summary>
        /// <param name="license">
        /// The license.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ValidateLicense(License license, string publickey)
        {
            try
            {
                //    //// validate license and define return value.
                const string ReturnValue = "Activated: License is Valid";

                var validationFailures =
                    license.Validate()
                        .ExpirationDate()
                        .When(LicenseException)
                        .And()
                        .Signature(publickey)
                        .AssertValidLicense();

                var failures = validationFailures as IValidationFailure[] ?? validationFailures.ToArray();

                return !failures.Any() ? ReturnValue : failures.Aggregate(string.Empty, (current, validationFailure) => current + validationFailure.HowToResolve + ": " + "\r\n" + validationFailure.Message + "\r\n");
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                return string.Empty;
            }
        }
        /// <summary>
        /// The license exception.
        /// </summary>
        /// <param name="license">
        /// The license.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool LicenseException(License license)
        {
            //// check licensetype.
            return license.Type == LicenseType.Trial;
        }

    }
}
