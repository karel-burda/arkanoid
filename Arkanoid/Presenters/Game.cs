using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Arkanoid.Model;
using Arkanoid.Model.Levels;
using Arkanoid.Model.Misc;
using Arkanoid.Views;

namespace Arkanoid.Presenters
{
    /// <summary>
    /// Game encapsulates the Arkanoid game scheme
    /// It contains score, current level, input/output handlers and other managers
    /// </summary>
    sealed partial class Game
    {

        // Singleton static instance
        private static volatile Game instance;

        // Custom mutex for thread safety
        private static object mutex = new Object();

        // Input handler
        public InputHandler InputHandler { get; private set; }

        // Game settings
        public GameSettings Settings { get; private set; }

        // Game Physics handler
        public PhysicsManager PhysicsHandler { get; private set; }

        // Sound manager
        public SoundManager SoundManager { get; private set; }

        // Reference to the main window
        public MainWindow AppWindow { get; private set; }

        // Reference to the canvas
        public Canvas Canvas { get; private set; }

        // Indicates current level of the game
        public AbstractLevel Level { get; set; }
    }

    /// <summary>
    /// Game methods 
    /// </summary>
    partial class Game
    {

        /// <summary>
        /// Sample getter for thread-safe singleton usage
        /// Features Lazy initialization (new instance created, if instance not yet exists)
        /// <returns>Instance of Game class</returns>
        /// </summary>
        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (mutex)
                    {
                        if (instance == null)
                        {
                            // Check the type also - MainWindow type
                            if (Application.Current.MainWindow.GetType() != typeof(MainWindow))
                            {
                                throw new Exception("MainWindow is not of correct class");
                            }

                            instance = new Game((MainWindow)Application.Current.MainWindow);
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Class only parametrized constructor
        /// </summary>
        /// <param name="window">Reference to the application main window</param>
        private Game(MainWindow window)
        {
            
            // Application window
            this.AppWindow = window;
            this.Canvas = this.AppWindow.Canvas;

            // check references
            if (this.AppWindow == null || this.Canvas == null)
            {
                throw new Exception("Window reference or Canvas is NULL");
            }
        }

        /// <summary>
        /// Provides basic initialization of all components 
        /// </summary>
        /// <returns>Success/failure</returns>
        public bool Init()
        {

            // Creating Settings instance
            this.Settings = new GameSettings();

            // Start with level 1
            this.Level = new Level1();
            // Do the level settings
            if (this.Level.Init())
            {
                this.Level.Render();
            }
            else 
            {
                throw new Exception("Could not set up level");
            }

            // Set up handlers
            this.PhysicsHandler = new PhysicsManager();
            this.SoundManager = new SoundManager();
            this.InputHandler = new InputHandler();

            // Connect key actions
            this.AppWindow.KeyDown += this.InputHandler.KeyboardHandler.KeyDown;

            return (this.AreComponentsInitialized());
        }

        /// <summary>
        /// Checks whether all criticial inner components are initialized
        /// </summary>
        /// <returns>Success/failure</returns>
        public bool AreComponentsInitialized()
        {

            return (this.PhysicsHandler != null && this.Settings != null && this.InputHandler != null && this.InputHandler.KeyboardHandler != null);
        }

        /// <summary>
        /// Restart the whole game with current level
        /// </summary>
        /// <returns>Success/failure</returns>
        /// <remarks>Currently no use</remarks>
        public bool Restart()
        {
            
            return false;
        }

        /// <summary>
        /// Causes game to pause
        /// <remarks>Pause deliberately allows the Bat to move (embedded cheat :))</remarks>
        /// </summary>
        public void Pause()
        { 
            
            this.Settings.IsRunning = !this.Settings.IsRunning;

            // Inspect he current state
            if (!this.Settings.IsRunning)
            {
                Game.Instance.AppWindow.ShowGamePaused();
                Game.Instance.Level.Freeze();
            }
            else 
            {
                Game.Instance.AppWindow.ShowGameResumed();
                Game.Instance.Level.UnFreeze();
            }
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <returns>Success/failure</returns>
        public bool Start()
        {

            this.Settings.IsRunning = true;

            return true;
        }
    }
}
