using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Arkanoid.Model;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;
using Arkanoid.Tests;

namespace Arkanoid
{
    /// <summary>
    /// Main window of the app
    /// </summary>
    public partial class MainWindow : Window
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Window construction
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // run unit tests?
            #if DEBUG

            TestRunner testRunner = new TestRunner();
            testRunner.RunTests();
            
            #endif
        }

        /// <summary>
        /// Custom Event called after the widnows was loaded (and element sizes are resolved)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void WindowLoaded(object sender, RoutedEventArgs args)
        {

            // Start a new instance of a game
            try
            {
                // try to initialize the game
                if (!Game.Instance.Init())
                {
                    throw new Exception("Game failed to initialize");
                }

                // There is no need to check for NULL, unit test were performed on this
                Game.Instance.Start();
            }
            // If there is some problem, present it to user
            catch (Exception e)
            {
                ErrorPresenter.Show(e.Message);
            }
        }

        /// <summary>
        /// Shows label
        /// </summary>
        public void ShowGamePaused()
        {
            this.textPaused.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides label
        /// </summary>
        public void ShowGameResumed()
        {
            this.textPaused.Visibility = Visibility.Hidden;
        }
    }
}
