﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using SharpEncrypt.Enums;
using SharpEncrypt.Models;

namespace SharpEncrypt.Tasks.Folder_Tasks
{
    public sealed class WriteSecuredFoldersListTask : SharpEncryptTaskModel
    {
        public override TaskType TaskType => TaskType.WriteSecuredFoldersListTask;

        public WriteSecuredFoldersListTask(string filePath, bool add, params FolderModel[] models) : base(ResourceType.File, filePath)
        {
            InnerTask = new Task(() =>
            {
                var created = false;
                if (!File.Exists(filePath))
                {
                    using (var _ = File.Create(filePath)) { }
                    created = true;
                    if (!add)
                        return;
                }

                var dirs = new List<FolderModel>();
                if (!created)
                {
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        if (fs.Length != 0 && new BinaryFormatter().Deserialize(fs) is List<FolderModel> folders)
                        {
                            dirs = folders;
                        }
                    }
                }

                if (add)
                {
                    dirs.AddRange(models.Where(x => x.Uri != null));
                }
                else
                {
                    if (dirs.Any())
                    {
                        dirs.RemoveAll(x => models.Any(z => z.Uri.Equals(x.Uri, StringComparison.Ordinal)));
                    }
                }
                

                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    new BinaryFormatter().Serialize(fs, dirs.Distinct().ToList());
                }

                if (add)
                    Result.Value = dirs;
            });
        }
    }
}