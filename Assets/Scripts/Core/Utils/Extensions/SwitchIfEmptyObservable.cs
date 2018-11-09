using System;
using UniRx;

namespace Core.Utils.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<T> SwitchIfEmpty<T>(this IObservable<T> source, Func<IObservable<T>> other)
        {
            return new SwitchIfEmptyObservable<T>(source, other);
        }
    }

    public class SwitchIfEmptyObservable<T> : IObservable<T>
    {
        private readonly IObservable<T> source;
        private readonly Func<IObservable<T>> other;
        private bool hasValue;

        public SwitchIfEmptyObservable(IObservable<T> source, Func<IObservable<T>> other)
        {
            this.source = source;
            this.other = other;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return source.Subscribe(
                value =>
                {
                    hasValue = true;
                    observer.OnNext(value);
                    observer.OnCompleted();
                }
                , observer.OnError
                , () =>
                {
                    if (!hasValue)
                        other.Invoke().Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);
                });
        }
    }
}