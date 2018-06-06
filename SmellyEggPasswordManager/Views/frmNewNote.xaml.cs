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
    /// frmNewNote.xaml 的交互逻辑
    /// </summary>
    public partial class frmNewNote : Window
    {

        private User _currentUser;

        private NoteController _lcController;

        private Note _oldNote;

        private bool _isEdit = false;

        public frmNewNote(List<string> listType, User user, string noteType = "", bool isEdit = false, Note oldNote = null)
        {
            InitializeComponent();
            _lcController = new NoteController();
            _currentUser = user;
            _isEdit = isEdit;
            _oldNote = oldNote;
            if (_isEdit)
            {
                ShowNote(_oldNote);
            }

            cmbType.ItemsSource = null;
            cmbType.ItemsSource = listType;

            if (!string.IsNullOrEmpty(noteType) && !"所有分组".Equals(noteType))
            {
                cmbType.SelectedIndex = listType.FindIndex(p => noteType.Equals(p));
            }
        }

        private void ShowNote(Note note)
        {
            cmbType.Text = note.NoteType;
            txtContent.Text = note.NoteContent;
            txtTitle.Text = note.NoteTitle;
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!Valid())
            {
                MessageBox.Show("请检查你的输入");
            }
            else
            {
                ShowLoadingAnimation();

                Note note = new Note() { NoteType = cmbType.Text, NoteTitle = txtTitle.Text, NoteContent = txtContent.Text, UserName =  _currentUser.UserName};
                if (_isEdit)
                {
                    var result = await Task.Run(() => _lcController.UpdateNote(note, _oldNote, _currentUser));
                    ShowLoadingAnimation(false);
                    if (result == true)
                    {
                        MessageBox.Show("更新日报成功！");
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("更新日报失败！");
                    }
                }
                else
                {
                    var result = await Task.Run(() => _lcController.AddNote(note, _currentUser));
                    ShowLoadingAnimation(false);
                    if (result)
                    {
                        MessageBox.Show("新增日报成功！");
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("新增日报失败！");
                    }
                }
                
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// 显示等待动画
        /// </summary>
        /// <param name="isLoading"></param>
        private void ShowLoadingAnimation(bool isLoading = true)
        {
            if (isLoading)
            {
                myLoading.Visibility = Visibility.Visible;
                myLoading.Spin = true;
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
                myLoading.Spin = false;
                myLoading.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtTitle.Text)
                || string.IsNullOrEmpty(txtContent.Text)
                || string.IsNullOrEmpty(cmbType.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
