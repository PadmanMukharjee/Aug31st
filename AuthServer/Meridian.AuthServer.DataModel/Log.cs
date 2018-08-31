using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.DataModel
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Exception { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Time { get; set; }
    }
}
