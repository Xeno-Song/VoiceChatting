using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
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

        private WaveOutEvent waveOutEvent;
        private BufferedWaveProvider waveProvider;

        public bool OpenDevice(string deviceName)
        {
            int deviceNumber = -1;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var caps = WaveOut.GetCapabilities(i);

                if (deviceName.StartsWith(caps.ProductName))
                {
                    deviceNumber = i;
                    break;
                }
            }

            if (deviceNumber == -1) return false;

            waveOutEvent = new WaveOutEvent
            {
                DeviceNumber = deviceNumber,
                DesiredLatency = 10,
                NumberOfBuffers = 256
            };
            waveProvider = new BufferedWaveProvider(new WaveFormat(48000, 16, 1));
            waveOutEvent.Init(waveProvider);

            return true;
        }

        public bool AddPlaybackBytes(byte[] datas) => AddPlaybackBytes(datas, datas.Length);

        public bool AddPlaybackBytes(byte[] datas, int length)
        {
            if (waveOutEvent == null) return false;

            waveProvider.AddSamples(datas, 0, length);
            if (waveOutEvent.PlaybackState != PlaybackState.Playing)
                waveOutEvent.Play();

            return true;
        }

        public bool CloseDevice()
        {
            if (waveOutEvent == null) return false;

            waveOutEvent.Stop();
            waveOutEvent.Dispose();

            waveOutEvent = null;
            return true;
        }
    }
}
