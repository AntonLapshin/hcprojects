using System;
using System.Collections.Generic;
using System.Net;

namespace tfs2010sync
{
    public class Connection
    {
        public static Connection RMSP = new Connection
                                            {
                                                SID = "rmsp",
                                                Hosts =
                                                    new List<Host>
                                                        {
                                                            new Host
                                                                {
                                                                    IP = new IPAddress(new byte[] {10, 0, 1, 52}),
                                                                    Port = 1621
                                                                },
                                                            new Host
                                                                {
                                                                    IP = new IPAddress(new byte[] {10, 0, 1, 54}),
                                                                    Port = 1621
                                                                },
                                                        }
                                            };

        public static Connection RMSTST = new Connection
                                              {
                                                  SID = "rmstst",
                                                  Hosts =
                                                      new List<Host>
                                                          {
                                                              new Host
                                                                  {
                                                                      IP = new IPAddress(new byte[] {10, 0, 4, 50}),
                                                                      Port = 1521
                                                                  },
                                                          }
                                              };

        public static Connection RMSTSTN = new Connection
                                               {
                                                   SID = "rmsp",
                                                   Hosts =
                                                       new List<Host>
                                                           {
                                                               new Host
                                                                   {
                                                                       IP = new IPAddress(new byte[] {10, 0, 1, 15}),
                                                                       Port = 1530
                                                                   },
                                                           }
                                               };

        public string SID { get; set; }
        public List<Host> Hosts { get; set; }
        public static Connection Current { get; set; }

        public string GetConnectionString(string login, string password)
        {
            if (Hosts == null) throw new Exception("Хост не задан");
            if (Hosts.Count == 0) throw new Exception("Хост не задан");
            if (Hosts.Count == 1)
            {
                return "User Id=" + login + ";Password=" + password + ";Data Source=(DESCRIPTION=" +
                       Hosts[0] +
                       "(CONNECT_DATA=(SID=" + SID + ")));";
            }

            return "User Id=" + login + ";Password=" + password + ";Data Source=(DESCRIPTION=" + string.Join("", Hosts) +
                   "(LOAD_BALANCE = yes)" +
                   "(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME=" + SID + ")));";
        }
    }

    public class Host
    {
        public IPAddress IP { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return "(ADDRESS=(PROTOCOL=TCP)(HOST=" + IP + ")(PORT=" + Port + "))";
        }
    }
}