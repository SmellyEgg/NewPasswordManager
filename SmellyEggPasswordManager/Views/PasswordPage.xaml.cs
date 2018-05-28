using SmellyEggPasswordManager.Controller;
using SmellyEggPasswordManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmellyEggPasswordManager.Views
{
    /// <summary>
    /// PasswordPage.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordPage : Page
    {
        private User _currentUser;
        private Frame _mainFrame;
        private List<Account> _currentDataSource;

        public PasswordPage(Frame mainframe, User user)
        {
            InitializeComponent();
            lcController = new LoginController();
            _currentUser = user;
            _mainFrame = mainframe;
            Refresh();
        }

        private async void Refresh()
        {
            if (object.Equals(_currentUser, null)) return;
            LoginController lc = new LoginController();
            _currentDataSource = await Task.Run(()=> lc.GetAccounts(_currentUser));
            var filterStr = _currentDataSource.AsParallel().GroupBy(p => p.AccountType)
                .Select(p => p.FirstOrDefault().AccountType).ToList();
            if (filterStr == null) filterStr = new List<string>();
            if (filterStr.Count > 0)
            {
                filterStr.Add("所有分组");
            }

            MyListView.ItemsSource = null;
            MyListView.ItemsSource = filterStr;
            MyListView.SelectedIndex = filterStr.Count - 1;

            MyAccountListView.ItemsSource = null;
            MyAccountListView.ItemsSource = _currentDataSource;
            if (!string.IsNullOrEmpty(txtFilter.Text))
            {
                FilterAccount();
            }
        }

        private void ShowAccountByType(string accountType)
        {
            var newDataSource = _currentDataSource.Where(p => accountType.Equals(p.AccountType)).ToList();
            if ("所有分组".Equals(accountType))
            {
                newDataSource = _currentDataSource;
            }
            
            MyAccountListView.ItemsSource = null;
            MyAccountListView.ItemsSource = newDataSource;
        }

        /// <summary>
        /// 新建分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            frmNewGroup frm = new frmNewGroup();
            if (frm.ShowDialog() == true)
            {
                var listSource = MyListView.ItemsSource as List<string>;
                if (object.Equals(listSource, null)) listSource = new List<string>();
                listSource.Add(frm.GroupName);
                MyListView.ItemsSource = null;
                MyListView.ItemsSource = listSource;
            }
        }

        /// <summary>
        /// 新增账户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAccount_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var listType = MyListView.ItemsSource as List<string>;
            if (object.Equals(listType, null) || listType.Count < 1)
            {
                MessageBox.Show("请先新建一个分组");
                return;
            }
            var currentType = MyListView.SelectedIndex == -1 ? string.Empty : MyListView.SelectedItem.ToString();
            frmNewAccount frm = new frmNewAccount(listType, _currentUser, currentType);
            if (frm.ShowDialog() == true)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                FilterAccount();
            }
        }

        private void FilterAccount()
        {
            if (_currentDataSource == null) return;
            var newsource = _currentDataSource.Where(p => p.AccountName.Contains(txtFilter.Text)).ToList();
            MyAccountListView.ItemsSource = null;
            MyAccountListView.ItemsSource = newsource;
        }

        private void SelectCurrentItem(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

        LoginController lcController;

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = MyAccountListView.SelectedItem as Account;
            var result = await lcController.DeleteAccount(selectedAccount);
            if (result)
            {
                MessageBox.Show("删除成功！");
                Refresh();
            }
            else
            {
                MessageBox.Show("删除失败!");
            }
            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = MyAccountListView.SelectedItem as Account;
            var listType = MyListView.ItemsSource as List<string>;
            frmEditAccount frm = new frmEditAccount(selectedAccount, listType, _currentUser);
            if (frm.ShowDialog() == true)
            {
                Refresh();
            }
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = MyListView.SelectedItem;
            if (!object.Equals(item, null))
            {
                ShowAccountByType(item.ToString());
            }
        }
    }
}
