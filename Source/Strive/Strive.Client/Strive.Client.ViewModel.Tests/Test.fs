#light

module Strive.Client.ViewModel.Tests.Test

open System.Net
open System.Net.Sockets
open NUnit.Framework
open FsUnit
open Strive.Client.ViewModel
open Strive.Network.Messaging


[<TestFixture>]
type ``my top level text``() = class

    let wvm = new WorldViewModel(null)
    //let p = new Perspective()

    [<Test>]
    member self.``some text``() =
        2 |> should equal 2

    [<Test>]
    member self.``Can host``() =
        wvm.AddOrReplace("foo", 

end