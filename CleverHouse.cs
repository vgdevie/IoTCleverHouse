using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ICH
{
    public static class CleverHouse
    {
        /// <summary>
        /// Log list.
        /// </summary>
        public static LogCollection Log = new LogCollection();

        /// <summary>
        /// Relays controller.
        /// </summary>
        public static RelayController Relays = new RelayController();

        /// <summary>
        /// Climate controller.
        /// </summary>
        public static ClimateController Climate = new ClimateController();

        /// <summary>
        /// Api controller.
        /// </summary>
        public static ApiController Api = new ApiController();

        /// <summary>
        /// Http Server.
        /// </summary>
        public static HTTPServer HttpServer = new HTTPServer();

        /// <summary>
        /// Opens pin on gpio controller
        /// </summary>
        /// <param name="number">Number of gpio pin data wire conected to.</param>
        /// <param name="sensorName">Name of sensor you want to connect.</param>
        /// <returns>Gpio Pin.</returns>
        /// <exception cref="DeviceException">Gpio Controller not found.</exception>
        /// <exception cref="DeviceException">"Can not open pin.</exception> 
        public static GpioPin OpenPin(int number, string sensorName)
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null) throw new DeviceException("Gpio Controller not found for sensor " + sensorName + ".");
            var p = gpio.OpenPin(number);
            if (p == null) throw new DeviceException("Can not open pin " + number.ToString() + " for sensor " + sensorName + ".");
            return p;
        }
       
    }

   
}
