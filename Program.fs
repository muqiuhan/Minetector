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
    match argv with
    | [| server_path |] ->
        if IO.Directory.Exists(server_path) then
            check_server_path server_path
            Minetector(server_path).Start()
            0
        else
            failwith $"Cannot find server in {server_path}"
    | _ -> failwith "Please give me the server path"
