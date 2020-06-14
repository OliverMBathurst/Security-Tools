﻿using FileGeneratorLibrary;
using System;
using System.IO;

namespace FileGen
{
    internal sealed class ArgumentsHandler
    {
        private readonly string[] _arguments;

        public ArgumentsHandler(string[] args) => _arguments = args;

        public void Execute()
        {
            var path = Environment.CurrentDirectory;
            var size = -1L;
            bool random = false, pause = true, postDelete = true;

            if ((_arguments.Length == 1 && (_arguments[0] == Resources.HelpSwitch || _arguments[0] == Resources.HelpShortSwitch)) || _arguments.Length == 0)
            {
                Console.WriteLine(Resources.Usage);
            }
            else
            {
                for (var i = 0; i + 1 < _arguments.Length; i++)
                {
                    switch (_arguments[i])
                    {
                        case "-path" when Directory.Exists(_arguments[i + 1]):
                            path = _arguments[i + 1];
                            break;
                        case "-size" when long.TryParse(_arguments[i + 1], out var sizeResult):
                            size = sizeResult;
                            break;
                        case "-random" when bool.TryParse(_arguments[i + 1], out var randomResult):
                            random = randomResult;
                            break;
                        case "-pause" when bool.TryParse(_arguments[i + 1], out var pauseResult):
                            pause = pauseResult;
                            break;
                        case "-postDelete" when bool.TryParse(_arguments[i + 1], out var postResult):
                            postDelete = postResult;
                            break;
                        default:
                            throw new ArgumentException(string.Format(Resources.InvalidArg, _arguments[i], Resources.Usage));
                    }
                    i++;
                }

                new FileGeneratorInstance().WriteNewFile(path, size, random, postDelete);

                if (pause)
                {
                    Console.WriteLine(Resources.FileHasBeenGenerated);
                    Console.ReadLine();
                }
            }
        }
    }
}