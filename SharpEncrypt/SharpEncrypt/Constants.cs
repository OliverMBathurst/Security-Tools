﻿using System.Text;

namespace SharpEncrypt
{
    internal static class Constants
    {
        public static string GuidIdentifier => "cd77e52c6eb14e008f5c3c548857f6a2";

        public static string DefaultLanguage => "en-GB";

        public static int KeySize = 252;

        public static byte[] GetGuidBytes() => Encoding.ASCII.GetBytes(GuidIdentifier);
    }
}
