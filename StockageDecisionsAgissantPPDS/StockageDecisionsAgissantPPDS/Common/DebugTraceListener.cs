#if !DEBUG
using System;
#endif
using System.Diagnostics;

namespace StockageDecisionsAgissantPPDS.Common
{
    /// <summary>
    /// TraceListener de Debug
    /// </summary>
    public class DebugTraceListener : TraceListener
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public DebugTraceListener()
        {
#if !DEBUG
            throw new NotSupportedException("DO NOT USE THIS IN RELEASE");
#endif
        }

        /// <inheritdoc />
        public override void Write(string message)
        {
        }

        /// <inheritdoc />
        public override void WriteLine(string message)
        {
             Debugger.Break();
        }
    }
}
