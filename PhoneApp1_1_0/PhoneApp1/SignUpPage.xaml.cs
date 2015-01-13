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

namespace PhoneApp1
{
    public partial class Page2 : PhoneApplicationPage
    {
        private const string strConnectionString = @"isostore:/UsernameDB.sdf";
        
        Boolean addBoolean = true;
        

        public Page2()
        {
            InitializeComponent();
        }


        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            IList<UsernameClass> UsernameList = this.getUsernameList();

            if (nameTextBox.Text == "" || passwordBox.Password == "")
            {
                MessageBoxResult message = MessageBox.Show("Please enter information in all fields", "Incorrect fields", MessageBoxButton.OK);
            }

            else if(nameTextBox.Text.Length < 6)
            {
                MessageBoxResult message = MessageBox.Show("Username must be longer than 5 characters", "Short username", MessageBoxButton.OK);

            }

            else if(passwordBox.Password.Length < 6)
            {
                MessageBoxResult message = MessageBox.Show("Password must be longer than 5 characters", "Short password", MessageBoxButton.OK);

            }
            else
            {
                using (UsernameContext userdb = new UsernameContext(strConnectionString))
                   {
                   // make a new object of type username class with values from textbox
                       UsernameClass newUsername = new UsernameClass
                       {
                           username = nameTextBox.Text.ToString(),
                           password = passwordBox.Password.ToString(),
                         
                       };

                       foreach (UsernameClass user in UsernameList)
                       {    
                           // test if the username is already in the DB
                           if (user.username.ToString().Equals(newUsername.username.ToString()))
                           {
                               addBoolean = false;
                               
                           }

                          
                       }

                       if (addBoolean == false)
                       {    // if the username is in the DB already, a message box pops
                           MessageBox.Show("This username already exists");
                          
                       }

                       else if(addBoolean == true)
                       {    // else, it submits the username.
                           userdb.Usernames.InsertOnSubmit(newUsername);
                           userdb.SubmitChanges();
                           MessageBoxResult result = MessageBox.Show("Username succesfully added", "Username added", MessageBoxButton.OK);
                            if (result == MessageBoxResult.OK)
                            {
                                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                            }
                           
                          
                       }
                       // refresh addboolean 
                       addBoolean = true;
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




        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        
    }
}