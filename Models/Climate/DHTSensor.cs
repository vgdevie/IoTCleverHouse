using Sensors.Dht;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ICH
{
    public class DHTSensor : IDevice
    {
        /// <summary>
        /// DHT Humidity & Temperature Sensor.
        /// Supports: DHT11, DHT22
        /// </summary>
        /// <param name="pin">Number of gpio pin data wire of sensor conected to.</param>
        /// <param name="type">Type of supported DHT sensor.</param>
        public DHTSensor(string name, int pin, DHTSensorType type)
        {
            Name = name;

            //Can throw device exception!
            var p = CleverHouse.OpenPin(pin, name);

            if (type == DHTSensorType.DHT11)
                dht = new Dht11(p, GpioPinDriveMode.Input);
            else
                dht = new Dht22(p, GpioPinDriveMode.Input);
        }

        /// <summary>
        /// Name of sensor.
        /// </summary>
        public string Name { get; set; }

        private IDht dht;

        /// <summary>
        /// Reads temperature async.
        /// </summary>
        /// <returns>Temperature in Celsium number or NV if sensor is not valid.</returns>
        public async Task<string> GetTemperatureAsync()
        {
            var r = await dht.GetReadingAsync().AsTask();

            if (!r.IsValid)
            {
                CleverHouse.Log.Add($"Trying to measure temperature on sensor { Name }, sensor is not available now.");
                return "NV";
            }

            var result = r.Temperature.ToString();

            CleverHouse.Log.Add($"Temperature { result } C measured on sensor { Name }.");

            return result;
        }

        /// <summary>
        /// Reads humidity async.
        /// </summary>
        /// <returns>Humidity persent or NV if sensor is not valid.</returns>
        public async Task<string> GetHumidityAsync()
        {
            var r = await dht.GetReadingAsync().AsTask();

            if (!r.IsValid)
            {
                CleverHouse.Log.Add($"Trying to measure humidity on sensor { Name }, sensor is not available now.");
                return "NV";
            }

            var result = r.Humidity.ToString();

            CleverHouse.Log.Add($"Humidity { result } C measured on sensor { Name }.");

            return result;
        }
    }
}
