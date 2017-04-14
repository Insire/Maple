using System;

namespace Maple.Core
{
    public interface IMapleLog
    {
        event LogMessageReceivedEventHandler LogMessageReceived;
        //
        // Summary:
        //     Logs a message object with the log4net.Core.Level.Error level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //     This method first checks if this logger is ERROR enabled by comparing the level
        //     of this logger with the log4net.Core.Level.Error level. If this logger is ERROR
        //     enabled, then it converts the message object (passed as parameter) to a string
        //     by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer. It then proceeds
        //     to call all the registered appenders in this logger and also higher in the hierarchy
        //     depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the name
        //     of the System.Exception but no stack trace. To print a stack trace use the Error(object,Exception)
        //     form instead.
        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(object message);
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Error level including the stack
        //     trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //     See the Error(object) form for more detailed information.
        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(object message, Exception exception);

        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Fatal level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //     This method first checks if this logger is FATAL enabled by comparing the level
        //     of this logger with the log4net.Core.Level.Fatal level. If this logger is FATAL
        //     enabled, then it converts the message object (passed as parameter) to a string
        //     by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer. It then proceeds
        //     to call all the registered appenders in this logger and also higher in the hierarchy
        //     depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the name
        //     of the System.Exception but no stack trace. To print a stack trace use the Fatal(object,Exception)
        //     form instead.
        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(object message);
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Fatal level including the stack
        //     trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //     See the Fatal(object) form for more detailed information.
        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal(object message, Exception exception);

        //
        // Summary:
        //     Logs a message object with the log4net.Core.Level.Info level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //     This method first checks if this logger is INFO enabled by comparing the level
        //     of this logger with the log4net.Core.Level.Info level. If this logger is INFO
        //     enabled, then it converts the message object (passed as parameter) to a string
        //     by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer. It then proceeds
        //     to call all the registered appenders in this logger and also higher in the hierarchy
        //     depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the name
        //     of the System.Exception but no stack trace. To print a stack trace use the Info(object,Exception)
        //     form instead.
        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(object message);
        //
        // Summary:
        //     Logs a message object with the INFO level including the stack trace of the System.Exception
        //     passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //     See the Info(object) form for more detailed information.
        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Info(object message, Exception exception);

        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Warn level.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        // Remarks:
        //     This method first checks if this logger is WARN enabled by comparing the level
        //     of this logger with the log4net.Core.Level.Warn level. If this logger is WARN
        //     enabled, then it converts the message object (passed as parameter) to a string
        //     by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer. It then proceeds
        //     to call all the registered appenders in this logger and also higher in the hierarchy
        //     depending on the value of the additivity flag.
        //     WARNING Note that passing an System.Exception to this method will print the name
        //     of the System.Exception but no stack trace. To print a stack trace use the Warn(object,Exception)
        //     form instead.
        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(object message);
        //
        // Summary:
        //     Log a message object with the log4net.Core.Level.Warn level including the stack
        //     trace of the System.Exception passed as a parameter.
        //
        // Parameters:
        //   message:
        //     The message object to log.
        //
        //   exception:
        //     The exception to log, including its stack trace.
        //
        // Remarks:
        //     See the Warn(object) form for more detailed information.
        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(object message, Exception exception);
    }
}
