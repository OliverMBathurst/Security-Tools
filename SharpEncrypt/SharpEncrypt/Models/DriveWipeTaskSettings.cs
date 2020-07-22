﻿using SharpEncrypt.Enums;
using System;

namespace SharpEncrypt.Models
{
    [Serializable]
    internal sealed class DriveWipeTaskSettings
    {
        public DriveWipeType Type { get; set; }

        public bool NameObfuscation { get; set; }

        public bool PropertyObfuscation { get; set; }

        public bool WipeRounds { get; set; }

        public override string ToString() => Type.ToString();
    }
}