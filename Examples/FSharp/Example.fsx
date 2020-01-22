#load @".paket\load\Ufal.UDPipe.CSharp.Unofficial.fsx"

open Ufal.UDPipe
open Ufal.UDPipe.CSharp.Unofficial.Helpers
open System
open System.IO

open System.Runtime.InteropServices

let (@@) a b = Path.Combine(a,b)

module Kernel =
    [<DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
    extern bool SetDllDirectory(string lpPathName);

Kernel.SetDllDirectory( __SOURCE_DIRECTORY__ @@ @"packages\Ufal.UDPipe.CSharp.Unofficial\runtimes\win-x64")

Console.Error.Write("Loading model: ");

// download model from @"https://lindat.mff.cuni.cz/repository/xmlui/bitstream/handle/11234/1-2898/russian-syntagrus-ud-2.3-181115.udpipe?sequence=71&isAllowed=y";
let modelFile = @"c://tmp/russian-syntagrus-ud-2.3-181115.udpipe"
let model: Model = Model.load(modelFile);

if (model |> isNull) then
    Console.Error.WriteLine("Cannot load model from file '{0}'", modelFile);
    failwith "Failed"


Console.Error.WriteLine("done");




let pipeline : Pipeline = new Pipeline(model, "tokenize", Pipeline.DEFAULT, Pipeline.DEFAULT, "conllu");
    
let process text =
    use error : ProcessingError = new ProcessingError()

    let processed = pipeline.process(text, error);
    processed

let texts = 
    [
        "Мама мыла раму мылом"
    ]

let conlluResults = texts |> List.map process 

let conlluParser = ConlluParser()
let lemmatizedTexts = conlluResults |> List.map conlluParser.ToLemText

lemmatizedTexts
|> Seq.iteri(fun ind lemmatizedText -> printfn "%i: %s" ind lemmatizedText)
