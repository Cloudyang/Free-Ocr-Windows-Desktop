﻿using System.Windows;
using System.Windows.Input;

namespace a9t9Ocr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //I'm a newbie in mvvm, so i don't know how to move this to mvvm model :)
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Height = Properties.Settings.Default.Height;
            Width = Properties.Settings.Default.Width;

            var tess = new TesseractOcr("chi_sim");
            var baiduAI = new BaiduAIOcr("chi_sim");
            var leftVm = new LeftSideViewModel(this,tess,baiduAI);
            LeftSideControl.DataContext = leftVm;

            var rightVm = new RightSideViewModel(tess);
            RightSideControl.DataContext = rightVm;

            var view = new MainWindowViewModel(this, leftVm, rightVm);
            DataContext = view;

            MenuItemFile.DataContext = leftVm;
            MenuItemHelp.DataContext = leftVm;
            MenuItemSave.DataContext = rightVm;
            MenuItemCut.DataContext = leftVm;
  //not yet    MenuItemLanguage.DataContext = rightVm;
        }
        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CutOpen_KeyUP(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.PrintScreen)
            {
                var leftVm = LeftSideControl.DataContext as LeftSideViewModel;
                leftVm?.OpenCut(sender);
            }
        }
    }
}
