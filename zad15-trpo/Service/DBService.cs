using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zad15_trpo.Models;

namespace zad15_trpo.Service
{
    public class DBService
    {
        private Ratovsky15Context context;
        public Ratovsky15Context Context => context;
        private static DBService? instance;
        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }
        private DBService()
        {
            context = new Ratovsky15Context();
        }
    }
}
