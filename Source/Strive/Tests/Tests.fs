#light

module Tests
open System.Net
open System.Net.Sockets
open NUnit.Framework
open FsUnit
open Strive.Network.Messages
open Strive.Network.Messaging


[<TestFixture>]
type ``my top level text``() = class

    let server = new Listener(new IPEndPoint(IPAddress.Any, 8888))

    [<Test>]
    member self.``some text``() =
        2 |> should equal 2

    [<Test>]
    member self.``Can host``() =
        server.Start()

end
