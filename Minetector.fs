module Minetector.Minetector

open Minetector.Environment
open System

type Minetector(_serverPath: string) =
    let _processInfo: Diagnostics.ProcessStartInfo =
        match Environment.OS with
        | Windows ->
            let info = new Diagnostics.ProcessStartInfo("cmd.exe", "/crun.bat")
            info.RedirectStandardError <- true
            info.RedirectStandardInput <- true
            info.RedirectStandardOutput <- true
            info.CreateNoWindow <- true
            info

        | Linux ->
            let info = new Diagnostics.ProcessStartInfo("run.sh")
            info.RedirectStandardError <- true
            info.RedirectStandardInput <- true
            info.RedirectStandardOutput <- true
            info.CreateNoWindow <- true
            info
    
    let mutable counter: int = 0
    let mutable serverProcess: option<Diagnostics.Process> = None

    member public this.ServerPath: string = _serverPath

    member public this.Start() =
        if counter >= 3 then
            failwith "Cannot start the server!!!!!!!!"
        else
            try
                serverProcess <- Some(Diagnostics.Process.Start(_processInfo))
                printfn $"Server started, process id {serverProcess.Value.Id}"
                counter <- counter + 1
                this.Protector()
            with :? ComponentModel.Win32Exception as e ->
                failwith $"Failed to start server: \n{e}"

    member private this.CheckState() =
        Option.map
            (fun (serverProcess: Diagnostics.Process) ->
                try
                    Diagnostics.Process.GetProcessById(serverProcess.Id) |> ignore
                    true
                with :? ArgumentException ->
                    false

            )
            serverProcess
        |> Option.get

    member private this.Protector() =
        while true do
            Threading.Thread.Sleep(10000)

            if not (this.CheckState()) then
                printfn "Server is not running, may be crashed, I will try to restart it..."
                this.Start()
            else
                ()
