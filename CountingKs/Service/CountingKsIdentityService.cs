using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace CountingKs.Service
{
    public class CountingKsIdentityService : CountingKs.Service.ICountingKsIdentityService
    {
        public String CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;//Identity of the Logged in user
            }
        }
    }
}