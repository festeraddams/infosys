// ------------------------------------------------------------------------
// <copyright file="SolrResponseBadRequestException.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.InfoSys.Common.Exceptions
{
    using System;

    /// <summary>
    /// Throws an exception if solr gave a bad request as response.
    /// </summary>
    public sealed class SolrResponseBadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolrResponseBadRequestException" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="source">The source.</param>
        public SolrResponseBadRequestException(Exception e, string source)
            : base(Properties.Resources.SOLR_RESPONSE_BAD_REQUEST_EXCEPTION, e)
        {
            this.Source = source;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
        /// </PermissionSet>
        public override string ToString()
        {
            return string.Format(
                Properties.Resources.SOLR_RESPONSE_BAD_REQUEST_EXCEPTION_TO_STRING,
                this.Source,
                this.InnerException);
        }
    }
}
