using System;

namespace AssortmentManagement.Model
{
    static class Logger
    {
        public static void LogHeadCreate(DBManager db)
        {
            Call(db, AssortmentProcedure.LogHeadCreate);
        }

        public static void LogDetailAdd(DBManager db, string eventType, string eventDesc)
        {
            AssortmentProcedure.LogDetailAdd.Parameters["i_event_type"].Value = eventType;
            AssortmentProcedure.LogDetailAdd.Parameters["i_event_desc"].Value = eventDesc.Substring(0, eventDesc.Length < 4000 ? eventDesc.Length : 4000);
            Call(db, AssortmentProcedure.LogDetailAdd);
        }

        public static void LogHeadUpdate(DBManager db, char status)
        {
            AssortmentProcedure.LogHeadUpdate.Parameters["i_status"].Value = status;
            Call(db, AssortmentProcedure.LogHeadUpdate);
        }

        public static void LogHeadDelete(DBManager db)
        {
            Call(db, AssortmentProcedure.LogHeadDelete);
        }

        private static void Call(DBManager db, AssortmentProcedure proc)
        {
            try
            {
                var parameters = db.CallProcedure(proc);
            }
            catch (AssortmentException e)
            {
                Console.Error.WriteLine("Log Write Error: " + proc.Name + "; Message: " + e.Message);
            }
        }
    }
}
