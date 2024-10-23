using Microsoft.AspNetCore.Http;
using Mst.Logging.CustomLogs;
using Mst.Logging.Enums;
using Mst.Logging.Logger;

namespace Mst.Nlog;

public class NLogAdapter<T> : Logger<T>
{
    public NLogAdapter(IHttpContextAccessor httpContextAccessor)
        :base(httpContextAccessor)
    {
            
    } 

    protected override void LogByFavoriteLibrary(Log log, Exception exception)
    {
        string loggerMessage = log.ToString();
        var logger = NLog.LogManager.GetLogger(name: typeof(T).ToString());

        switch (log.Level)
        {
            case LogLevelEnum.Trace:
                {
                    logger.Trace(exception, message: loggerMessage);
                    break;
                }

            case LogLevelEnum.Debug:
                {
                    logger.Debug(exception, message: loggerMessage);
                    break;
                }

            case LogLevelEnum.Information:
                {
                    logger.Info(exception, message: loggerMessage);
                    break;
                }

            case LogLevelEnum.Warning:
                {
                    logger.Warn(exception, message: loggerMessage);
                    break;
                }

            case LogLevelEnum.Error:
                {
                    logger.Error(exception, message: loggerMessage);
                    break;
                }

            case LogLevelEnum.Critical:
                {
                    logger.Fatal(exception, message: loggerMessage);
                    break;
                }
        }
    }
}
