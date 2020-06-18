﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace GutmannLibrary
{
    public sealed class GutmannInstance
    {
        private readonly int[] _patterns = new[] { 
            85, 170, 146, 73, 
            36, 0, 17, 34, 
            51, 68, 85, 102, 
            119, 136, 153, 170,
            187, 204, 221, 238,
            255, 146, 73, 36,
            109, 182, 219
        };

        private const long BUFFER_LENGTH = 1024L;

        public void WipeFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist.");

            RandomWipe(path);
            GutmannPatternWipe(path);
            RandomWipe(path);
        }

        public static void RandomWipe(string path, int passes = 4, long bufferLength = BUFFER_LENGTH)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist.");

            using (var bw = new BinaryWriter(File.OpenWrite(path)))
            {
                using (var provider = new RNGCryptoServiceProvider())
                {
                    for (var i = 0; i < passes; i++)
                    {
                        bw.Seek(0, SeekOrigin.Begin);
                        var remainingLength = bw.BaseStream.Length;
                        var bytes = new byte[bufferLength];
                        while (remainingLength > 0)
                        {
                            if (remainingLength < bytes.Length)
                                bytes = new byte[remainingLength];

                            provider.GetNonZeroBytes(bytes);
                            bw.Write(bytes);
                            remainingLength -= bytes.Length;
                        }
                    }
                }
            }
        }

        public void GutmannPatternWipe(string path, long bufferLength = BUFFER_LENGTH)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist.");

            using (var bw = new BinaryWriter(File.OpenWrite(path)))
            {
                foreach(var number in _patterns)
                {
                    bw.Seek(0, SeekOrigin.Begin);
                    var remainingLength = bw.BaseStream.Length;
                    var bytes = new byte[bufferLength];
                    while (remainingLength > 0)
                    {
                        if (remainingLength < bytes.Length)
                            bytes = new byte[remainingLength];

                        bw.Write(Fill(bytes, number));
                        remainingLength -= bytes.Length;
                    }
                }
            }
        }

        private static byte[] Fill(byte[] array, int number)
        {
            var value = (byte)number;
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
    }
}
