using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMS.Model;

namespace WMS.BLL
{
    public class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory;
        private static readonly string _currentLogFile;
        private static readonly LogConfig _logConfig;
        
        static Logger()
        {
            _logConfig = ConfigManager.Config.LogConfig;
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logConfig.LogFilePath);
            
            // 确保日志目录存在
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
            
            // 创建当前日志文件名
            _currentLogFile = Path.Combine(_logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
            
            // 清理旧日志文件
            Task.Run(() => CleanupOldLogFiles());
        }
        
        public static void Log(string message, LogLevel level = LogLevel.Information, Exception exception = null)
        {
            // 检查日志级别
            if (level < _logConfig.MinimumLogLevel)
            {
                return;
            }
            
            string logMessage = FormatLogMessage(message, level, exception);
            
            // 记录到控制台
            if (_logConfig.LogToConsole)
            {
                Console.WriteLine(logMessage);
            }
            
            // 记录到文件
            if (_logConfig.LogToFile)
            {
                WriteToFile(logMessage);
            }
            
            // 记录到数据库
            if (_logConfig.LogToDatabase)
            {
                // 这里可以实现将日志写入数据库的逻辑
                // 为了简单起见，这里暂不实现
            }
        }
        
        public static void Trace(string message, Exception exception = null)
        {
            Log(message, LogLevel.Trace, exception);
        }
        
        public static void Debug(string message, Exception exception = null)
        {
            Log(message, LogLevel.Debug, exception);
        }
        
        public static void Info(string message, Exception exception = null)
        {
            Log(message, LogLevel.Information, exception);
        }
        
        public static void Warn(string message, Exception exception = null)
        {
            Log(message, LogLevel.Warning, exception);
        }
        
        public static void Error(string message, Exception exception = null)
        {
            Log(message, LogLevel.Error, exception);
        }
        
        public static void Critical(string message, Exception exception = null)
        {
            Log(message, LogLevel.Critical, exception);
        }
        
        private static string FormatLogMessage(string message, LogLevel level, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ");
            sb.Append($"[{level}] ");
            sb.Append($"[Thread:{Thread.CurrentThread.ManagedThreadId}] ");
            sb.Append(message);
            
            if (exception != null)
            {
                sb.AppendLine();
                sb.Append($"Exception: {exception.Message}");
                sb.AppendLine();
                sb.Append($"StackTrace: {exception.StackTrace}");
                
                if (exception.InnerException != null)
                {
                    sb.AppendLine();
                    sb.Append($"InnerException: {exception.InnerException.Message}");
                    sb.AppendLine();
                    sb.Append($"InnerStackTrace: {exception.InnerException.StackTrace}");
                }
            }
            
            return sb.ToString();
        }
        
        private static void WriteToFile(string logMessage)
        {
            try
            {
                lock (_lock)
                {
                    // 检查日志文件大小
                    CheckLogFileSize();
                    
                    // 写入日志
                    File.AppendAllText(_currentLogFile, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入日志文件失败: {ex.Message}");
            }
        }
        
        private static void CheckLogFileSize()
        {
            if (File.Exists(_currentLogFile))
            {
                FileInfo fileInfo = new FileInfo(_currentLogFile);
                long fileSizeInMB = fileInfo.Length / (1024 * 1024);
                
                if (fileSizeInMB >= _logConfig.MaxLogFileSize)
                {
                    // 如果文件大小超过限制，创建新的日志文件
                    string newFileName = Path.Combine(_logDirectory, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                    File.Move(_currentLogFile, newFileName);
                }
            }
        }
        
        private static void CleanupOldLogFiles()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_logDirectory);
                FileInfo[] files = dirInfo.GetFiles("log_*.txt");
                
                // 按创建时间排序
                Array.Sort(files, (f1, f2) => f2.CreationTime.CompareTo(f1.CreationTime));
                
                // 删除超过最大数量的旧日志文件
                if (files.Length > _logConfig.MaxLogFileCount)
                {
                    for (int i = _logConfig.MaxLogFileCount; i < files.Length; i++)
                    {
                        files[i].Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理旧日志文件失败: {ex.Message}");
            }
        }
    }
} 