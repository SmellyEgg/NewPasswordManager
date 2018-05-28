using SmellyEggPasswordManager.Controller;
using SmellyEggPasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmellyEggPasswordManager.Views
{
    /// <summary>
    /// frmEditAccount.xaml 的交互逻辑
    /// </summary>
    public partial class frmEditAccount : Window
    {
        private Account _oldAccount;
        private LoginController lcController;
        private User _currentUser;

        public frmEditAccount(Account account, List<string> accountType, User user)
        {
            InitializeComponent();
            lcController = new LoginController();
            _oldAccount = account;
            _currentUser = user;
            SetAccountInfo(account, accountType);
        }

        private void SetAccountInfo(Account account, List<string> accountType)
        {
            MyAccountTypeCombo.ItemsSource = accountType;
            MyAccountTypeCombo.Text = account.AccountType;

            txtAccountName.Text = account.AccountName;
            txtAccountPassword.Password = account.AccountPassword;
            txtRepeatPassword.Password = account.AccountPassword;
        }

        private void txtRepeatPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Valid())
            {
                MessageBox.Show("请检查输入！");
            }
            else
            {
                Account newaccount = new Account() { AccountType = MyAccountTypeCombo.Text,
                AccountName = txtAccountName.Text, AccountPassword = txtAccountPassword.Password};
                if (!ValidDifference(newaccount, _oldAccount))
                {
                    MessageBox.Show("更新账户成功！");
                    DialogResult = true;
                    return;
                }
                var result = await lcController.UpdateAccount(newaccount, _oldAccount, _currentUser);
                if (result == true)
                {
                    MessageBox.Show("更新账户成功！");
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("更新账户失败！");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newaccount"></param>
        /// <param name="oldaccount"></param>
        /// <returns></returns>
        private bool ValidDifference(Account newaccount, Account oldaccount)
        {
            if (newaccount.AccountType.Equals(oldaccount.AccountType)
                && newaccount.AccountName.Equals(oldaccount.AccountName)
                && newaccount.AccountPassword.Equals(oldaccount.AccountPassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool Valid()
        {
            if (string.IsNullOrEmpty(MyAccountTypeCombo.Text)
                || string.IsNullOrEmpty(txtAccountName.Text)
                || string.IsNullOrEmpty(txtAccountPassword.Password)
                || string.IsNullOrEmpty(txtRepeatPassword.Password))
            {
                return false;
            }
            else if (!txtAccountPassword.Password.Equals(txtRepeatPassword.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
