using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.SoundSystem
{
    internal class MicrophoneController
    {
        public static List<string> GetMicrophonesList()
        {
            var deviceNameList = new List<string>();
            var enumerator = new MMDeviceEnumerator();
            var deviceEnumerator = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in deviceEnumerator)
            {
                deviceNameList.Add(device.FriendlyName);
            }

            enumerator.Dispose();
            return deviceNameList;
        }

        private WaveInEvent waveInEvent;
        public event EventHandler OnDataAvaliable;

        public MicrophoneController()
        {
            int deviceNumber = 0;
            for (int i = -1; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                var caps = WaveIn.GetCapabilities(i);
                Debug.WriteLine(String.Format("Device : {0}", caps.ProductName));
                
                if (caps.ProductName.StartsWith("Mic | Line 2 (4- Audient EVO4)"))
                {
                    deviceNumber = i;
                }
            }

            waveInEvent = new WaveInEvent
            {
                DeviceNumber = deviceNumber,
                WaveFormat = new WaveFormat(rate: 44100, bits: 16, channels: 1),
                BufferMilliseconds = 10
            };
            waveInEvent.DataAvailable += WaveInEvent_DataAvailable;
            waveInEvent.StartRecording();
        }

        private void WaveInEvent_DataAvailable(object sender, WaveInEventArgs e)
        {
            OnDataAvaliable?.Invoke(this, null);

            // copy buffer into an array of integers
            Int16[] values = new Int16[e.Buffer.Length / 2];
            Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);

            // determine the highest value as a fraction of the maximum possible value
            float fraction = (float)values.Max() / 32768;

            Debug.WriteLine(String.Format(
                "Voice Meter : {0:00.0} %",
                fraction * 100));
        }
    }
}
