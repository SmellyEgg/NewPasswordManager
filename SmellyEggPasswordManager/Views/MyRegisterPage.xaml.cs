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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmellyEggPasswordManager.Views
{
    /// <summary>
    /// MyRegisterPage.xaml 的交互逻辑
    /// </summary>
    public partial class MyRegisterPage : Page
    {
        private Frame MainFrame;

        public MyRegisterPage(Frame mainFrame)
        {
            InitializeComponent();
            MainFrame = mainFrame;
        }

        private void backButtonClick(object sender, RoutedEventArgs e)
        {
            if (!object.Equals(MainFrame, null) && MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void txtUserKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonRegister(null, null);
            }
        }

        private async void ButtonRegister(object sender, RoutedEventArgs e)
        {
            if (!Valid())
            {
                txtTipsShow.Text = "请检查你的输入";
            }
            else
            {
                User user = new User() { UserName = txtUserName.Text, UserPassword = txtPassword.Text, PasswordKey = txtUserKey.Text};
                LoginController lc = new LoginController();
                var result = await lc.TryRegister(user);
                if (result)
                {
                    MessageBox.Show("注册成功");
                    MainFrame.GoBack();
                }
                else
                {
                    txtTipsShow.Text = "注册失败，请换一个用户名!";
                }
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtUserName.Text) ||
                string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtUserKey.Text))
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
