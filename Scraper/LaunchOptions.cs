using CommandLine;
using System.Collections.Generic;

namespace Scraper
{
	public class LaunchOptions
    {
		[Option('F', "file", Required = true, HelpText = "File name(s).", Separator = ',')]
		public IEnumerable<string> FileNames { get; set; }

		[Option('S', "strowords", Required = true, HelpText = "Words that will not be counted.", Separator = ',')]
		public IEnumerable<string> StropWords { get; set; }

		[Option('C', "characters", Required = true, HelpText = "Enable count the number of characters in the file?.")]
		public bool CountCharacters { get; set; }

		[Option('L', "captial", Required = false, Default = false, HelpText = "Enable count words that start with a Capital letter.")]
		public bool CountCapltialLetter { get; set; }
    }
}
