using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    interface IMeasuringDevice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueConvert"></param>
        /// <returns></returns>

        decimal ImperialValue(int valueConvert); //Return Imperial Value of the most recent measurment generated.

        decimal MetricValue(int valueConvert); //Return Metric value of the most recent measurement generated.

        int[] GetRawData(); //Retrieve copy of all recent data that is collected - return as array of int values.

        void StartCollecting();    //Start device running - begins collection and recording.

        void StopCollecting(); //Stop device running - stops collection and recording. 

    }
}
// Data Collector Project - Roldan #2057584 - © Fall 2020
