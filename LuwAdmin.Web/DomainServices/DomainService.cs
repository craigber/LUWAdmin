using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.DomainServices
{
    public class DomainService
    {
        public ExpirationService Expiration { get; }

        public DomainService()
        {
            Expiration = new ExpirationService();
        }
    }
}
