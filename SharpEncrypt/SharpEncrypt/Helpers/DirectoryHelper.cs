﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AesLibrary;
using FileGeneratorLibrary;
using SharpEncrypt.Models;

namespace SharpEncrypt.Helpers
{
    internal static class DirectoryHelper
    {
        public static string GetDirectoryPath(string dir)
        {
            if(dir == null)
                throw new ArgumentNullException(nameof(dir));

            var dirInfo = Path.GetDirectoryName(dir);
            if (dirInfo == null)
                return Directory.GetDirectoryRoot(dir);
            
            return dirInfo.Length > 0 ? dirInfo : null;
        }

        public static IEnumerable<FolderModel> EnumerateAndSecureSubFolders(string folderPath, string password, string ext) =>
            Directory.GetDirectories(folderPath).Select(x
                => new FolderModel
                {
                    Uri = x,
                    FileModels = EnumerateAndSecureFiles(x, password, ext).ToList(),
                    SubFolders = EnumerateAndSecureSubFolders(x, password, ext).ToList()
                });

        public static IEnumerable<FileModel> EnumerateAndSecureFiles(string folderPath, string password, string ext)
        {
            var fileModels = new List<FileModel>();

            foreach (var filePath in Directory.GetFiles(folderPath).Where(x => !Path.GetExtension(x).Equals(ext)))
            {
                ContainerHelper.ContainerizeFile(filePath, AesHelper.GetNewAesKey(), password);
                var newPath = FileGeneratorHelper.GetValidFileNameForDirectory(
                    GetDirectoryPath(filePath),
                    Path.GetFileNameWithoutExtension(filePath),
                    ext);

                File.Move(filePath, newPath);

                fileModels.Add(new FileModel
                {
                    File = Path.GetFileName(filePath),
                    Time = DateTime.Now,
                    Secured = newPath
                });
            }

            return fileModels;
        }

        public static void DecontainerizeDirectoryFiles(FolderModel masterModel, string folderPath, string password, string encryptedFileExtension, bool includeSubFolders)
        {
            foreach (var filePath in Directory.GetFiles(folderPath).Where(x => !Path.GetExtension(x).Equals(encryptedFileExtension)))
            {
                ContainerHelper.DecontainerizeFile(filePath, password);

                var fileModel = folderPath.Equals(masterModel.Uri)
                    ? masterModel.FileModels.FirstOrDefault(x => x.Secured.Equals(filePath))
                    : GetFileModel(masterModel, file => file.Secured.Equals(filePath));

                var ext = string.Empty;
                if (fileModel != null)
                    ext = Path.GetExtension(fileModel.File);

                var newPath = FileGeneratorHelper.GetValidFileNameForDirectory(
                    GetDirectoryPath(filePath),
                    Path.GetFileNameWithoutExtension(filePath),
                    ext);

                File.Move(filePath, newPath);
            }

            if (!includeSubFolders) return;

            foreach (var subFolderPath in Directory.GetDirectories(folderPath))
                DecontainerizeDirectoryFiles(masterModel, subFolderPath, password, encryptedFileExtension, true);
        }

        public static FolderModel GetSubFolderModel(FolderModel masterModel, Func<FolderModel, bool> predicate)
        {
            foreach (var subFolderModel in masterModel.SubFolders.Where(predicate))
                return subFolderModel;

            return masterModel.SubFolders.Select(subFolder => GetSubFolderModel(subFolder, predicate))
                .FirstOrDefault(model => model != null);
        }

        public static FileModel GetFileModel(FolderModel masterModel, Func<FileModel, bool> predicate)
        {
            foreach (var fileModel in masterModel.FileModels.Where(predicate))
                return fileModel;

            return masterModel.SubFolders.Select(subFolder => GetFileModel(subFolder, predicate))
                .FirstOrDefault(model => model != null);
        }
    }
}