﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SharpEncrypt.Enums;
using SharpEncrypt.Models;

namespace SharpEncrypt.Tasks.Logging_Tasks
{
    public sealed class ReadLogFileTask : SharpEncryptTaskModel
    {
        public override TaskType TaskType => TaskType.ReadLogFileTask;

        public override bool IsExclusive => true;

        public ReadLogFileTask(string logFilePath) : base(ResourceType.File, logFilePath)
        {
            InnerTask = new Task(() =>
            {
                var lines = new List<string>();
                if (File.Exists(logFilePath))
                {
                    using (var fs = new FileStream(logFilePath, FileMode.Open))
                    {
                        if (fs.Length > 0)
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                while (!sr.EndOfStream)
                                {
                                    lines.Add(sr.ReadLine());
                                }
                            }
                        }
                    }
                }

                Result.Value = lines.ToArray();
            });
        }
    }
}
