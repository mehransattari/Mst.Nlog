using Microsoft.AspNetCore.Http;
using Moq;
using Mst.Logging.CustomLogs;
using Mst.Logging.Enums;
using Xunit.Sdk;

namespace Mst.Nlog.Test;

public class UnitTest1
{
  [Fact]  
    public void LogByFavoriteLibrary_ShouldLogCorrectly_ForDifferentLogLevels()  
    {  
        // Arrange  
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();  
        var logger = NLog.LogManager.GetLogger("TestLogger");  
        var logAdapter = new NLogAdapter<TestClass>(mockHttpContextAccessor.Object);  
        var log = new Log { Level = LogLevelEnum.Information }; // مثال با یک سطح لاگ   

        // حفظ حالت نمایش از NLog  
        var mockLogger = new Mock<ILogger>();  

        using (LogManager.SuppressInternalTrace())  
        {  
            LogManager.UseLocalization(false);  
            LogManager.Configuration = new NLog.Config.LoggingConfiguration();  
            LogManager.Configuration.AddRule(LogLevel.Debug, LogLevel.Fatal, mockLogger.Object);  
            LogManager.ReconfigExistingLoggers();  
            
            // Act  
            logAdapter.LogByFavoriteLibrary(log, null);  

            // Assert  
            mockLogger.Verify(x => x.Info(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once); // برای سطح Information  
        }  
    }  
}