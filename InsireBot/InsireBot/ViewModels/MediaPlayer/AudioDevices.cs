using Maple.Core;
using System.Linq;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.AudioDevice}" />
    public class AudioDevices : BaseListViewModel<AudioDevice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDevices"/> class.
        /// </summary>
        public AudioDevices() : base()
        {
            Items.AddRange(PlaybackDeviceFactory.GetAudioDevices().ToList());
        }
    }
}
