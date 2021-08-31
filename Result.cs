using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.ManifestCreator
{
    /// <summary>
    /// Represents a result or exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Result<T>
    {
        public Result(T? value)
        {
            this.Value = value;
            this.Error = null;
        }

        public Result(T? value, Exception? error)
        {
            this.Value = value;
            this.Error = error;
        }

        public T? Value { get; init; }
        public Exception? Error { get; init; }

        public void Deconstruct(out T? value, out Exception? error)
        {
            value = this.Value;
            error = this.Error;
        }

        public Result<TOther> Pipe<TOther>(Func<T, TOther?> selector)
        {
            if (this.Value == null)
            {
                return new Result<TOther>(default, this.Error);
            }

            var val = selector(this.Value);
            return new Result<TOther>(val);
        }
         
        public T ValueOr(Func<T> creator)
        {
            return this.Value ?? creator();
        }

        public static implicit operator Result<T>(T result) => new Result<T>(result);
        public static implicit operator Result<T>(Exception error) => new Result<T>(default, error);
    }

    public static class ResultExtensions
    {
        public static async Task<Result<TOther>> PipeAsync<T, TOther>(this Task<Result<T>> task, Func<T, Task<TOther>> selector)
        {
            try
            {
                var val = await task;
                if (val.Value != null)
                {
                    var other = await selector(val.Value);
                    return new Result<TOther>(other, val.Error);
                }
                else
                {
                    return new Result<TOther>(default, val.Error);
                }
            }
            catch (Exception error)
            {
                return new Result<TOther>(default, error);
            }          
        }

        public static async Task<Result<TOther>> PipeAsync<T, TOther>(this Task<Result<T>> task, Func<T, Task<Result<TOther>>> selector)
        {
            try
            {
                var val = await task;
                if (val.Error != null)
                {
                    return new Result<TOther>(default, val.Error);
                }

                var selectedResult = val.Pipe(selector);
                if (selectedResult.Value != null)
                {
                    var other = await selectedResult.Value;
                    return other;
                }

                return new Result<TOther>(default, selectedResult.Error);
            }
            catch (Exception error)
            {
                return new Result<TOther>(default, error);
            }
        }
    }
}
