using CurrencyExtractor;
using CurrencyExtractor.Models;
using CurrencyTransformator;
using CurrencyTransformator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

namespace WpfWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsRunning { get; set; } = false;
        public volatile string outputPath;
        public MainWindow()
        {
            InitializeComponent();
            if (ConfigurationManager.AppSettings.AllKeys.Contains("OutputPath"))
            {
                textBox1.Text = ConfigurationManager.AppSettings["OutputPath"].ToString();
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("Output directory does not exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!IsRunning)
            {
                SaveConfig();
                IsRunning = true;
                outputPath = textBox1.Text;
                await Task.Run(() => 
                {
                    try
                    {
                        AppendLog("Starting integration");
                        AppendLog("Extraction started");
                        AppendLog("Establishing connection...");
                        var progress = new Progress<string>(value => AppendLog(value));
                        List<MediatedSchema> mediatedSchemas = Extractor.GetAllFromWeb(progress).ToList();
                        AppendLog("Mediated schema objects deserialized");

                        AppendLog("Extraction successful");
                        AppendLog("Starting transformation...");

                        bool differentCurrency, differentDate;
                        foreach (var mediated in mediatedSchemas)
                        {
                            Validator.ValidateMediatedSchemaObject(mediated, out differentCurrency, out differentDate);
                            if (differentCurrency)
                            {
                                AppendLog("Curriencies are different. Removing data");
                                continue;
                            }
                            if (differentDate)
                            {
                                AppendLog("Dates are different. Removing data");
                                continue;
                            }

                            FinalOutput finalOutput = Transformator.TransformToOutput(mediated);
                            string serialized = Serializer.SerializeFinalOutput(finalOutput);
                            File.WriteAllText(outputPath + "\\transformed-" + mediated.API.@base + ".json", serialized);
                        }

                        AppendLog("Transformation completed");
                        AppendLog("Integration finished successfully");
                    }
                    catch (Exception ex)
                    {
                        AppendLog("Exception occured. Logging details...");
                        Logger.LogException("ErrorLogs.log", ex);
                        AppendLog("Logging finished. Press any key to stop application");
                    }
                    IsRunning = false;
                });
            }
            else
            {
                MessageBox.Show("Process already running! Please wait.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AppendLog(string log)
        {
            textBox.Dispatcher.Invoke(() => 
            {
                textBox.AppendText(log + "\r\n");
                textBox.ScrollToEnd();
            });
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = dialog.SelectedPath;
                    SaveConfig();
                }
            }
         }

        private void SaveConfig()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings["OutputPath"] == null)
            {
                settings.Add("OutputPath", textBox1.Text);
            }
            else
            {
                settings["OutputPath"].Value = textBox1.Text;
            }
            configFile.Save();
        }
    }
}
