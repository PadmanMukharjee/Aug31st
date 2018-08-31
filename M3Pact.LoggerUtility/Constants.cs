namespace M3Pact.LoggerUtility
{
    /// <summary>
    /// Constants class contains all the naming conventions used in configuration file and other string constants.
    /// </summary>
    class Constants
    {
        public const string AppConfig_Key_Target = "LoggerSettings:Target";
        public const string AppConfig_Key_LogLevel = "LoggerSettings:LogLevel";
        public const string AppConfig_Key_FlatFilePath = "LoggerSettings:FlatFileTargetPath";
        public const string AppConfig_Key_DBTargetPath = "LoggerSettings:DBTargetPath";
        public const string AppConfig_Key_ParamDelimiterInLog = "LoggerSettings:ParamDelimiterInLog";
        public const string DBConfig_Application_Connection_String = "ConnectionStrings:M3PactConnection";
        public const string DBConfig_Attribute_FromSchema = @"/DBConfig/Table/@fromSchema";
        public const string DBConfig_Attribute_TableName = @"/ DBConfig / Table / @name";
        public const string DBConfig_Node_DBConnectionString = @"/DBConfig/ConnectionParameters/ConnectionString";
        public const string DBConfig_Node_Columns = "Columns";
        public const string DBConfig_Node_AdditionalColumns = "AdditionalColumns";
        public const string DBConfig_Attribute_Name = "name";
        public const string DBConfig_Attribute_MappedTo = "mappedTo";
        public const string DBConfig_Attribute_Type = "type";
        public const string Time = "time";
        public const string LogLevel = "loglevel";
        public const string Message = "message";
        public const string Exception = "exception";
        public const string StackTrace = "stacktrace";
        public const string ExceptionMessage_MappedToNotFound = "Wrong or no Mappedto field is specified in the config file";
        public const string FileTargetPath = "TargetPath";
        public const string LogFrequencyForFlatFile = "LoggerSettings:LogFrequency";
        public const string AllowedFileExtensionsForLogging = ".txt";
        public const string InvalidFileFormatException = "Invalid file format.Only text files are allowed";
        public const string LogFileDateFormat = "ddMMyyyy";
    }
}
