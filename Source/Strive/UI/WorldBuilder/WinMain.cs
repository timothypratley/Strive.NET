using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Strive.Multiverse;
using Strive.Data;
using Strive.Math3D;
using Strive.Logging;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Rendering.Controls;
using Strive.Resources;
using Strive.UI.WorldView;


namespace Strive.UI.WorldBuilder
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;

		static Schema multiverse;
		static IEngine engine;
		static World world;
		private System.Windows.Forms.MenuItem menuItem5;

		string currentFileName;
		private System.Windows.Forms.MenuItem menuItem6;
		string currentConnectionString;


		public WinMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			// The multiverse is everywhere, it is all around us
			multiverse = new Schema();

			// the 3d rendering engine
			engine = Strive.Rendering.Activator.GetEngine();

			// the worldview
			world = new World( engine );

			ResourceManager.SetPath( "c:/strive/resources" );
			ResourceManager.factory = engine;

			mouse = engine.Mouse;
			keyboard = engine.Keyboard;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new WinMain());
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(16, 64);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 368);
			this.panel1.TabIndex = 2;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 439);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(528, 22);
			this.statusBar1.TabIndex = 3;
			this.statusBar1.Text = "statusBar1";
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton1,
																						this.toolBarButton2});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(528, 39);
			this.toolBar1.TabIndex = 4;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Text = "Raise";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Text = "Lower";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem3,
																					  this.menuItem5,
																					  this.menuItem4,
																					  this.menuItem6});
			this.menuItem1.Text = "File";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.Text = "Open From File";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "Save To File";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "View";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "Open From Database";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 3;
			this.menuItem6.Text = "Save To Database";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 461);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.toolBar1,
																		  this.statusBar1,
																		  this.panel1});
			this.Menu = this.mainMenu1;
			this.Name = "WinMain";
			this.Text = "Main";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WinMain_Closing);
			this.Load += new System.EventHandler(this.Main_Load);
			this.ResumeLayout(false);

		}
		#endregion

		Strive.Common.StoppableThread st;
		private void Main_Load(object sender, System.EventArgs e) {
			world.InitialiseView( panel1 );
			st = new Strive.Common.StoppableThread( new Strive.Common.StoppableThread.WhileRunning( InputHandler ) );
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			//world.Render();
		}

		private void menuItem3_Click(object sender, System.EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = "c:/" ;
			openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;

			if(openFileDialog1.ShowDialog( this ) == DialogResult.OK) {
				st.Stop();
				world.RemoveAll();
				currentFileName = openFileDialog1.FileName;
				multiverse = MultiverseFactory.getMultiverseFromFile( currentFileName );
				renderMultiverse();
				world.CameraPosition = new Vector3D( 0, 100, 0 );
				world.CameraRotation = new Vector3D( 0, 90, 0 );
				st.Start();
			}
		}

		private void renderMultiverse() {
			Schema.WorldRow wr = multiverse.World.FindByWorldID( 1 );
			if ( wr == null ) {
				throw new Exception( "ERROR: World ID not valid!" );	
			}
			
			Log.LogMessage( "Loading world \"" + wr.WorldName + "\"..." );
			foreach ( Schema.AreaRow ar in wr.GetAreaRows() ) {
				Log.LogMessage( "Loading area \"" + ar.AreaName + "\"..." );
				// don't load area 0, its players and their eq
				if ( ar.AreaID == 0 ) continue;
				foreach ( Schema.ObjectTemplateRow otr in ar.GetObjectTemplateRows() ) {
					// nb: add terrain pieces first,
					// we rely on these being the first thing sent to the client.
					// todo: make groundlevel lookup serverside...
					// thus client need not have the terrain piece before the
					// object on top of it.
					foreach ( Schema.TemplateTerrainRow ttr in otr.GetTemplateTerrainRows() ) {
						foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
							Terrain t = new Terrain( ttr, otr, oir );
							world.Add( t );
						}
					}
					foreach ( Schema.TemplateMobileRow tmr in otr.GetTemplateMobileRows() ) {
						foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
							// NB: we only add avatars to our world, not mobiles
							Mobile m = new Mobile( tmr, otr, oir );
							world.Add( m );
						}
					}
					foreach ( Schema.TemplateItemRow tir in otr.GetTemplateItemRows() ) {
						foreach ( Schema.ItemEquipableRow ier in tir.GetItemEquipableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Equipable e = new Equipable( ier, tir, otr, oir );
								world.Add( e );
							}
						}
						foreach ( Schema.ItemJunkRow ijr in tir.GetItemJunkRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Junk j = new Junk( ijr, tir, otr, oir );
								world.Add( j );
							}
						}
						foreach ( Schema.ItemQuaffableRow iqr in tir.GetItemQuaffableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Quaffable q = new Quaffable( iqr, tir, otr, oir );
								world.Add( q );
							}
						}
						foreach ( Schema.ItemReadableRow irr in tir.GetItemReadableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Readable r = new Readable( irr, tir, otr, oir );
								world.Add( r );
							}
						}
						foreach ( Schema.ItemWieldableRow iwr in tir.GetItemWieldableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Wieldable w = new Wieldable( iwr, tir, otr, oir );
								world.Add( w );
							}
						}
					}
				}
			}
			Log.LogMessage( "Loaded world." );
		}

		float pitch = 0;
		Vector3D avatarPosition = new Vector3D( 0, 100, 0 );
		Vector3D avatarRotation = new Vector3D( 0, 0, 0 );
		IMouse mouse;
		IKeyboard keyboard;
		void ProcessMouseInput() {
			#region ProcessMouseInput

			bool WasMouseInput = false;

			mouse.GetAbsState();

			if ( mouse.Button1down ) {
				IModel m = world.RenderingScene.MousePick( mouse.X, mouse.Y );
				if ( m != null ) {
					if ( m is ITerrain ) {
						// TODO: hmk it seems a bit homosexual
						// to have to loop through like this, prolly there is a better
						// way?
						foreach ( TerrainPiece tp in world.TerrainPieces.terrainPieces.Values ) {
							if ( tp.model == m ) {
								// readd it with a different altitude
								// multiple adds are ok
								tp.altitude += 2;
								Schema.ObjectInstanceRow oir = multiverse.ObjectInstance.FindByObjectInstanceID( tp.physicalObject.ObjectInstanceID );
								oir.Y = tp.altitude;
								world.TerrainPieces.Add( tp );
								break;
							}
						}
					} else {
						MessageBox.Show( this, m.Name ); 
					}
				}
			}

			if ( false ) {
				if( mouse.X != 0 ) {
					WasMouseInput = true;
					avatarRotation.Y += mouse.X*0.2f; 
					avatarRotation.X = pitch;
				}
				if( mouse.Y != 0 ) {
					WasMouseInput = true;
					pitch += mouse.Y*0.2f;
					if ( pitch > 60 ) { pitch = 60; }
					if ( pitch < -60 ) { pitch = -60; }
					avatarRotation.X = pitch;
				}
			}

			// todo: only send once for mouse or keyboard input
			if(WasMouseInput) {
				world.CameraRotation = avatarRotation;
			}
#endregion
		}

		void ProcessKeyboardInput() {
			#region 2.0 Get keyboard input 
			bool WasKeyboardInput = false;
			const int moveunit = 5;
			if(keyboard.GetKeyState(Key.key_W)) {
				WasKeyboardInput = true;
				avatarPosition.X +=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit*2;
				avatarPosition.Z +=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit*2;
			}

			if(keyboard.GetKeyState(Key.key_S)) {
				WasKeyboardInput = true;
				avatarPosition.X -=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
				avatarPosition.Z -=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
			}

			if(keyboard.GetKeyState(Key.key_D)) {
				WasKeyboardInput = true;
				avatarPosition.X +=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				avatarPosition.Z -=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(keyboard.GetKeyState(Key.key_A)) {
				WasKeyboardInput = true;
				avatarPosition.X -=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				avatarPosition.Z += 
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(keyboard.GetKeyState(Key.key_Q)) {
				WasKeyboardInput = true;
				avatarRotation.Y -= 5.0F; 
			}

			if(keyboard.GetKeyState(Key.key_E)) {
				WasKeyboardInput = true;
				avatarRotation.Y += 5.0F;
			}

			if(WasKeyboardInput) {
				// check that we can go there
				/** TODO: enable collision detection
				foreach ( PhysicalObjectInstance poi in Game.CurrentWorld.physicalObjectInstances.Values ) {
					if ( poi.model.ModelFormat == ModelFormat.Scape ) {
						continue;
					}
					if ( poi == Game.CurrentWorld.CurrentAvatar ) {
						continue;
					}
					//Log.LogMessage(	"CD2 model " + m.Key + " at " + m.Position + " from " + avatarPosition );
					//todo  convert to 3d when centers is sorted
					float dx1 = Game.CurrentWorld.CurrentAvatar.model.Position.X - poi.model.Position.X;
					float dz1 = Game.CurrentWorld.CurrentAvatar.model.Position.Z - poi.model.Position.Z;
					//float dy1 = _scene.View.Position.Y - m.Position.Y;
					float distance_squared1 = dx1*dx1 + dz1*dz1;// + dy1*dy1;
					if ( distance_squared1 < poi.model.BoundingSphereRadiusSquared + 100 ) {
						// already a collision, ignore collision detection
						continue;
					}
					float dx = avatarPosition.X - poi.model.Position.X;
					float dz = avatarPosition.Z - poi.model.Position.Z;
					//float dy = avatarPosition.Y - m.Position.Y;
					float distance_squared = dx*dx + dz*dz;// + dy*dy;
					// assumes my radius is root 100
					if ( distance_squared < poi.model.BoundingSphereRadiusSquared + 100 ) {
						Log.LogMessage( "Canceled move due to collision" );
						return;
					}
				} */
				world.CameraPosition = avatarPosition;
				world.CameraRotation = avatarRotation;
			}

				#endregion
		}

		void InputHandler() {
			ProcessMouseInput();
			ProcessKeyboardInput();
			world.Render();
			System.Threading.Thread.Sleep( 10 );
		}

		private void WinMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			st.Stop();
		}

		private void menuItem4_Click(object sender, System.EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = "c:/" ;
			openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;

			if(openFileDialog1.ShowDialog( this ) == DialogResult.OK) {
				currentFileName = openFileDialog1.FileName;
				MultiverseFactory.persistMultiverseToFile( multiverse, currentFileName );
			}
		}

		private void menuItem5_Click(object sender, System.EventArgs e) {
			OpenFromDatabase ofd = new OpenFromDatabase();
			if ( ofd.ShowDialog( this ) == DialogResult.OK ) {
				st.Stop();
				world.RemoveAll();
				currentConnectionString = ofd.connectionString;
				multiverse = MultiverseFactory.getMultiverseFromDatabase( currentConnectionString );
				renderMultiverse();
				world.CameraPosition = new Vector3D( 0, 100, 0 );
				world.CameraRotation = new Vector3D( 0, 90, 0 );
				st.Start();
			}
		}

		private void menuItem6_Click(object sender, System.EventArgs e) {
			MultiverseFactory.persistMultiverseToDatabase( multiverse ); 
		}
	}
}
