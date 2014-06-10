using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace LogentriesCore
{
    public static class ClassExtensions
    {
        /// <summary>
        /// Never just suppress all exceptions. Rethrow fatal ones!
        /// I.e.:
        ///     try
        ///     {
        ///         Work();
        ///     }
        ///     catch(Exception ex)
        ///     {
        ///         if (ex.IsFatal()) throw;       --- DO NOT FORGET THIS !!!
        ///         Log.Warn(ex.ToString());
        ///     }
        /// </summary>
        public static bool IsFatal(this Exception exception)
        {
            while (exception != null)
            {
                if (((exception is OutOfMemoryException) && !(exception is InsufficientMemoryException))
                    || (exception is AccessViolationException)
                    || (exception is SEHException)
                    || (exception is ThreadAbortException)        // NOTE: ThreadAbortException automatically rethrows even if you try to suppress it
                    || (exception is StackOverflowException))     // NOTE: StackOverflowException can't be caught in .NET 2.0+
                    return true;

                if (!(exception is TypeInitializationException) && !(exception is TargetInvocationException))
                    break;

                exception = exception.InnerException;
            }

            return false;
        }
    }
}
