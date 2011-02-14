#light

module Strive.Client.ViewModel.Tests.Test

open System.Net
open System.Net.Sockets
open System.Windows.Media.Media3D
open System.Windows.Input
open NUnit.Framework
open FsUnit
open Strive.Network.Messaging
open Strive.Client.ViewModel


[<TestFixture>]
type ``my top level text``() = class

    let isKeyPressed x = x = Key.Up
    let wvm = new WorldViewModel(null)
    //let p = new PerspectiveViewModel(wvm, isKeyPressed)

    [<Test>]
    member self.``some text``() =
        2 |> should equal 2

    [<Test>]
    member self.``Can host``() =
        wvm.AddOrReplace("foo","bar",new Vector3D(), new Quaternion())

end