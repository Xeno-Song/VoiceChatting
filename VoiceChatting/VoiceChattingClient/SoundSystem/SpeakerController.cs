using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.SoundSystem
{
    internal class SpeakerController
    {
        public static List<string> GetSpeakerList()
        {
            var deviceNameList = new List<string>();
            var enumerator = new MMDeviceEnumerator();
            var deviceEnumerator = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in deviceEnumerator)
            {
                deviceNameList.Add(device.FriendlyName);
            }

            enumerator.Dispose();
            return deviceNameList;
        }
    }
}
