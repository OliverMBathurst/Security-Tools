﻿using System.Security.Cryptography;

namespace SharpEncrypt.Helpers
{
    internal static class RsaKeyPairHelper
    {
        public static (RSAParameters publicKey, RSAParameters privateKey) GetNewKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return (rsa.ExportParameters(false), rsa.ExportParameters(true));
            }
        }

        public static RSAParameters GetNewKeyPairParameters()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return rsa.ExportParameters(true);
            }
        }
    }
}