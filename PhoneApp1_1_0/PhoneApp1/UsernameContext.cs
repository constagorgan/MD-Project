using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace PhoneApp1
{
    public class UsernameContext : DataContext
    {
        public UsernameContext(string connectionString)
            : base(connectionString)
        {
        }
        public Table<UsernameClass> Usernames
        {
            get
            {
                return this.GetTable<UsernameClass>();
            }
        }

    }
}
