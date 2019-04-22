using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PasswordCheckServer
{
    /// <summary>
    /// Interaction logic for BruteForcePage.xaml
    /// </summary>
    public partial class BruteForcePage : Page
    {
        String password;
        static char[] Match;
        int Combi;
        String FindPassword;
        static int Characters;
        static Stopwatch timer;
        bool keepTimer;
        double possibleCombos;
        Thread thread;
        Thread timeThread;
        Thread progressThread;
        bool found;

        public BruteForcePage()
        {
            InitializeComponent();

            //define characters for testing purposes
            Match = new char[]{'a','A','b','B','c','C','d','D','e','E','f','F','g','G','h','H','i','I','j','J','k','K',
                                              'l','L','m','M','n','N','o','O','p','P','q','Q','r','R','s','S','t','T','u','U','v','V',
                                              'w','W','x','X','y','Y','z','Z',',','.','!','?'};

            timer = new Stopwatch();
            keepTimer = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            FindPassword = passwordBox.Text;
            Characters = FindPassword.Length;
            timer.Start();
            
            //calculate the possible combos
            double n = Match.Length;
            double r = Characters;
            /*
            //store n in a temp variable while we calculate the factorial
            double ntemp = n;
            double nFactorial = 1;
            while (ntemp != 1)
            {
                nFactorial = nFactorial * ntemp;
                ntemp = ntemp - 1;
            }
            //pnow calculate n - r factorial
            double n_rFactorial = 1;
            double n_r = n - r;
            while (n_r != 1)
            {
                n_rFactorial = n_rFactorial * n_r;
                n_r = n_r - 1;
            }
            possibleCombos = nFactorial / n_rFactorial;
            */
            possibleCombos = Math.Pow(n, r);

            this.Dispatcher.Invoke((Action)(() =>
            { possibleCombosLabel.Content = "" + possibleCombos; }));

            progressBar.Maximum = possibleCombos;
            progressBar.Minimum = 0;

            //set potential time assuming ~9,300 combos per second
            timeLeft.Content = "" + Math.Round((possibleCombos / 9300) / 60 / 60,2) + " Hours";

            //spawn thread doing the password checking
            thread = new Thread(new ThreadStart(RunRecurse));
            thread.Start();
            //timeThread = new Thread(new ThreadStart(UpdateTimer));
            //timeThread.Start();
            progressThread = new Thread(new ThreadStart(UpdateProgress));
            progressThread.Start();

        }

        void RunRecurse()
        {
            //reset variables
            keepTimer = true;
            Combi = 0;
            found = false;
            for (int i = 1; i <= Characters; i++)
            {
                Recurse(i, 0, "");
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                guessing.Content = "WE ARE DONE!!!";
                triedCombos.Content = "" + Combi;
                timer.Stop();
                keepTimer = false;
            }));
            thread.Abort();
        }

        void Recurse(int Lenght, int Position, string BaseString)
        {
            //int Count = 0;

            for (int Count = 0; Count < Match.Length; Count++)
            {
                if (found == true)
                {
                    break;
                }
                Combi++;
                if (Position < Lenght - 1)
                {
                    Recurse(Lenght, Position + 1, BaseString + Match[Count]);
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    guessing.Content = BaseString + Match[Count];
                }));
                if ((BaseString + Match[Count]).Equals(FindPassword))
                {
                    //
                    found = true;
                    break;
                }
            }
        }

        /*
        private void UpdateTimer()
        {
            while (keepTimer)
            {
                this.Dispatcher.Invoke((Action)(() =>
                { timeElapsed.Content = timer.Elapsed; }));
                Thread.Sleep(500);
            }
            timer.Stop();
            timeThread.Abort();
        }
        */

        private void UpdateProgress()
        {
            while (keepTimer)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    timeElapsed.Content = timer.Elapsed.ToString("g");
                    progressBar.Value = Combi;
                    percentNumber.Content = "" + Math.Round(Combi / possibleCombos * 100,2) + "%";
                    triedCombos.Content = "" + Combi;
                    //calcluate possible time left:
                    //var timeLeftCurrent = timer.Elapsed.Seconds / Combi * possibleCombos;
                    //timeLeft.Content = "" + timeLeftCurrent;
                }));
            }
            progressThread.Abort();
        }
    }
}
