﻿using SharpEncrypt.AbstractClasses;
using SharpEncrypt.Enums;
using System.Threading.Tasks;

namespace SharpEncrypt.Tasks
{
    internal sealed class SecureFolderTask : SharpEncryptTask
    {
        public override bool IsLongRunning => true;

        public override TaskType TaskType => TaskType.SecureFolderTask;

        public SecureFolderTask(string folderPath) : base(ResourceType.Folder, folderPath)
        {
            InnerTask = new Task(() => {
                //logic here
                Result.Value = folderPath;
            });
        }
    }
}
