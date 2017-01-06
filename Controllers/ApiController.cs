using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICH
{
    public class ApiController
    {
        /// <summary>
        /// Performs api command.
        /// </summary>
        /// <param name="command">Command string.</param>
        /// <returns>Result.</returns>
        public async Task<HTTPResponse> Comand(string command)
        {
            try
            {
                var query = command.Split('/');

                // r/[sensor name]/[t(temperature) or h(humidity)]
                if (query[0] == "c") return await Climate(query[1], query[2]);
                // r/[relay name]
                if (query[0] == "r") return await SwitchRelay(query[1]);
                // rs/[relay name]
                if (query[0] == "rs") return RelayState(query[1]);

            }
            catch
            {
                return new HTTPResponse("Command reading error.", true);
            }

            return new HTTPResponse("Command not found.", true);
        }

        /// <summary>
        /// Getting climate.
        /// </summary>
        private async Task<HTTPResponse> Climate(string name, string type)
        {
            if (!CleverHouse.Climate.HasDevices)
                return new HTTPResponse("No climate sensors found.", true);

            if (!CleverHouse.Climate.HasDevice(name))
                return new HTTPResponse("Sensor with specified name not found.", true);

            try
            {
                if (type == "t")
                    return new HTTPResponse(await CleverHouse.Climate.Devices[name].GetTemperatureAsync());
                if (type == "h")
                    return new HTTPResponse(await CleverHouse.Climate.Devices[name].GetHumidityAsync());
            }
            catch (DeviceException ex)
            {
                return new HTTPResponse(ex.Message, true);
            }

            return new HTTPResponse("Getting climate info error.", true);
        }

        /// <summary>
        /// Controlling relay.
        /// </summary>
        private async Task<HTTPResponse> SwitchRelay(string name)
        {
            if (!CleverHouse.Relays.HasDevices)
                return new HTTPResponse("No relays found.", true);

            if (!CleverHouse.Relays.HasDevice(name))
                return new HTTPResponse("Relay with specified name not found.", true);

            try
            {
                return new HTTPResponse(await CleverHouse.Relays.Devices[name].SwitchAsync());
            }
            catch (DeviceException ex)
            {
                return new HTTPResponse(ex.Message, true);
            }
        }

        /// <summary>
        /// Getting relay state
        /// </summary>
        private static HTTPResponse RelayState(string name)
        {
            if (!CleverHouse.Relays.HasDevices)
                return new HTTPResponse("No relays found.", true);

            if (!CleverHouse.Relays.HasDevice(name))
                return new HTTPResponse("Relay with specified name not found.", true);

            try
            {
                return new HTTPResponse(CleverHouse.Relays.Devices[name].State.ToString());
            }
            catch (DeviceException ex)
            {
                return new HTTPResponse(ex.Message, true);
            }
        }
    }
}
