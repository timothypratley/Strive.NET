using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DataHarness
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView MultiverseTree;
		private System.Windows.Forms.Button RefreshTree;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WinMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.RefreshTree = new System.Windows.Forms.Button();
			this.MultiverseTree = new System.Windows.Forms.TreeView();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// RefreshTree
			// 
			this.RefreshTree.Location = new System.Drawing.Point(8, 8);
			this.RefreshTree.Name = "RefreshTree";
			this.RefreshTree.TabIndex = 0;
			this.RefreshTree.Text = "Refresh";
			this.RefreshTree.Click += new System.EventHandler(this.Refresh_Click);
			// 
			// MultiverseTree
			// 
			this.MultiverseTree.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.MultiverseTree.ImageIndex = -1;
			this.MultiverseTree.Location = new System.Drawing.Point(8, 40);
			this.MultiverseTree.Name = "MultiverseTree";
			this.MultiverseTree.SelectedImageIndex = -1;
			this.MultiverseTree.Size = new System.Drawing.Size(448, 656);
			this.MultiverseTree.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(88, 8);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 702);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1,
																		  this.MultiverseTree,
																		  this.RefreshTree});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "WinMain";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new WinMain());
		}

		private void Refresh_Click(object sender, System.EventArgs e)
		{
			

			// 0.1 clear the tree
			MultiverseTree.Nodes.Clear();

			// 1.0 Get a connection
			SqlConnection con = new SqlConnection("Data Source=demeter\\VSdotNET;Initial Catalog=strive;Integrated Security=true");
			con.Open();

			// 2.0 Instantiate a multiverse
			Multiverse tehMultiverse = new Multiverse();

			// 3.0 Instantiate the multiverse factory - this loads the multiverse
			MultiverseFactory tehFactory = new MultiverseFactory(tehMultiverse, con);


			// Populate the tree:

			foreach(Multiverse.PlayerRow enumPlayer in tehMultiverse.Player.Rows)
			{
				TreeNode playerNode = new TreeNode(enumPlayer.UserName + ": " + enumPlayer.Email);

				foreach(Multiverse.MobilePossesableByPlayerRow enumPossesable in enumPlayer.GetMobilePossesableByPlayerRows())
				{
					TreeNode mobileNode = new TreeNode(enumPossesable.MobileRow.PhysicalObjectName);

					playerNode.Nodes.Add(mobileNode);

				}

				MultiverseTree.Nodes.Add(playerNode);

			}

			foreach(Multiverse.WorldRow enumWorld in tehMultiverse.World.Rows)
			{
				TreeNode worldNode = new TreeNode(enumWorld.WorldName + ": " + enumWorld.Description);

				foreach(Multiverse.AreaRow enumArea in enumWorld.GetAreaRows())
				{
					TreeNode areaNode = new TreeNode(enumArea.AreaName);

					TreeNode areaTerrainNode = new TreeNode("Terrain:");

					foreach(Multiverse.TerrainRow enumTerrain in enumArea.GetTerrainRows())
					{
						TreeNode terrainNode = new TreeNode(enumTerrain.X + ":" + enumTerrain.Y + ":" + enumTerrain.Z);

						areaTerrainNode.Nodes.Add(terrainNode);
					}

					areaNode.Nodes.Add(areaTerrainNode);

					
					TreeNode areaMobileNode = new TreeNode("Mobile:");

					foreach(Multiverse.MobileRow enumMobile in enumArea.GetMobileRows())
					{
						TreeNode mobileNode = new TreeNode(enumMobile.PhysicalObjectName);

						TreeNode mobileInventoryNode = new TreeNode("Inventory");

						TreeNode mobileInventoryWieldableNode = new TreeNode("Wieldable");

						mobileInventoryNode.Nodes.Add(mobileInventoryWieldableNode);

						foreach(Multiverse.InventoryRow enumInventory in enumMobile.GetInventoryRows())
						{
							if(enumInventory.WieldableRow != null)
							{
								mobileInventoryWieldableNode.Nodes.Add(enumInventory.WieldableRow.PhysicalObjectName);
							}

						}

						mobileNode.Nodes.Add(mobileInventoryNode);

						areaMobileNode.Nodes.Add(mobileNode);

					}

					areaNode.Nodes.Add(areaMobileNode);

					worldNode.Nodes.Add(areaNode);
				}

				MultiverseTree.Nodes.Add(worldNode);
			}


			// close the connection
			con.Close();


		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if(System.IO.File.Exists("output.xml")) { System.IO.File.Delete("output.xml"); }
			System.IO.StreamWriter sw = System.IO.File.CreateText("output.xml");

			// 1.0 Get a connection
			SqlConnection con = new SqlConnection("Data Source=demeter\\VSdotNET;Initial Catalog=strive;Integrated Security=true");
			con.Open();

			// 2.0 Instantiate a multiverse
			Multiverse tehMultiverse = new Multiverse();

			// 3.0 Instantiate the multiverse factory - this loads the multiverse
			MultiverseFactory tehFactory = new MultiverseFactory(tehMultiverse, con);

			sw.Write(tehMultiverse.GetXml());

			sw.Close();
		}
	}
}
