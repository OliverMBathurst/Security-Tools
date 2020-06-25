﻿using SharpEncrypt.AbstractClasses;
using SharpEncrypt.Enums;
using SharpEncrypt.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SharpEncrypt.Tasks
{
    internal sealed class WriteSecuredFileReferenceTask : SharpEncryptTask
    {
        public override TaskType TaskType => TaskType.WriteSecuredFileReferenceTask;

        public override SharpEncryptTaskResult Result { get; } = new SharpEncryptTaskResult { Type = typeof(bool) };

        public WriteSecuredFileReferenceTask(string path, IEnumerable<FileDataGridItemModel> models, bool add = true)
        {
            InnerTask = new Task(() => 
            {
                var listOfModels = new List<FileDataGridItemModel>();
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open))
                    {                        
                        if (new BinaryFormatter().Deserialize(fs) is List<FileDataGridItemModel> list)
                        {
                            listOfModels = list;
                        }
                    }
                }

                foreach (var model in models)
                {
                    if (add)
                        listOfModels.Add(model);
                    else
                        listOfModels.RemoveAll(x => x.Secured == model.Secured);
                }

                using (var f = new FileStream(path, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(f, listOfModels);
                }
            });
        }
    }
}