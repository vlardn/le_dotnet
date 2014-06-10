using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

using LogentriesCore.Net;

namespace NLog.Targets
{
    [Target("Logentries")]
    public sealed class LogentriesTarget : TargetWithLayout
    {
        private AsyncLogger logentriesAsync;

        public LogentriesTarget()
        {
            logentriesAsync = new AsyncLogger()
            { 
                LogInternalDebug = InternalLogger.Debug,
                LogInternalInfo = InternalLogger.Info,
                LogInternalWarn = InternalLogger.Warn,
                LogInternalError = InternalLogger.Error 
            };
        }

        /** Debug flag. */
        public bool Debug 
        {
            get { return logentriesAsync.getDebug(); }
            set { logentriesAsync.setDebug(value); } 
        }

        /** Option to set Token programmatically or in Appender Definition */
        public string Token
        {
            get { return logentriesAsync.getToken(); }
            set { logentriesAsync.setToken(value); }
        }

        /** HTTP PUT Flag */
        public bool HttpPut
        {
            get { return logentriesAsync.getUseHttpPut(); }
            set { logentriesAsync.setUseHttpPut(value); }
        }

        /** SSL/TLS parameter flag */
        public bool Ssl
        {
            get { return logentriesAsync.getUseSsl(); }
            set { logentriesAsync.setUseSsl(value); }
        }

        /** ACCOUNT_KEY parameter for HTTP PUT logging */
        public String Key
        {
            get { return logentriesAsync.getAccountKey(); }
            set { logentriesAsync.setAccountKey(value); }
        }

        /** LOCATION parameter for HTTP PUT logging */
        public String Location
        {
            get { return logentriesAsync.getLocation(); }
            set { logentriesAsync.setLocation(value); }
        }

        public bool AppendExceptionString { get; set; }

        public bool KeepConnection { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            string renderedEvent = this.Layout.Render(logEvent);

            if (AppendExceptionString && logEvent.Exception != null)
            {
                renderedEvent += String.Format(", {0}", logEvent.Exception);
            }

            logentriesAsync.AddLine(renderedEvent);
        }

        protected override void CloseTarget()
        {
            base.CloseTarget();

            logentriesAsync.Stop();
        }
    }
}
