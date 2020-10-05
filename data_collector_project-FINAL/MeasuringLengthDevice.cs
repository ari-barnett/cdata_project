using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using static $safeprojectname$.Queue;

namespace $safeprojectname$
{
    class MeasuringLengthDevice : IMeasuringDevice
    {

        const decimal centimeter = 2.54M; //Assign the conversion factor for metric conversion (1 inch = 2.54 centimeters)
        const decimal inches = 1M; //Asssign the conversion factor for imperial conversion

        private Timer timer; // Setup for use of a timer 
        public Units unitsUsed;
        private int[] captureData;
        private int recent_measure = 0;


        Device device = null;
        dataQueue<Details_of_Measurment> queue = null; //TODO - resolved :)

        public Units UnitsToUse
        {
            set { this.unitsUsed = value; }
        }

        public MeasuringLengthDevice()
        {
            device = new Device();
            queue = new dataQueue<Details_of_Measurment>();
            queue.Limit = 10;
            recent_measure = Measurment;
            unitsUsed = Units.Imperial;
            captureData = new int[10];
        }

        public int[] GetRawData()
        {
            return captureData;
        }

        public decimal ImperialValue(int valueConvert)
        {
            decimal resulting = (decimal)valueConvert * inches;
            return resulting;
            //Performing the conversion factor to imperial 
        }

        public decimal MetricValue(int valueConvert)
        {
            decimal resulting = (decimal)valueConvert * centimeter;
            return resulting;
            //Performing the conversion factor to metric 
        }

        public void StartCollecting()
        {
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(2).TotalMilliseconds);
        }

        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                recent_measure = device.GetMeasurment;
                Debug.WriteLine("Measure Length Device Timer " + recent_measure);

                queue.Enqueue(new Details_of_Measurment()
                {
                    Measurment = recent_measure,
                    dateTime = DateTime.Now
                });
            });
        }
        

    
        public void StopCollecting()
        {
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public string Prior => BuildHistory(queue);

        private string BuildHistory(dataQueue<Details_of_Measurment> queueCollection)
        {
            //this code will loop through the collection gathered and build a string of the collected values during the operation. 
            StringBuilder buildString = new StringBuilder();
            int counter = 0;

            foreach (var t in queueCollection.queue)
            {
                // returns the appropriate value as indicated by the user selection. Wheter Metric or imperial. 
                if (unitsUsed == Units.Metric)
                {
                    buildString.AppendLine($"{MetricValue(t.Measurment)} cm");

                }
                else
                {
                    buildString.AppendLine($"{ImperialValue(t.Measurment)} in");
                }

                //this code will populate the collected data to an array form. 
                captureData[counter] = t.Measurment;
                counter++;
            }

            return buildString.ToString();
        }

        public int Measurment
        {
            get { return this.recent_measure; } 
        }
        class Details_of_Measurment
        {
            public int Measurment { get; set; }
            public DateTime dateTime { get; set; }

        }
    }
}
// Data Collector Project - Roldan #2057584 - © Fall 2020