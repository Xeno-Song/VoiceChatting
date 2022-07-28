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

        public double InputLevel { get; private set; } = 0.0;

        private WaveInEvent waveInEvent;
        public event EventHandler<WaveInEventArgs> OnDataAvaliable;

        public MicrophoneController()
        {
            
        }

        /// <summary>
        /// Open microphone controller
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns>If failed to find device, return <see langword="false"/>, else <see langword="true"/></returns>
        public bool OpenDevice(string deviceName)
        {
            int deviceNumber = -1;
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var caps = WaveIn.GetCapabilities(i);

                if (deviceName.StartsWith(caps.ProductName))
                {
                    deviceNumber = i;
                    break;
                }
            }

            if (deviceNumber == -1) return false;

            waveInEvent = new WaveInEvent
            {
                DeviceNumber = deviceNumber,
                WaveFormat = new WaveFormat(rate: 48000, bits: 16, channels: 1),
                BufferMilliseconds = 10,
                NumberOfBuffers = 256
            };
            waveInEvent.DataAvailable += WaveInEvent_DataAvailable;
            waveInEvent.StartRecording();

            return true;
        }

        public bool CloseDevice()
        {
            if (waveInEvent == null) return false;

            waveInEvent.StopRecording();
            waveInEvent.Dispose();

            waveInEvent = null;
            return true;
        }

        private void WaveInEvent_DataAvailable(object sender, WaveInEventArgs e)
        {
            OnDataAvaliable?.Invoke(this, e);

            // // copy buffer into an array of integers
            // Int16[] values = new Int16[e.Buffer.Length / 2];
            // Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);
            // 
            // // determine the highest value as a fraction of the maximum possible value
            // float fraction = (float)values.Max() / 32768;
            // 
            // Debug.WriteLine(String.Format(
            //     "Voice Meter : {0:00.0} %, Buffer Size : {1}",
            //     fraction * 100, e.Buffer.Length));
            // InputLevel = fraction * 100;
        }
    }
}
