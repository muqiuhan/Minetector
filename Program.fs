module Minetector.Main

open Minetector.Minetector

open System

let check_server_path (path: string) : unit =
    let files =
        IO.Directory.GetFiles(path) |> Array.map (fun file -> IO.Path.GetFileName(file))

    if
        (Array.exists (fun file -> file = "run.bat") files
         && Array.exists (fun file -> file = "run.sh") files)
    then
        ()
    else
        failwith $"The {path} is not a valid sevrer path"

[<EntryPoint>]
let main (argv: array<string>) : int =
    check_server_path "."
    Minetector(server_path).Start()
    0
