using System.Linq;

namespace ICH
{
    public abstract class ADevicesController<T>
    {
        public bool HasDevices
        {
            get
            {
                return Devices.Count > 0 ? true : false;
            }
        }

        public bool HasDevice(string name)
        {
            return Devices.Where(e => (e as IDevice).Name == name).Any();
        }

        public DevicesCollection<T> Devices;
    }
}
