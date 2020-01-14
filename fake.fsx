#r "paket: groupref Build //"
#load "./.fake/fake.fsx/intellisense.fsx"

open Fake

open Fake.Core
open Fake.DotNet
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open System.IO
open CommandLine
open Fake.IO
open Fake.Core.TargetOperators
open Fake.IO.FileSystemOperators

let buildDir  = ".tmp/build/"
let appReferences = !! "**/*.fsproj"
let mutable dotnetExePath = "dotnet"
let currentDirectory = __SOURCE_DIRECTORY__
// --------------------------------------------------------------------------------------
// Helpers
// --------------------------------------------------------------------------------------

let throwOnNot0 res = if res <> 0 then failwithf "Error"

let runDotnet workingDir args =
    Shell.Exec(
        dotnetExePath,
        args,
        workingDir) |> throwOnNot0
        

// --------------------------------------------------------------------------------------
// Targets
// --------------------------------------------------------------------------------------

Target.create "Clean" (fun _ ->
    Shell.cleanDirs [buildDir]
)

Target.create "Restore" (fun _ ->
    appReferences
    |> Seq.iter (fun p ->
        let dir = System.IO.Path.GetDirectoryName p
        runDotnet dir "restore"
    )
)


let packBinaries(workingDirectory) =
    Shell.Exec (
        currentDirectory @@ ".paket" @@ "paket.exe",
        "pack .nupkg",
        workingDirectory) |> throwOnNot0

let createNugetPackage buildBinaries templateDir =
    printfn "Create nuget package %s" templateDir
    let tmpDir = templateDir @@  ".tmp"
    let packageDir = templateDir @@ ".nupkg"
    Shell.cleanDirs [ packageDir ; tmpDir ] |> ignore
    buildBinaries tmpDir
    packBinaries(templateDir)


Target.create "Build-Nuget" (fun _ -> 
    let projectFile = @"src\Ufal.UDPipe.CSharp.Unofficial.csproj"
    
    let dir = Path.GetDirectoryName projectFile
    
    createNugetPackage 
        (fun outDir -> runDotnet dir (sprintf "build --configuration Release  --output %s" outDir))
        (currentDirectory @@ @"Nuget"))

  
"Clean"
  ==> "Restore"
  ==> "Build-Nuget"

Target.runOrDefaultWithArguments "Build-Nuget"
