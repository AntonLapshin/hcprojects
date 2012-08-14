using System;

namespace tfs2010sync
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var query = Exec.GetQuery();
                Exec.Go(query);
            }
            catch (Exception)
            {
            }
        }
    }
}
