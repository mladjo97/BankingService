﻿using System;
using System.Diagnostics;

namespace AuditManager
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "BankingService";
        const string LogName = "BankingServiceTest";

        private static void CreateEventLog()
        {
            try
            {
                if(!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                if (customLog == null)
                {
                    customLog = new EventLog();
                    customLog.Source = SourceName;
                }
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void AuthenticationSuccess(string userName)
        {
            CreateEventLog();
            
            if (customLog != null)
            {
                customLog.WriteEntry($"User {userName} was successfully authenticated.", EventLogEntryType.Information, 101, 1);                
            }
            else
            {
                throw new ArgumentException($"Error while trying to write event <AuthenticationSuccess> to event log.");
            }
        }

        public static void AuthorizationSuccess(string userName, string methodName)
        {            
            if (customLog != null)
            {
                customLog.WriteEntry($"User {userName} was successfully authorized in {methodName}.", EventLogEntryType.Information, 102, 1);
            }
            else
            {
                throw new ArgumentException($"Error while trying to write event <AuthorizationSuccess> to event log.");
            }
        }
        
        public static void AuthorizationFailed(string userName, string methodName, string reason)
        {
            if (customLog != null)
            {
                customLog.WriteEntry($"User {userName} failed authorization in {methodName}. Reason: {reason}", EventLogEntryType.Information, 103, 1);
            }
            else
            {
                throw new ArgumentException($"Error while trying to write event <AuthorizationFailed> to event log.");
            }
        }

        public static void DatabaseAction(string userName, string action)
        {
            CreateEventLog();

            if (customLog != null)
            {
                customLog.WriteEntry($"User {userName} {action.ToLower()}.", EventLogEntryType.Information, 104, 1);
            }
            else
            {
                throw new ArgumentException($"Error while trying to write event <AuthenticationSuccess> to event log.");
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
