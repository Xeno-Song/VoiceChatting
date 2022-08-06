using System;
using System.Collections.Generic;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal interface IVoiceData : IDisposable
    {
        int Length { get; }

        int CopyFrom(byte[] buffer, int offset, int count);
        int CopyTo(byte[] buffer, int offset);
    }
}
