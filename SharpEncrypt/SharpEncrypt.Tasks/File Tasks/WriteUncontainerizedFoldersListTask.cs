﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using SharpEncrypt.Enums;
using SharpEncrypt.Models;
using SharpEncrypt.Models.Result_Models;

namespace SharpEncrypt.Tasks.File_Tasks
{
    public sealed class WriteUncontainerizedFoldersListTask : SharpEncryptTaskModel
    {
        public override TaskType TaskType => TaskType.WriteUncontainerizedFoldersListTask;

        public override bool IsExclusive => true;

        public WriteUncontainerizedFoldersListTask(string path, IReadOnlyCollection<FolderModel> directoryPaths, bool exitAfter, bool silent) 
            : base(ResourceType.File, path)
        {
            InnerTask = new Task(() =>
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, directoryPaths);
                }

                Result.Value = new FinalizableTaskResultModel
                {
                    ExitAfter = exitAfter,
                    Silent = silent
                };
            });
        }
    }
}
