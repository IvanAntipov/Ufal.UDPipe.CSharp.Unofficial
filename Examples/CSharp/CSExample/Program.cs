using System;
using System.Linq;
using Ufal.UDPipe;
using Ufal.UDPipe.CSharp.Unofficial.Helpers;

namespace CSExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading model...");

            var modelFile = @"c://tmp/russian-syntagrus-ud-2.3-181115.udpipe";

            Model model  = Model.load(modelFile);

            if (model == null)
            {
                Console.Error.WriteLine("Cannot load model from file '{0}'", modelFile);
                throw new Exception("Failed");
            }


            Console.Error.WriteLine("done");




            Pipeline pipeline = new Pipeline(model, "tokenize", Pipeline.DEFAULT, Pipeline.DEFAULT, "conllu");

            string Process(string text)
            {
                using (ProcessingError error = new ProcessingError())
                {
                    var processedConllu = pipeline.process(text, error);
                    if (error.occurred())
                    {
                        throw new Exception($"Error occured {error.message}");
                    }
                    return processedConllu;
                }
            }

            var texts = new[]
            {
                "Мама мыла раму мылом"
            };

            var conlluResults = texts.Select(Process).ToList();

            ConlluParser conlluParser = new ConlluParser();
            
            var lemmatizedTexts = conlluResults.Select(conllu => conlluParser.ToLemText(conllu)).ToList();

            foreach (var text in lemmatizedTexts)
            {
                Console.WriteLine($"{text}");
            }
        }
    }
}
