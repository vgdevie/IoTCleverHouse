using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ICH
{
    /// <summary>
    /// Single  relay.
    /// </summary>
    public class Relay : IDevice
    {
        private GpioPin p;
        private RelayState state = RelayState.OFF;

        public Relay(string name, int pin)
        {
            //Can throw device exception!
            p = CleverHouse.OpenPin(pin, name);

            Name = name;
            p.SetDriveMode(GpioPinDriveMode.Output);
        }

        /// <summary>
        /// Name of relay.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current state. Can be set.
        /// </summary>
        public RelayState State
        {
            get
            {
                return state;
            }
            set
            {
                if (value == RelayState.ON)
                    p.Write(GpioPinValue.Low);
                else p.Write(GpioPinValue.High);
                state = value;
                CleverHouse.Log.Add($"Relay { Name } state changed to { value.ToString() }.");
            }
        }

        /// <summary>
        /// Swithces relay state.
        /// </summary>
        /// <returns>State after switching.</returns>
        public async Task<string> SwitchAsync()
        {
            await Task.Run(() =>
            {
                State = (State == RelayState.OFF) ? RelayState.ON : RelayState.OFF;
                
            });
            return state.ToString();
        }
    }
}
