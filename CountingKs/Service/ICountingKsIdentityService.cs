using System;
namespace CountingKs.Service
{
    public interface ICountingKsIdentityService
    {
        string CurrentUser { get; }
    }
}
