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

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string strConnectionString = @"isostore:/UsernameDB.sdf";
        Boolean testUsername = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (UsernameContext userdb = new UsernameContext(strConnectionString))
            {
                // if DB does not exist
                if (userdb.DatabaseExists() == false)
                {
                    //Create db
                    userdb.CreateDatabase();
                    //add UsernamenClass objects to it.
                    UsernameClass user1 = new UsernameClass
                    {
                        username = "Justin",
                        password = "123456"

                    };
                    userdb.Usernames.InsertOnSubmit(user1);
                    userdb.SubmitChanges();
                }
            }
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {

            IList<UsernameClass> UsernameList = this.getUsernameList();
            testUsername = false;

            // test if the textboxes are correct.
            if (nameTextBox.Text == "" || passwordBox.Password == "")
            {
                MessageBoxResult message = MessageBox.Show("Please enter information in all fields", "Incorrect fields", MessageBoxButton.OK);

            }

            else
            {
                UsernameClass newUser = new UsernameClass
                {
                    username = nameTextBox.Text.ToString(),
                    password = passwordBox.Password.ToString()
                    
                };

                foreach (UsernameClass user in UsernameList)
                {
                    // test if the username is already in the DB
                    if (user.username.ToString().Equals(newUser.username.ToString()))
                    {
                       testUsername = true;
                       if(user.password.ToString().Equals(newUser.password.ToString()))
                       {
                             NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
                       }

                       else
                       {
                           MessageBoxResult message = MessageBox.Show("Wrong password", "Wrong password", MessageBoxButton.OK);
                       }
                    }
                        
                   
                }

                if (testUsername == false)
                {
                    MessageBoxResult message = MessageBox.Show("Wrong username", "Wrong username", MessageBoxButton.OK);
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

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUpPage.xaml", UriKind.Relative));
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Called when a page becomes the active page in a frame
            base.OnNavigatedFrom(e);
            // Text is param, you can define anything instead of Text 
            // but remember you need to further use same param.
            PhoneApplicationService.Current.State["Text"] = nameTextBox.Text ;
        }

       

       

    }
}