using System;
using System.Security.Permissions;
using System.Windows;
using AssortmentManagement.UserValues;
using Oracle.DataAccess.Client;

namespace AssortmentManagement.Model
{
    public static class UnhandledExceptionHandler
    {
        public static DBManager Db { get; set; }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Initialize()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyUnhandledExceptionHandler;
        }

        public static void MyUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;

            if (e.GetType() == Type.GetType("System.OutOfMemoryException")){
                
                MessageBox.Show("Нехватка оперативной памяти", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string desc = "Type: " + e.GetType() + "Method: " + e.TargetSite + "; Message:" + e.Message + "; Stack:" + e.StackTrace;
            try
            {
                //Db.LogDetailAdd("UNHANDLED_EXCEPTION", desc);
                //Db.LogHeadUpdate();
                Logger.LogDetailAdd(Db, LogEvents.UnhandledException, desc);
                Logger.LogHeadUpdate(Db, (char)ExitCodes.Exception);
                //Db.GttTablesBackup("ERROR");
            }
            catch (OracleException oEx)
            {
                MessageBox.Show("Произошла ошибка при записи в лог: " + oEx.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
