﻿using OTPLibrary;
using SharpEncrypt.AbstractClasses;
using SharpEncrypt.Enums;
using SharpEncrypt.Exceptions;
using SharpEncrypt.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SharpEncrypt.Tasks
{
    internal sealed class OpenOtpPasswordStoreTask : SharpEncryptTask
    {
        public override TaskType TaskType => TaskType.OpenOTPPasswordStoreTask;

        public OpenOtpPasswordStoreTask(string filePath, string keyFilePath) 
            : base(ResourceType.File, filePath, keyFilePath)
        {
            InnerTask = new Task(() =>
            {
                if (!File.Exists(filePath))
                {
                    using (var fs = File.Create(filePath)) 
                    {
                        new BinaryFormatter().Serialize(fs, new List<PasswordModel>());
                    }
                    throw new OtpPasswordStoreFirstUseException();
                }

                if (!File.Exists(keyFilePath))
                {
                    Result.Value = new KeyFileStoreFileTupleModel { StoreFile = filePath, KeyFile = keyFilePath };
                    throw new OtpKeyFileNotAvailableException();
                }

                if (new FileInfo(filePath).Length > new FileInfo(keyFilePath).Length)
                {
                    throw new InvalidKeyException();
                }

                OTPHelper.Transform(filePath, keyFilePath);

                var models = new List<PasswordModel>();
                using(var fs = new FileStream(filePath, FileMode.Open))
                {
                    if (fs.Length > 0)
                    {
                        if (new BinaryFormatter().Deserialize(fs) is List<PasswordModel> list)
                        {
                            models = list;
                        }
                    }
                }

                OTPHelper.Transform(filePath, keyFilePath);

                Result.Value = new OpenOtpPasswordStoreTaskResult(models, filePath, keyFilePath);
            });
        }
    }
}
