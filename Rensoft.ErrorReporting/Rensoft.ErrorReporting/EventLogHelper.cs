using System;
using System.Diagnostics;

/// <summary>
/// Provides a means of writing errors to the event log.
/// </summary>
public class EventLogHelper
{
    private string eventLogSource;
    private string eventLogName;

    public EventLogHelper(string eventLogSource, string eventLogName)
    {
        this.eventLogSource = eventLogSource;
        this.eventLogName = eventLogName;
    }

    public string CreateErrorMessage(Exception exception)
    {
        string errorDetails = string.Empty;
        Exception currentException = exception;
        while (currentException != null)
        {
            errorDetails += "Type: " + currentException.GetType().Name + "\r\n";
            errorDetails += "Message: " + currentException.Message + "\r\n";
            errorDetails += "Stack Trace:\r\n" + currentException.StackTrace + "\r\n\r\n";

            currentException = currentException.InnerException;
        }
        return errorDetails;
    }

    public void Write(Exception exception)
    {
        // convert exception to useful text message
        Write(CreateErrorMessage(exception), EventLogEntryType.Error);
    }

    public void Write(string message, EventLogEntryType type)
    {
        if (!EventLog.SourceExists(eventLogSource))
        {
            EventLog.CreateEventSource(eventLogSource, eventLogName);
        }

        EventLog.WriteEntry(eventLogSource, message, type);
    }
}
