using System;
using System.Net;
using System.Collections;
using System.Configuration;
using Strive.Network.Client.NetworkHandler;
using Strive.Network.Server.Messages;

namespace Strive.Network.Client {
	/// <summary>
	/// </summary>
	class TehClien7 {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main( string[] args ) {
			// Retreive default server:port
			if ( ConfigurationSettings.AppSettings["server"] == null ) {
				throw new ConfigurationException( "server" );
			}
			if ( ConfigurationSettings.AppSettings["port"] == null ) {
				throw new ConfigurationException( "port" );
			}
			int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
			string server = ConfigurationSettings.AppSettings["server"];

			// Incomming packets from the server are queued as they arrive
			Queue packetQueue = new Queue();

			// The world consists of PhysicalObjects which get rendered
			// by a viewer
			Strive.Network.Client.World world = new World();

			// Client handler listens for messages and puts them onto
			// packetQueue
			ServerConnection sc = new ServerConnection( packetQueue );
			sc.Start( new IPEndPoint( Dns.GetHostByName( server ).AddressList[0], port ) );
		
			// MessageProcessor takes packets off the Queue,
			// and interprets messages, updating the world
			MessageProcessor mp = new MessageProcessor(
				world, packetQueue
			);

			// WorldViewer is what the user sees,
			// and notifies the server if the user moves etc
			WorldViewer wv = new WorldViewer( world, sc );
			wv.Start();

			// login and possess the dantra physcial object
			sc.Send(
				new Strive.Network.Server.Messages.ToServer.Login( "dantra", "dantra" )
			);

			System.Random r = new System.Random();
			sc.Send(
				new Strive.Network.Server.Messages.ToServer.Possess( r.Next(10) )
			);

			// create the local avatar
			PhysicalObject po = new PhysicalObject();
			world.Add( po );

			wv.Avatar = po;
	
			// listen to socket, render, write back to server
			while ( sc.IsRunning ) {
				mp.ProcessOustandingMessages();
				wv.Handle();
				System.Thread.Sleep( 3000 );
			}
		}
	}
}
