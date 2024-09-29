using System;
using System.Threading.Tasks;

namespace Utility
{
    public class Promise<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource = new();
        
        public bool TryResolve(T result)
        {
            return _taskCompletionSource.TrySetResult(result);
        }
        
        public void Resolve(T result)
        {
            _taskCompletionSource.SetResult(result);
        }
        
        public async void TryRejectAfterTimeout(int msTimeout)
        {
            await System.Threading.Tasks.Task.Delay(msTimeout);
            var type = GetType();
            TryReject(new TimeoutException($"{type} has timed out"));
        }
        
        public bool TryReject(Exception exception)
        {
            return _taskCompletionSource.TrySetException(exception);
        }

        public void Reject(Exception exception)
        {
            _taskCompletionSource.SetException(exception);
        }
        public bool TryCancel()
        {
            return _taskCompletionSource.TrySetCanceled();
        }
        
        public void Cancel()
        {
            _taskCompletionSource.SetCanceled();
        }
        
        public Task<T> Task => _taskCompletionSource.Task;
    }

    public class Promise
    {
        private readonly TaskCompletionSource<object> _taskCompletionSource = new();

        public bool TryResolve()
        {
            return _taskCompletionSource.TrySetResult(null);
        }
        
        public void Resolve()
        {
            _taskCompletionSource.SetResult(null);
        }

        public void Reject(Exception exception)
        {
            _taskCompletionSource.SetException(exception);
        }
        
        public void Cancel()
        {
            _taskCompletionSource.SetCanceled();
        }

        public Task Task => _taskCompletionSource.Task;
    }
}