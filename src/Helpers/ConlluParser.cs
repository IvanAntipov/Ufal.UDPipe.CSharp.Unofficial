using System;
using System.Collections.Generic;
using System.Linq;

namespace Ufal.UDPipe.CSharp.Unofficial.Helpers
{
    public class ConlluParser
    {
        public IReadOnlyCollection<ConlluLine> ParseConllu(string conllu)
        {
            var lines = SplitByLines(conllu);
            var items =
                lines
                    .Where(line =>
                        !String.IsNullOrWhiteSpace(line) &&
                        !line.StartsWith("#"))
                    .Select(ParseLine)
                    .ToList();
            return items;

        }

        public string ToLemText(string text, IReadOnlyCollection<string> wordTypesToUse)
        {
            return string.Join(
                " ",
                ParseConllu(text)
                    .Where(i => wordTypesToUse.Contains(i.Type))
                    .Select(i => i.Lem));
        }

        private int? TryParseId(string str)
        {
            if (Int32.TryParse(str, out var res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        private string[] SplitByFields(string line) => line.Split('\t');

        private string[] SplitByLines(string text) => text.Split('\n');

        private ConlluLine ParseLine(string line)
        {
            var fields = SplitByFields(line);
            return new ConlluLine(
                ord: TryParseId(fields[0]),
                form: fields[1],
                lem: fields[2],
                type: fields[3]);
        }
        public class WordTypes
        {
            // ReSharper disable InconsistentNaming
            public const string VERB = "VERB";
            public const string NOUN = "NOUN";
            public const string ADJ = "ADJ";
            public const string PUNCT = "PUNCT";
            public const string PRON = "PRON";
            public const string ADV = "ADV";
            public const string ADP = "ADP";
            public const string CCONJ = "CCONJ";
            // ReSharper restore InconsistentNaming
        }
        public class ConlluLine
        {
            public int? Ord { get; }
            public string Form { get; }
            public string Lem { get; }
            public string Type { get; }

            public ConlluLine(int? ord, string form, string lem, string type)
            {
                Ord = ord;
                Form = form;
                Lem = lem;
                Type = type;
            }
        }

    }
}