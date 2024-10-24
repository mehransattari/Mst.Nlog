### Install package

`Install-Package MST.Nlog -Version 1.0.0`

### Program.cs
```
builder.Services.AddTransient(typeof(Mst.ILogger<>),
typeof(Mst.Nlog.NLogAdapter<>));
```
                             
### Insert Log In Database
```
public class LogEntry
{
    public int Id { get; set; } // Primary Key

    public DateTime LogDate { get; set; }

    public string Level { get; set; }

    public string Logger { get; set; }

    public string Message { get; set; }

    public string Exception { get; set; }
}
```

### Add file nlog.config
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog
	xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
	xsi:schemaLocation="NLog NLog.xsd"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	autoReload="true"
	internalLogFile="C:\Users\mehran\source\repos\LogNet\LogNet\wwwroot\logs\logsNLog.log"
	internalLogLevel="Trace"
	>

  <targets>

    <target xsi:type="Database" name="database"
        connectionString="Server=DESKTOP-AAOIHBN;Database=Test10Db;Trusted_Connection=True;MultipleActiveResultSets=true"
        commandText="INSERT INTO LogEntries (LogDate, Level, Logger, Message, Exception) VALUES (@log_date, @level, @logger, @message, @exception)">
      <parameter name="@log_date" layout="${longdate}" />
      <parameter name="@level" layout="${level:uppercase=true}" />
      <parameter name="@logger" layout="${logger}" />
      <parameter name="@message" layout="${message}" />
      <parameter name="@exception" layout="${exception:format=tostring}" />
    </target>


    <target xsi:type="File" name="LogFatalToFile"
			fileName="C:\Users\mehran\source\repos\LogNet\LogNet\wwwroot\logs\logsFatalMessages.log"
			layout="${longdate}|${level:uppercase=true}|${logger}|${message}|${all-event-properties} ${exception:format=tostring}"

			keepFileOpen="true"
			archiveEvery="Minute"
			archiveNumbering="DateAndSequence"
			/>

    <target xsi:type="File" name="LogErrorToFile"
			fileName="C:\Users\mehran\source\repos\LogNet\LogNet\wwwroot\logs\logsErrorMessages.log"
			layout="${longdate}|${level:uppercase=true}|${logger}|${message}|${all-event-properties} ${exception:format=tostring}"
			/>

    <target xsi:type="File" name="LogWarningToFile"
			fileName="C:\Users\mehran\source\repos\LogNet\LogNet\wwwroot\logs\logsWarningMessages.log"
			layout="${longdate}|${level:uppercase=true}|${logger}|${message}|${all-event-properties} ${exception:format=tostring}"
			/>

    <!--TempFile-->
    <target xsi:type="File" name="TempFile"
			fileName="C:\Users\mehran\source\repos\LogNet\LogNet\wwwroot\logs\logsTemp.log"
			layout="${longdate}|${level:uppercase=true}|${logger}|${message}|${all-event-properties} ${exception:format=tostring}"
			/>

    <target xsi:type="Console" name="LogToConsole"
			layout="${longdate}|${level:uppercase=true}|${logger}|${message}|${all-event-properties} ${exception:format=tostring}"
			/>
  </targets>

  <rules>
    <!-- BlackHole without writeTo -->
    <logger name="Microsoft.*" maxlevel="Info" final="true" />
    <!--
    Trace
    Debug
    Info
    Warn
    Error
    Fatal
    -->
    <logger name="*" minlevel="Fatal" maxlevel="Fatal" writeTo="LogFatalToFile" />
    <logger name="*" level="Error" writeTo="LogErrorToFile" />
    <logger name="*" level="Warn" writeTo="LogWarningToFile" />

    <logger name="*" minlevel="Trace" maxlevel="Info" writeTo="LogToConsole" />

    <logger name="Application.Program" minlevel="Trace" maxlevel="Fatal" writeTo="TempFile" />

    <logger name="*" minlevel="Trace" writeTo="database" />

  </rules>
</nlog>



```


###How use controller

```
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Log_Sample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    
    public TestController(Mst.Logging.Logger.ILogger<TestController> logger)
    {
        Logger = logger;
    }

    protected Mst.Logging.Logger.ILogger<TestController> Logger { get; }

    [HttpGet]
    public IActionResult Get()
    {
        var innerException = new Exception(message: "Inner Exception Message (1)");
        var exception = new Exception(message: "Exception Message (1)", innerException: innerException);

        var hashtable = new Hashtable();

        hashtable.Add(key: "Key1", value: "Value1");
        hashtable.Add(key: "Key2", value: "Value2");

        Logger.LogTrace(message: "Trace 1", parameters: hashtable);
        Logger.LogDebug(message: "Debug 1", parameters: hashtable);
        Logger.LogInformation(message: "Info 1", parameters: hashtable);
        Logger.LogWarning(message: "Warn 1", parameters: hashtable);
        Logger.LogError(exception, message: "Error 1", parameters: hashtable);
        Logger.LogCritical(exception, message: "Critical 1", parameters: hashtable);

        string message = "Hello, World!";

        return Ok(value: message);
    }

}

```
