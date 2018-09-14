using System;
using System.Collections.Generic;
using System.IO;
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

namespace RCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter = 0;
        int totalTries;

        private LowLevelKeyboardListener _listener;

        public MainWindow()
        {
            totalTries = getTries();
            InitializeComponent();

            // Show tries on GUI
            TotalLabel.Content = totalTries;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;

            _listener.HookKeyboard();
        }

        private void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.KeyPressed == Key.R)
            {
                counter++;
                NumLabel.Content = counter;

                // Play sound when counter has hit 100, and different sound at 500
                if(counter % 100 == 0 && counter % 500 != 0)
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Pokémon_Level_Up_Sound_Effect);
                    player.Play();
                } else if(counter % 500 == 0)
                {
                    System.Media.SoundPlayer player2 = new System.Media.SoundPlayer(Properties.Resources.Item_Get___Poke_mon_Colosseum_IKVXBxC4Gds);
                    player2.Play();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();
        }

        private int getTries()
        {
            int tries;
            // Get directory
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var complete = System.IO.Path.Combine(systemPath, "ResetCounter");

            var filePath = System.IO.Path.Combine(complete, "totaltries.txt");

            // Make sure file doesn't already exist
            if(!File.Exists(filePath))
            {
                // If it doesn't, create it, and write the default tries (0) to it
                Directory.CreateDirectory(complete.ToString());
                File.Create(filePath).Close();

                StreamWriter sw = new StreamWriter(filePath);
                sw.Write("0");
                sw.Close();
            }

            // Whether the file existed already or not, read the number of tries written and return it
            StreamReader sr = new StreamReader(filePath);
            tries = Convert.ToInt32(sr.ReadLine());
            sr.Close();

            // Return tries
            return tries;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var complete = System.IO.Path.Combine(systemPath, "ResetCounter");

            var filePath = System.IO.Path.Combine(complete, "totaltries.txt");

            totalTries += counter;
            counter = 0;
            NumLabel.Content = 0;

            // Update total tries counter
            TotalLabel.Content = totalTries;

            using (StreamWriter writeTotal = new StreamWriter(filePath, false))
            {
                writeTotal.WriteLine(totalTries.ToString());
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            NumLabel.Content = 0;
        }
    }
}
