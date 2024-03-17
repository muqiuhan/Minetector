module Minetector.Environment

open System

type OperatingSystem =
    | Windows
    | Linux

type Environment =
    static member public OS =
        if OperatingSystem.IsWindows() then Windows
        else if OperatingSystem.IsLinux() then Linux
        else failwith "Unsupported operating system"

