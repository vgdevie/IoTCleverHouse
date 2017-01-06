using System;

namespace ICH
{
    class DeviceException : Exception
    {
        public DeviceException(string message) : base (message)
        {
            //Adding Exception to log.
            CleverHouse.Log.Add(message, true);
        }
    }
}
