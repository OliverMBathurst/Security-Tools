﻿using System;

namespace SharpEncrypt.Models
{
    [Serializable]
    public sealed class CreateOtpPasswordStoreKeyTaskResultModel
    {
        public string StorePath { get; set; }

        public string KeyPath { get; set; }

        public bool OpenAfter { get; set; }
    }
}