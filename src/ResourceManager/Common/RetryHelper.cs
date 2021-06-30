using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager
{
    public static class RetryHelper
    {
        public static readonly Predicate<Exception> IOExceptionsFilter = ex => ex is IOException;

        public static readonly Predicate<Exception> UnauthorizedIOExceptions = ex => ex is UnauthorizedAccessException || ex is IOException;

        public static readonly Predicate<Exception> DirectoryNotFoundExceptions = ex => ex is DirectoryNotFoundException;

        public static readonly Predicate<Exception> FileNotFoundExceptions = ex => ex is FileNotFoundException;

        public static readonly Predicate<Exception> AnyExceptionFilter = ex => true;

        private static readonly int defaultMaxAttempts = 5;

        private static readonly TimeSpan defaultRetryDelay = TimeSpan.FromSeconds(0.2);

        public static void TryOperation(Action operation, Predicate<Exception> exceptionFilter = null)
            => TryOperation(defaultMaxAttempts, operation, exceptionFilter);

        public static void TryOperation(int maxAttempts, Action operation, Predicate<Exception> exceptionFilter = null)
            => TryOperation<object>(maxAttempts, defaultRetryDelay, () => {
                operation.Invoke();
                return null;
            }, exceptionFilter);

        public static T TryOperation<T>(Func<T> operation, Predicate<Exception> exceptionFilter = null)
            => TryOperation(defaultMaxAttempts, defaultRetryDelay, operation, exceptionFilter);

        public static T TryOperation<T>(int maxAttempts, TimeSpan retryDelay, Func<T> operation, Predicate<Exception> exceptionFilter = null)
        {
            if (maxAttempts <= 0)
            {
                throw new ArgumentException("Max attempts cannot be less than 1");
            }

            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (exceptionFilter is null)
            {
                exceptionFilter = AnyExceptionFilter;
            }

            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    return operation.Invoke();
                }
                catch (Exception ex)
                {
                    if (exceptionFilter.Invoke(ex))
                    {
                        Thread.Sleep(retryDelay);
                        continue;
                    }
                    throw;
                }
            }
            return operation.Invoke();
        }

        public static Task TryOperationAsync(Func<Task> operation, Predicate<Exception> exceptionFilter = null)
            => TryOperationAsync(defaultMaxAttempts, defaultRetryDelay, operation, exceptionFilter);

        public static async Task TryOperationAsync(int maxAttempts, TimeSpan retryDelay, Func<Task> operation, Predicate<Exception> exceptionFilter = null)
        {
            if (maxAttempts <= 0)
            {
                throw new ArgumentException("Max attempts cannot be less than 1");
            }

            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (exceptionFilter is null)
            {
                exceptionFilter = AnyExceptionFilter;
            }

            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    await operation.Invoke().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (exceptionFilter.Invoke(ex))
                    {
                        await Task.Delay(retryDelay);
                        continue;
                    }
                    throw;
                }
            }
            await operation.Invoke();
        }

        public static Task<T> TryOperationAsync<T>(Func<Task<T>> operation, Predicate<Exception> exceptionFilter = null)
            => TryOperation(defaultMaxAttempts, defaultRetryDelay, operation, exceptionFilter);

        public static async Task<T> TryOperationAsync<T>(int maxAttempts, TimeSpan retryDelay, Func<Task<T>> operation, Predicate<Exception> exceptionFilter = null)
        {
            if (maxAttempts <= 0)
            {
                throw new ArgumentException("Max attempts cannot be less than 1");
            }

            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (exceptionFilter is null)
            {
                exceptionFilter = AnyExceptionFilter;
            }

            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    T result = await operation.Invoke().ConfigureAwait(false);
                    return result;
                }
                catch (Exception ex)
                {
                    if (exceptionFilter.Invoke(ex))
                    {
                        await Task.Delay(retryDelay);
                        continue;
                    }
                    throw;
                }
            }
            T endResult = await operation.Invoke().ConfigureAwait(false);
            return endResult;
        }
    }
}
