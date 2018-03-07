using System.Collections.Generic;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class ProvisioningResult
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProvisioningResult()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="success"></param>
        /// <param name="tenantCode"></param>
        /// <param name="schemas"></param>
        public ProvisioningResult(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="success"></param>
        /// <param name="tenantCode"></param>
        /// <param name="schemas"></param>
        public ProvisioningResult(bool success, string tenantCode) : this(success)
        {
            this.Success = success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="success"></param>
        /// <param name="tenantCode"></param>
        /// <param name="schemas"></param>
        public ProvisioningResult(bool success, string tenantCode, IEnumerable<string> schemas) : this(success, tenantCode)
        {
            this.TakenSchemas = schemas;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> TakenSchemas { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string TenantCode { get; set; }

        #endregion Properties
    }
}