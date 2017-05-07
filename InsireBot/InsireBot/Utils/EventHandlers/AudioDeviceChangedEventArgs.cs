using System;

namespace Maple
{
    public delegate void AudioDeviceChangedEventHandler(object sender, AudioDeviceChangedEventArgs e);

    public class AudioDeviceChangedEventArgs : EventArgs
    {
        public AudioDevice AudioDevice { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDeviceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public AudioDeviceChangedEventArgs(AudioDevice item)
        {
            AudioDevice = item;
        }
    }
}
