using System;

namespace Core.Infrastructure.Actions
{
    public class Stopable
    {
        private readonly IDisposable[] disposables;

        public Stopable(params IDisposable[] disposables)
        {
            this.disposables = disposables;
        }

        public void Execute()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}