using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonObjects.Config;

namespace VoiceChattingClient.Configs
{
    internal class AudioConfig : Config
    {
        public string MicrophoneDeviceName { get; set; }
        public string SpeakerDeviceName { get; set; }
    }
}
