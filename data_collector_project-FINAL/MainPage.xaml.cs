using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace $safeprojectname$
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Timer timer; //Prepare for a creation of a timer for the display of data. 
        MeasureDisplay data_for_display = null;
        MeasuringLengthDevice measuringLengthDevice = null;

        public MainPage()
        {
            //Utilize the MeasuringLengthDevice Class...
            measuringLengthDevice = new MeasuringLengthDevice();
            this.InitializeComponent();

            //Initially only the StartBTN should be able to be selected as to prevent errors
            stopBTN.IsEnabled = false;
            rawDataBTN.IsEnabled = false;

            //Create an if/else statement to utilize the each radio button to allow the user to select 
            //their desired data value

            if (imperialRBTN.IsChecked == true)
            {
                measuringLengthDevice.unitsUsed = Units.Imperial;
            }
            else { measuringLengthDevice.unitsUsed = Units.Metric; }

            data_for_display = new MeasureDisplay
            {
               measure = measuringLengthDevice.Measurment,
               prior_hist = measuringLengthDevice.Prior,
               AltMeasure = 0
            };

            //finally a timer is set up for the async event to be performed
            //15sec's will update the random ints that are shown on the screen...
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(2).TotalMilliseconds);
            
        }

        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                data_for_display.measure = measuringLengthDevice.Measurment;

                if (stopBTN.IsEnabled == true)
                {
                    if (metricRBTN.IsChecked == true)
                    {
                        //When metric is selected the current value is displayed in cm, alternative in inches, and past value as raw data
                        previousDataLBL.Text = $"{measuringLengthDevice.ImperialValue(measuringLengthDevice.Measurment)} in { DateTime.Now}";
                        currentDataLBL.Text = $"{measuringLengthDevice.MetricValue(measuringLengthDevice.Measurment)} cm {DateTime.Now}";
                        past_value.Text = $"{data_for_display.AltMeasure} units";
                    }
                    else
                    {
                        //When imperial is selected the current value is displayed in inches, alternative in cm, and past value as raw data
                        previousDataLBL.Text = $"{measuringLengthDevice.MetricValue(measuringLengthDevice.Measurment)} cm { DateTime.Now}";
                        currentDataLBL.Text = $"{measuringLengthDevice.ImperialValue(measuringLengthDevice.Measurment)} in {DateTime.Now}";
                        past_value.Text = $"{data_for_display.AltMeasure} units";
                    }
                    
                }

                data_for_display.AltMeasure = measuringLengthDevice.ImperialValue(measuringLengthDevice.Measurment);
                data_for_display.prior_hist = measuringLengthDevice.Prior;
                this.DataContext = null;
                this.DataContext = data_for_display;

            });
        }

            private void startButton_Click(object sender, RoutedEventArgs e)
            {
                stopBTN.IsEnabled = true; //once the start button is clicked the stop button is enabled for use
                startBTN.IsEnabled = false; //disable the start button once clicked
                rawDataBTN.IsEnabled = true;  //once the display raw data button is clicked the stop button is enabled for use
            measuringLengthDevice.StartCollecting(); //Unsure why there is an error present here...
            }

            private void stopButton_Click(object sender, RoutedEventArgs e)
            {
                startBTN.IsEnabled = true;  //once the stop button is clicked the start button is enabled for use
                stopBTN.IsEnabled = false;  //disable the stop button once clicked
            measuringLengthDevice.StopCollecting();
            }

            private async void rawDataBTN_Click(object sender, RoutedEventArgs e)
            {
                //TODO; Double check that this works - although it shows no errors... was pieced together from
                //stackoverflow... - RESOLVED
                StringBuilder dataString = new StringBuilder();
                int[] data = measuringLengthDevice.GetRawData();
                foreach (var item in data)
                {
                    if (item != 0)
                    {
                        dataString.Append(item + ",");
                    }
                }
                var dialog = new MessageDialog("Value history is printed above...", $"{dataString.ToString()}");
                await dialog.ShowAsync();
            }

            private void exitBTN_Click(object sender, RoutedEventArgs e)
            {
                Application.Current.Exit();
            }


            //Set each radio button to perform the appropriate conversion...
            private void imperialRadioButton_Checked(object sender, RoutedEventArgs e)
            {
                measuringLengthDevice.unitsUsed = Units.Imperial;
            Debug.WriteLine("Imperial Radio Button Checked");
            }

            private void metricRadioButton_Checked(object sender, RoutedEventArgs e)
            {
                measuringLengthDevice.unitsUsed = Units.Metric;
            Debug.WriteLine("Metric Radio Button Checked");
            }
        }
    } 
// Data Collector Project - Roldan #2057584 - © Fall 2020
