using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for DictionaryPage.xaml
    /// </summary>
    public partial class DictionaryPage : Page
    {
        string dictionaryPath;
        List<string> dictionary;

        public DictionaryPage()
        {
            InitializeComponent();
            //disable start button, will be re-enabled once the dictionary is loaded.
            startButton.IsEnabled = false;
            //set dictionary path
            //dictionaryPath = "";
            

            //load the dictionary
            Thread loader = new Thread(new ThreadStart(() => LoadDictionary()));
            loader.Start();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadDictionary()
        {
            //set up progress bar for loader.
            this.Dispatcher.Invoke((Action)(() =>
            {
                progressBar_Loader.Minimum = 0;
                progressBar_Loader.Maximum = 100;
            }));

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("10-million-password-list-top-1000000.txt")); ;

            //get number of lines in dictionary file
            using (File dictionary = assembly.)
            int lineCount = File.ReadLines(dictionaryPath).Count();
            int currentLine = 0;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader file = new StreamReader(stream))
            {
                string ln;
                double currentPercent = 0;
                double previousPercent = 0;

                while ((ln = file.ReadLine()) != null)
                {
                    dictionary.Add(ln);
                    currentLine++;
                    //calculate new percent
                    currentPercent = Math.Round((double) (currentLine / lineCount));
                    //only update UI if percentage has meaningfully changed
                    if (currentPercent > previousPercent)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            progressBar_Loader.Value = currentPercent;
                            previousPercent = currentPercent;
                        }));
                    }
                }

                //close the file
                file.Close();
            }
        }
    }
}
