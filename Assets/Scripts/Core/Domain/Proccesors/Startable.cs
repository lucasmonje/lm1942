using System;

namespace Core.Domain.Proccesors
{
    public interface Startable
    {
        IDisposable Start();
    }
}