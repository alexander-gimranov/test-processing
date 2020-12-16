using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper
{
    partial class Program
    {
        public record FileProcessingResult(string FileName, int? CountCharacters, int? CountCapltialLetter, Exception Exception);

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<LaunchOptions>(args)
                .WithParsedAsync(RunOptionsAsync)
                .GetAwaiter()
                .GetResult();
        }

        static async Task RunOptionsAsync(LaunchOptions opts)
        {
            var tasks = opts.FileNames.Select(fn => ProcessingFile(fn, opts.StropWords, opts.CountCharacters, opts.CountCapltialLetter));
            await Task.WhenAll(tasks);

            Console.WriteLine("Processing result(s): ");
            foreach(var task in tasks)
            {
                if (task.Result.Exception != null)
                {
                    Console.WriteLine($"'{task.Result.FileName}' - error : '{task.Result.Exception.Message}'");
                }
                else
                {
                    Console.WriteLine(task.Result.FileName);
                    if (task.Result.CountCharacters != null)
                        Console.WriteLine($"   Сount number of characters in the file - {task.Result.CountCharacters}");
                    if (task.Result.CountCapltialLetter != null)
                        Console.WriteLine($"   Count words which start with a Capital letter - {task.Result.CountCapltialLetter}");
                }
                Console.WriteLine();
            }
        }

        private static async Task<FileProcessingResult> ProcessingFile(string fn, IEnumerable<string> stropWords, bool countCharacters, bool countCapltialLetter)
        {
            int characters = 0;
            int capltialLetter = 0;

            try
            {
                using var reader = File.OpenText(fn);
                var fileText = await reader.ReadToEndAsync();

                Parallel.ForEach(fileText.Split(' ').Select(w => w.Trim()), word =>
                {
                    if (stropWords.Contains(word))
                        return;

                    if (countCharacters)
                        characters += word.Length;

                    if (countCapltialLetter && word[0] == char.ToUpper(word[0]))
                    {
                        System.Diagnostics.Debug.WriteLine(word);
                        capltialLetter++;
                    }
                });

                return new FileProcessingResult(fn, countCharacters ? characters : null, countCapltialLetter ? capltialLetter : null, null);
            }
            catch (Exception ex)
            {
                return new FileProcessingResult(fn, null, null, ex);
            }
        }
    }
}
