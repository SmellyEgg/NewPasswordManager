using SmellyEggPasswordManager.Controller;
using SmellyEggPasswordManager.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmellyEggPasswordManager.Views
{
    /// <summary>
    /// MyLoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class MyLoginPage : Page
    {
        private Frame _mainFrame;

        public MyLoginPage(Frame MainFrame)
        {
            InitializeComponent();
            _mainFrame = MainFrame;
        }

        private void RegisterClick(object sender, MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new MyRegisterPage(_mainFrame));
        }

        /// <summary>
        /// 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonLoginClick(null, null);
            }
        }

        /// <summary>
        /// 登陆按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonLoginClick(object sender, RoutedEventArgs e)
        {
            
            if (!Valid())
            {
                txtTipsShow.Text = "请检查你的输入！";
            }
            else
            {
                User user = new User() { UserName = txtUserName.Text, UserPassword = txtPassword.Password};
                LoginController sql = new LoginController();
                //myLoading.Visibility = Visibility.Visible;
                //myLoading.Spin = true;
                var result = await sql.TryLogin(user);
                //myLoading.Visibility = Visibility.Hidden;
                //myLoading.Spin = false;
                if (!object.Equals(result, null))
                {
                    MessageBox.Show("登陆成功");
                    //跳转到密码管理界面
                    //_mainFrame.Navigate();
                }
                else
                {
                    txtTipsShow.Text = "账号或者密码不正确";
                }
            }
        }

        /// <summary>
        /// 启动登陆
        /// </summary>
        /// <returns></returns>
        private bool LoginProccess()
        {

            return true;
        }

        /// <summary>
        /// 验证有效性
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Password.Trim()))
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
