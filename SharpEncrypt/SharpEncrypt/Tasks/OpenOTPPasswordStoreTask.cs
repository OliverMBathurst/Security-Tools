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
    internal sealed class OpenOTPPasswordStoreTask : SharpEncryptTask
    {
        public override TaskType TaskType => TaskType.OpenOTPPasswordStoreTask;

        public override bool IsSpecial => false;

        public OpenOTPPasswordStoreTask(string filePath, string keyFilePath) 
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
                    throw new OTPPasswordStoreFirstUseException();
                }

                if (!File.Exists(keyFilePath))
                {
                    Result.Value = new KeyFileStoreFileTupleModel { StoreFile = filePath, KeyFile = keyFilePath };
                    throw new OTPKeyFileNotAvailableException();
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

                Result.Value = new OpenOTPPasswordStoreTaskResult(models, filePath, keyFilePath);
            });
        }
    }
}