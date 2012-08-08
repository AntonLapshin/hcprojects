using System;
using System.Collections.Generic;

namespace AssortmentManagement.Model
{
    class Merchant
    {
        public int ID { get; private set; }
        public string MerchName { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        public Merchant(string login, string password)
        {
            Login = login;
            Password = password;
        }

        /// <summary>
        /// Gets merch info
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        /// <param name="db">DbManager object</param>
        public void GetMerchInfo(DBManager db)
        {
            var parameters = db.CallProcedure(AssortmentProcedure.GetMerch);
            ID = Convert.ToInt32(parameters["o_merch"].ToString());
            MerchName = parameters["o_merch_name"].ToString();
        }

        public override string ToString()
        {
            return "(" + ID + ", " + MerchName + ")";
        }

        /// <summary>
        /// Gets merch list
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        /// <param name="db">DbManager object</param>
        /// <returns>Merch list</returns>
        public static SortedList<string, string> GetMerchList(DBManager db)
        {
            var parameters = db.CallProcedure(AssortmentProcedure.GetMerchList);

            var merchList = new SortedList<string, string>();
            var merches = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (merches != null)
            {
                if (merches.Count == 0) throw new AssortmentException("Список менеджеров пуст");
                foreach (var t in merches)
                {
                    merchList.Add(t["MERCH_NAME"].ToString(), t["MERCH_FAX"].ToString());
                }
            }
            else
            {
                throw new AssortmentException("Ошибка при получении списка пользователей");
            }
            return merchList;            
        }
    }
}
