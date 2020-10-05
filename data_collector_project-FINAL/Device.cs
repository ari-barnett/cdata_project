using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace $safeprojectname$
{
    public class Device
    {
        Random random = new Random(); // for generation of a random number
        private int random_number = 0;
        private Timer timer; // setup for timer to be used for generation of random number
        



        public Device()
        {
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(1).TotalMilliseconds);
        }
        // The above code creates the timer needed for the production of the random value. 

        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                random_number = random.Next(1, 10);
            });
        }

        public int GetMeasurment => random_number;
        // The above code will generate a random number between 1-10 and will then assign that number to the public int GetMeasurment. (lines 23-31)
    }
}
// Data Collector Project - Roldan #2057584 - © Fall 2020