using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Data.Linq;

namespace PhoneApp1
{
    public partial class changePasswordPage : PhoneApplicationPage
    {

        private const string strConnectionString = @"isostore:/UsernameDB.sdf";

        public changePasswordPage()
        {
            InitializeComponent();
        }

        String username;
        

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void changePassButton_Click(object sender, RoutedEventArgs e)
        {
            IList<UsernameClass> UsernameList = this.getUsernameList();

            if (oldPasswordBox.Password == "" || newPasswordBox.Password == "" || confirmPasswordBox.Password == "")
            {
                MessageBoxResult message = MessageBox.Show("Please enter information in all fields", "Incorrect fields", MessageBoxButton.OK);
                return;
            }

            else if (newPasswordBox.Password != confirmPasswordBox.Password)
            {
                MessageBoxResult message = MessageBox.Show("New password has different values", "Incorrect fields", MessageBoxButton.OK);
                return;
            }


            else if (newPasswordBox.Password.Length < 6 || confirmPasswordBox.Password.Length < 6)
            {
                MessageBoxResult message = MessageBox.Show("New password must be longer than 6 characters", "Incorrect fields", MessageBoxButton.OK);
                return;
            }

           using (UsernameContext userdb = new UsernameContext(strConnectionString))
           {

               UsernameClass newUsername = new UsernameClass
               {
                   username = username.ToString(),
                   password = oldPasswordBox.Password.ToString()

               };


               

               var query = from User in userdb.Usernames where User.username == username select User;
               foreach (UsernameClass User in query)
               {
                   if (User.password != oldPasswordBox.Password.ToString())
                   {
                       MessageBoxResult message = MessageBox.Show("Wrong old password", "Wrong password", MessageBoxButton.OK);
                       return;
                   }
                   else
                   {
                       MessageBoxResult result = MessageBox.Show("Password succesfully changed", "New password", MessageBoxButton.OK);
                       User.password = newPasswordBox.Password.ToString();
                       if (result == MessageBoxResult.OK)
                       {
                           NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
                       }
                   }
               }

                 try
                {
                   userdb.SubmitChanges();
                }


                 catch (Exception f)
                 {
                     Console.WriteLine(f);
                     // Provide for exceptions.
                 }

           }

         }

        


        public IList<UsernameClass> getUsernameList()
        {
            IList<UsernameClass> UsernameList = null;
            using (UsernameContext userdb = new UsernameContext(strConnectionString))
            {

                IQueryable<UsernameClass> userQuery = from User in userdb.Usernames select User;
                UsernameList = userQuery.ToList();

            }

            return UsernameList;
        }

         protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("Text"))
                username = (string)PhoneApplicationService.Current.State["Text"];
        }


    }
}