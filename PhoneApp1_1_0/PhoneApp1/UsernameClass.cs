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
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace PhoneApp1
{
   
    [Table]
    public class UsernameClass
    {
        [Column(IsPrimaryKey = true)]
        public String username
        {get;
        set;
        }
        [Column(CanBeNull = false)]
        public String password
        {
            get;
            set;
        }
       
    }
}

