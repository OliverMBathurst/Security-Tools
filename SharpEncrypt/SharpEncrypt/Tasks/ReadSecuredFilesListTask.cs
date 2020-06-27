﻿using SharpEncrypt.AbstractClasses;
using SharpEncrypt.Enums;
using SharpEncrypt.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SharpEncrypt.Tasks
{
    internal sealed class ReadSecuredFilesListTask : SharpEncryptTask
    {
        public override TaskType TaskType => TaskType.ReadSecuredFileListTask;
    
        public ReadSecuredFilesListTask(string path)
        {
            InnerTask = new Task(() =>
            {
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        if(new BinaryFormatter().Deserialize(fs) is List<FileDataGridItemModel> models)
                        {
                            Value = models;
                        }
                    }
                }
            });
        }
    }
}
