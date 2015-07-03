using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Collections.Generic;

namespace DungeonGen
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.NumericUpDown LevelNumeric;
		private System.Windows.Forms.NumericUpDown RoomDensityNumeric;
		private System.Windows.Forms.NumericUpDown EntrancesNumeric;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown ExitsNumeric;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox SmallCheckBox;
		private System.Windows.Forms.CheckBox MediumCheckBox;
		private System.Windows.Forms.CheckBox LargeCheckBox;
		private System.Windows.Forms.CheckBox HugeCheckBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown MonsterDensityNumeric;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown SecretDoorNumeric;
		private System.Windows.Forms.NumericUpDown TrapDensityNumeric;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private IContainer components;
		private Panel panel1;


		private Dungeon		m_theDungeon = new Dungeon();
		private Label label12;
		private NumericUpDown dungeonSizeX;
		private NumericUpDown dungeonSizeY;
		private int m_picWidth = 0;
		private int m_picHeight = 0;
		private Bitmap m_picBitmap;
		private int m_picSelected = 0;
		private List<bool> m_editList = new List<bool>();
		private Graphics	m_formGraphics;

		public static Random sRand = new Random();
		private ComboBox comboBoxStyle;
		private MenuItem menuItem2;
		private ListBox listBoxRooms;
		private Button buttonExit;
		private Button buttonSave;
		private Button buttonAddRoom;
		private TextBox textBoxWeight;
		private Label labelWeight;
		private Label labelRoomName;
		private TextBox textBoxRoomName;
		private Panel panelSelect;
		private Panel panelEdit;
		private PictureBox pictureBoxCells;
		private NumericUpDown numericHallwaySize;
		
		private static string sDungeonConfig = "Config\\Dungeon.cfg";
		
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			panel1.Width = (int)dungeonSizeX.Value * iCell.CellSizeX;
			panel1.Height = (int)dungeonSizeY.Value * iCell.CellSizeY;
			m_formGraphics = panel1.CreateGraphics();

			ReadConfigFile();
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

		private void ReadConfigFile()
		{
			if (File.Exists(sDungeonConfig))
			{
				StreamReader sr = File.OpenText(sDungeonConfig);
				string data = sr.ReadLine();

				while (data != null)
				{
					string[] data_entry = data.Split(new Char[] { '=' }, 2);

					switch (data_entry[0])
					{
						case "Level":
							LevelNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "RoomDensity":
							RoomDensityNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "Entrances":
							EntrancesNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "Exits":
							ExitsNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "Style":
							comboBoxStyle.Text = data_entry[1];
						break;

						case "Small":
							SmallCheckBox.Checked = Convert.ToBoolean(data_entry[1]);
						break;

						case "Medium":
							MediumCheckBox.Checked = Convert.ToBoolean(data_entry[1]);
						break;

						case "Large":
							LargeCheckBox.Checked = Convert.ToBoolean(data_entry[1]);
						break;

						case "Huge":
							HugeCheckBox.Checked = Convert.ToBoolean(data_entry[1]);
						break;

						case "HallwaySize":
							numericHallwaySize.Value = Convert.ToInt16(data_entry[1]);
						break;

						case "MonsterDensity":
							MonsterDensityNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "SecretDensity":
							SecretDoorNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "TrapDensity":
							TrapDensityNumeric.Value = Convert.ToDecimal(data_entry[1]);
						break;

						case "DungeonSizeX":
							dungeonSizeX.Value = Convert.ToInt32(data_entry[1]);
						break;

						case "DungeonSizeY":
							dungeonSizeY.Value = Convert.ToInt32(data_entry[1]);
						break;
					}
					
					data = sr.ReadLine();
				}
				
				sr.Dispose();
			}
		}

		private void WriteConfigFile()
		{
			StreamWriter sr = File.CreateText(sDungeonConfig);

			if (sr != null)
			{
				string output;
				output = "Level=" + LevelNumeric.Value;
				sr.WriteLine(output);

				output = "RoomDensity=" + RoomDensityNumeric.Value;
				sr.WriteLine(output);
				
				output = "Entrances=" + EntrancesNumeric.Value;
				sr.WriteLine(output);

				output = "Exits=" + ExitsNumeric.Value;
				sr.WriteLine(output);
				
				output = "Style=" + comboBoxStyle.Text;
				sr.WriteLine(output);
				
				output = "Small=" + SmallCheckBox.Checked;
				sr.WriteLine(output);
				
				output = "Medium=" + MediumCheckBox.Checked;
				sr.WriteLine(output);
				
				output = "Large=" + LargeCheckBox.Checked;
				sr.WriteLine(output);

				output = "Huge=" + HugeCheckBox.Checked;
				sr.WriteLine(output);
				
				output = "HallwaySize=" + numericHallwaySize.Value;
				sr.WriteLine(output);
				
				output = "MonsterDensity=" + MonsterDensityNumeric.Value;
				sr.WriteLine(output);
				
				output = "SecretDensity= " + SecretDoorNumeric.Value;
				sr.WriteLine(output);

				output = "TrapDensity=" + TrapDensityNumeric.Value;
				sr.WriteLine(output);

				output = "DungeonSizeX=" + dungeonSizeX.Value;
				sr.WriteLine(output);

				output = "DungeonSizeY=" + dungeonSizeY.Value;
				sr.WriteLine(output);
				
				sr.Flush();
				sr.Dispose();
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.LevelNumeric = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.RoomDensityNumeric = new System.Windows.Forms.NumericUpDown();
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.EntrancesNumeric = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.ExitsNumeric = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SmallCheckBox = new System.Windows.Forms.CheckBox();
			this.MediumCheckBox = new System.Windows.Forms.CheckBox();
			this.LargeCheckBox = new System.Windows.Forms.CheckBox();
			this.HugeCheckBox = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.MonsterDensityNumeric = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.SecretDoorNumeric = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.TrapDensityNumeric = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelEdit = new System.Windows.Forms.Panel();
			this.textBoxRoomName = new System.Windows.Forms.TextBox();
			this.labelRoomName = new System.Windows.Forms.Label();
			this.textBoxWeight = new System.Windows.Forms.TextBox();
			this.labelWeight = new System.Windows.Forms.Label();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonAddRoom = new System.Windows.Forms.Button();
			this.panelSelect = new System.Windows.Forms.Panel();
			this.pictureBoxCells = new System.Windows.Forms.PictureBox();
			this.listBoxRooms = new System.Windows.Forms.ListBox();
			this.label12 = new System.Windows.Forms.Label();
			this.dungeonSizeX = new System.Windows.Forms.NumericUpDown();
			this.dungeonSizeY = new System.Windows.Forms.NumericUpDown();
			this.comboBoxStyle = new System.Windows.Forms.ComboBox();
			this.numericHallwaySize = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.LevelNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RoomDensityNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.EntrancesNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitsNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MonsterDensityNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SecretDoorNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TrapDensityNumeric)).BeginInit();
			this.panel1.SuspendLayout();
			this.panelEdit.SuspendLayout();
			this.panelSelect.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxCells)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dungeonSizeX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dungeonSizeY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHallwaySize)).BeginInit();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.treeView1.ForeColor = System.Drawing.Color.Green;
			this.treeView1.ItemHeight = 15;
			this.treeView1.LineColor = System.Drawing.Color.DimGray;
			this.treeView1.Location = new System.Drawing.Point(0, 351);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(208, 327);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Dungeon Level";
			// 
			// LevelNumeric
			// 
			this.LevelNumeric.Location = new System.Drawing.Point(152, 7);
			this.LevelNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.LevelNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LevelNumeric.Name = "LevelNumeric";
			this.LevelNumeric.Size = new System.Drawing.Size(56, 20);
			this.LevelNumeric.TabIndex = 3;
			this.LevelNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "Room Density";
			// 
			// RoomDensityNumeric
			// 
			this.RoomDensityNumeric.DecimalPlaces = 2;
			this.RoomDensityNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.RoomDensityNumeric.Location = new System.Drawing.Point(152, 32);
			this.RoomDensityNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.RoomDensityNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.RoomDensityNumeric.Name = "RoomDensityNumeric";
			this.RoomDensityNumeric.Size = new System.Drawing.Size(56, 20);
			this.RoomDensityNumeric.TabIndex = 5;
			this.RoomDensityNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            131072});
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Location = new System.Drawing.Point(40, 309);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(120, 23);
			this.buttonGenerate.TabIndex = 6;
			this.buttonGenerate.Text = "Generate Dungeon";
			this.buttonGenerate.Click += new System.EventHandler(this.GenerateDungeon_Click);
			// 
			// EntrancesNumeric
			// 
			this.EntrancesNumeric.Location = new System.Drawing.Point(152, 256);
			this.EntrancesNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.EntrancesNumeric.Name = "EntrancesNumeric";
			this.EntrancesNumeric.Size = new System.Drawing.Size(56, 20);
			this.EntrancesNumeric.TabIndex = 8;
			this.EntrancesNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 256);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Entrances";
			// 
			// ExitsNumeric
			// 
			this.ExitsNumeric.Location = new System.Drawing.Point(152, 280);
			this.ExitsNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.ExitsNumeric.Name = "ExitsNumeric";
			this.ExitsNumeric.Size = new System.Drawing.Size(56, 20);
			this.ExitsNumeric.TabIndex = 10;
			this.ExitsNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 280);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Exits";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0, 87);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 23);
			this.label6.TabIndex = 13;
			this.label6.Text = "Room Size";
			// 
			// SmallCheckBox
			// 
			this.SmallCheckBox.Checked = true;
			this.SmallCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SmallCheckBox.Location = new System.Drawing.Point(80, 87);
			this.SmallCheckBox.Name = "SmallCheckBox";
			this.SmallCheckBox.Size = new System.Drawing.Size(56, 16);
			this.SmallCheckBox.TabIndex = 14;
			this.SmallCheckBox.Text = "Small";
			// 
			// MediumCheckBox
			// 
			this.MediumCheckBox.Checked = true;
			this.MediumCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MediumCheckBox.Location = new System.Drawing.Point(144, 87);
			this.MediumCheckBox.Name = "MediumCheckBox";
			this.MediumCheckBox.Size = new System.Drawing.Size(64, 16);
			this.MediumCheckBox.TabIndex = 15;
			this.MediumCheckBox.Text = "Medium";
			// 
			// LargeCheckBox
			// 
			this.LargeCheckBox.Checked = true;
			this.LargeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.LargeCheckBox.Location = new System.Drawing.Point(80, 103);
			this.LargeCheckBox.Name = "LargeCheckBox";
			this.LargeCheckBox.Size = new System.Drawing.Size(56, 16);
			this.LargeCheckBox.TabIndex = 16;
			this.LargeCheckBox.Text = "Large";
			// 
			// HugeCheckBox
			// 
			this.HugeCheckBox.Location = new System.Drawing.Point(144, 103);
			this.HugeCheckBox.Name = "HugeCheckBox";
			this.HugeCheckBox.Size = new System.Drawing.Size(56, 16);
			this.HugeCheckBox.TabIndex = 17;
			this.HugeCheckBox.Text = "Huge";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(0, 232);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 23);
			this.label7.TabIndex = 19;
			this.label7.Text = "Style";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(0, 129);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(110, 23);
			this.label8.TabIndex = 20;
			this.label8.Text = "Target Hallway Size";
			// 
			// MonsterDensityNumeric
			// 
			this.MonsterDensityNumeric.DecimalPlaces = 2;
			this.MonsterDensityNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.MonsterDensityNumeric.Location = new System.Drawing.Point(152, 160);
			this.MonsterDensityNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MonsterDensityNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.MonsterDensityNumeric.Name = "MonsterDensityNumeric";
			this.MonsterDensityNumeric.Size = new System.Drawing.Size(56, 20);
			this.MonsterDensityNumeric.TabIndex = 26;
			this.MonsterDensityNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            131072});
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(0, 160);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 23);
			this.label9.TabIndex = 25;
			this.label9.Text = "Monster Density";
			// 
			// SecretDoorNumeric
			// 
			this.SecretDoorNumeric.DecimalPlaces = 2;
			this.SecretDoorNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.SecretDoorNumeric.Location = new System.Drawing.Point(152, 184);
			this.SecretDoorNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
			this.SecretDoorNumeric.Name = "SecretDoorNumeric";
			this.SecretDoorNumeric.Size = new System.Drawing.Size(56, 20);
			this.SecretDoorNumeric.TabIndex = 28;
			this.SecretDoorNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(0, 184);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 23);
			this.label10.TabIndex = 27;
			this.label10.Text = "Secret Door Density";
			// 
			// TrapDensityNumeric
			// 
			this.TrapDensityNumeric.DecimalPlaces = 2;
			this.TrapDensityNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.TrapDensityNumeric.Location = new System.Drawing.Point(152, 208);
			this.TrapDensityNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.TrapDensityNumeric.Name = "TrapDensityNumeric";
			this.TrapDensityNumeric.Size = new System.Drawing.Size(56, 20);
			this.TrapDensityNumeric.TabIndex = 30;
			this.TrapDensityNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            131072});
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(0, 208);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(100, 23);
			this.label11.TabIndex = 29;
			this.label11.Text = "Trap Density";
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.vScrollBar1.Location = new System.Drawing.Point(-17, 351);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 0);
			this.vScrollBar1.TabIndex = 31;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
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
			this.menuItem1.Text = "Save";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Edit Rooms";
			this.menuItem2.Click += new System.EventHandler(this.EditRoom_click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "jpg";
			this.saveFileDialog1.Filter = "JPG|*.jpg";
			this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.DarkGray;
			this.panel1.Controls.Add(this.panelEdit);
			this.panel1.Controls.Add(this.panelSelect);
			this.panel1.Controls.Add(this.vScrollBar1);
			this.panel1.Location = new System.Drawing.Point(208, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(942, 678);
			this.panel1.TabIndex = 0;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
			// 
			// panelEdit
			// 
			this.panelEdit.Controls.Add(this.textBoxRoomName);
			this.panelEdit.Controls.Add(this.labelRoomName);
			this.panelEdit.Controls.Add(this.textBoxWeight);
			this.panelEdit.Controls.Add(this.labelWeight);
			this.panelEdit.Controls.Add(this.buttonExit);
			this.panelEdit.Controls.Add(this.buttonSave);
			this.panelEdit.Controls.Add(this.buttonAddRoom);
			this.panelEdit.Location = new System.Drawing.Point(0, 366);
			this.panelEdit.Name = "panelEdit";
			this.panelEdit.Size = new System.Drawing.Size(284, 79);
			this.panelEdit.TabIndex = 45;
			this.panelEdit.Visible = false;
			// 
			// textBoxRoomName
			// 
			this.textBoxRoomName.Location = new System.Drawing.Point(10, 48);
			this.textBoxRoomName.MaxLength = 128;
			this.textBoxRoomName.Name = "textBoxRoomName";
			this.textBoxRoomName.Size = new System.Drawing.Size(160, 20);
			this.textBoxRoomName.TabIndex = 41;
			this.textBoxRoomName.Text = "Generic Room";
			this.textBoxRoomName.TextChanged += new System.EventHandler(this.textBoxRoomName_TextChanged);
			// 
			// labelRoomName
			// 
			this.labelRoomName.AutoSize = true;
			this.labelRoomName.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labelRoomName.Location = new System.Drawing.Point(11, 32);
			this.labelRoomName.Name = "labelRoomName";
			this.labelRoomName.Size = new System.Drawing.Size(66, 13);
			this.labelRoomName.TabIndex = 42;
			this.labelRoomName.Text = "Room Name";
			// 
			// textBoxWeight
			// 
			this.textBoxWeight.Location = new System.Drawing.Point(190, 48);
			this.textBoxWeight.Name = "textBoxWeight";
			this.textBoxWeight.Size = new System.Drawing.Size(81, 20);
			this.textBoxWeight.TabIndex = 40;
			this.textBoxWeight.Text = "10";
			this.textBoxWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxWeight.TextChanged += new System.EventHandler(this.textBoxWeight_TextChanged);
			// 
			// labelWeight
			// 
			this.labelWeight.AutoSize = true;
			this.labelWeight.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labelWeight.Location = new System.Drawing.Point(183, 32);
			this.labelWeight.Name = "labelWeight";
			this.labelWeight.Size = new System.Drawing.Size(88, 13);
			this.labelWeight.TabIndex = 39;
			this.labelWeight.Text = "Selection Weight";
			// 
			// buttonExit
			// 
			this.buttonExit.ForeColor = System.Drawing.Color.MidnightBlue;
			this.buttonExit.Location = new System.Drawing.Point(189, 3);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(82, 26);
			this.buttonExit.TabIndex = 36;
			this.buttonExit.Text = "Exit Edit";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.ExitRoom_click);
			// 
			// buttonSave
			// 
			this.buttonSave.ForeColor = System.Drawing.Color.MidnightBlue;
			this.buttonSave.Location = new System.Drawing.Point(98, 3);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(82, 26);
			this.buttonSave.TabIndex = 35;
			this.buttonSave.Text = "Save Rooms";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.SaveRoom_click);
			// 
			// buttonAddRoom
			// 
			this.buttonAddRoom.ForeColor = System.Drawing.Color.MidnightBlue;
			this.buttonAddRoom.Location = new System.Drawing.Point(10, 3);
			this.buttonAddRoom.Name = "buttonAddRoom";
			this.buttonAddRoom.Size = new System.Drawing.Size(82, 26);
			this.buttonAddRoom.TabIndex = 34;
			this.buttonAddRoom.Text = "Add Room";
			this.buttonAddRoom.UseVisualStyleBackColor = true;
			this.buttonAddRoom.Click += new System.EventHandler(this.AddRoom_click);
			// 
			// panelSelect
			// 
			this.panelSelect.Controls.Add(this.pictureBoxCells);
			this.panelSelect.Controls.Add(this.listBoxRooms);
			this.panelSelect.Location = new System.Drawing.Point(284, 3);
			this.panelSelect.Name = "panelSelect";
			this.panelSelect.Size = new System.Drawing.Size(297, 442);
			this.panelSelect.TabIndex = 44;
			this.panelSelect.Visible = false;
			// 
			// pictureBoxCells
			// 
			this.pictureBoxCells.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.pictureBoxCells.BackColor = System.Drawing.Color.White;
			this.pictureBoxCells.Location = new System.Drawing.Point(3, 0);
			this.pictureBoxCells.Name = "pictureBoxCells";
			this.pictureBoxCells.Size = new System.Drawing.Size(112, 436);
			this.pictureBoxCells.TabIndex = 44;
			this.pictureBoxCells.TabStop = false;
			this.pictureBoxCells.Click += new System.EventHandler(this.pictureBoxCells_Click);
			this.pictureBoxCells.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCells_MouseClick);
			// 
			// listBoxRooms
			// 
			this.listBoxRooms.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.listBoxRooms.FormattingEnabled = true;
			this.listBoxRooms.Location = new System.Drawing.Point(117, 3);
			this.listBoxRooms.Name = "listBoxRooms";
			this.listBoxRooms.ScrollAlwaysVisible = true;
			this.listBoxRooms.Size = new System.Drawing.Size(174, 433);
			this.listBoxRooms.TabIndex = 33;
			this.listBoxRooms.TabStop = false;
			this.listBoxRooms.UseTabStops = false;
			this.listBoxRooms.SelectedIndexChanged += new System.EventHandler(this.listBoxRooms_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(0, 60);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(74, 13);
			this.label12.TabIndex = 32;
			this.label12.Text = "Dungeon Size";
			// 
			// dungeonSizeX
			// 
			this.dungeonSizeX.Location = new System.Drawing.Point(88, 57);
			this.dungeonSizeX.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.dungeonSizeX.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.dungeonSizeX.Name = "dungeonSizeX";
			this.dungeonSizeX.Size = new System.Drawing.Size(58, 20);
			this.dungeonSizeX.TabIndex = 33;
			this.dungeonSizeX.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
			// 
			// dungeonSizeY
			// 
			this.dungeonSizeY.Location = new System.Drawing.Point(152, 58);
			this.dungeonSizeY.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.dungeonSizeY.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.dungeonSizeY.Name = "dungeonSizeY";
			this.dungeonSizeY.Size = new System.Drawing.Size(56, 20);
			this.dungeonSizeY.TabIndex = 34;
			this.dungeonSizeY.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
			// 
			// comboBoxStyle
			// 
			this.comboBoxStyle.DisplayMember = "1";
			this.comboBoxStyle.FormattingEnabled = true;
			this.comboBoxStyle.Items.AddRange(new object[] {
            "Cavern",
            "Community",
            "Dungeon",
            "Lair"});
			this.comboBoxStyle.Location = new System.Drawing.Point(87, 231);
			this.comboBoxStyle.Name = "comboBoxStyle";
			this.comboBoxStyle.Size = new System.Drawing.Size(121, 21);
			this.comboBoxStyle.TabIndex = 35;
			this.comboBoxStyle.Text = "Dungeon";
			this.comboBoxStyle.ValueMember = "1";
			this.comboBoxStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxStyle_SelectedIndexChanged);
			// 
			// numericHallwaySize
			// 
			this.numericHallwaySize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericHallwaySize.Location = new System.Drawing.Point(152, 130);
			this.numericHallwaySize.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numericHallwaySize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericHallwaySize.Name = "numericHallwaySize";
			this.numericHallwaySize.Size = new System.Drawing.Size(56, 20);
			this.numericHallwaySize.TabIndex = 36;
			this.numericHallwaySize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.SteelBlue;
			this.ClientSize = new System.Drawing.Size(1152, 671);
			this.Controls.Add(this.numericHallwaySize);
			this.Controls.Add(this.comboBoxStyle);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.dungeonSizeY);
			this.Controls.Add(this.dungeonSizeX);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.TrapDensityNumeric);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.SecretDoorNumeric);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.MonsterDensityNumeric);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.HugeCheckBox);
			this.Controls.Add(this.LargeCheckBox);
			this.Controls.Add(this.MediumCheckBox);
			this.Controls.Add(this.SmallCheckBox);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.ExitsNumeric);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.EntrancesNumeric);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.buttonGenerate);
			this.Controls.Add(this.RoomDensityNumeric);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.LevelNumeric);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.ForeColor = System.Drawing.Color.White;
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Dungeon Generator v0.1 for D&D 4.0";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.LevelNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RoomDensityNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.EntrancesNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitsNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MonsterDensityNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SecretDoorNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TrapDensityNumeric)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panelEdit.ResumeLayout(false);
			this.panelEdit.PerformLayout();
			this.panelSelect.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxCells)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dungeonSizeX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dungeonSizeY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHallwaySize)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				Application.Run(new Form1());
			}
			catch (ArgumentException e)
			{
				Console.WriteLine("Invalid Data " + e.Message, e);
			}
			catch (Exception e)
			{
				Console.WriteLine("Code Error " + e.Message, e);
			}
		}

		private void GenerateDungeon_Click(object sender, System.EventArgs e)
		{
			panel1.Width = (int)dungeonSizeX.Value * iCell.CellSizeX;
			panel1.Height = (int)dungeonSizeY.Value * iCell.CellSizeY;
			m_formGraphics = panel1.CreateGraphics();
			iCell temp = new iCell(new Point(panel1.Width, panel1.Height));
			
			GenerationParameters genParms = new GenerationParameters();
			genParms.m_floor = GetFloorType();
			Room.LoadRoomData(genParms.m_floor);
			genParms.m_CorridorSize = (int)numericHallwaySize.Value;
			genParms.m_bSmallRooms = SmallCheckBox.Checked;
			genParms.m_bMedRooms = MediumCheckBox.Checked;
			genParms.m_bLargeRooms = LargeCheckBox.Checked;
			genParms.m_bHugeRooms = HugeCheckBox.Checked;
			genParms.m_entrances = (int)EntrancesNumeric.Value;
			genParms.m_exits = (int)ExitsNumeric.Value;
			genParms.m_roomDensity.Set(RoomDensityNumeric.Value);
			genParms.m_secretDoorDensity.Set(SecretDoorNumeric.Value);
			genParms.m_trapDensity.Set(TrapDensityNumeric.Value);

			m_theDungeon.Generate(genParms, temp);
			
			SpawnParameters spawnParms = new SpawnParameters();
			spawnParms.m_level = (int)LevelNumeric.Value;
			spawnParms.m_monsterDensity.Set(MonsterDensityNumeric.Value);
			m_theDungeon.SpawnDungeon(spawnParms);
			
			treeView1.Nodes.Clear();
			
			for (int i = 0; i < m_theDungeon.GetRoomCount(); i++)
			{
				if (m_theDungeon.GetRoom(i).GetRoomIndex() != -1)
				{
					TreeNode leaf = new TreeNode(m_theDungeon.GetRoom(i).GetDungeonIndex() + " - " + m_theDungeon.GetRoom(i).GetName());
					leaf.Nodes.Add(new TreeNode("Description"));
					leaf.Nodes.Add(new TreeNode("Encounter"));
					leaf.Nodes.Add(new TreeNode("Treasure"));
					treeView1.Nodes.Add(leaf);
				}
			}
			
			panel1.Invalidate();
			
			WriteConfigFile();
		}
		
		private Floor GetFloorType()
		{
			if (comboBoxStyle.Text == "Community")
			{
				return Floor.Wood;
			}
			else if (comboBoxStyle.Text == "Cavern")
			{
				return Floor.Earth;
			}
			
			return Floor.Stone;
		}
			
		private void vScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Render(m_formGraphics);
		}

		private void Render(Graphics toGraphics)
		{
			SolidBrush myBrush;
			myBrush = new SolidBrush(panel1.BackColor);
			toGraphics.FillRectangle(myBrush, 0, 0, panel1.Width, panel1.Height);
			myBrush.Dispose();

			m_theDungeon.Render(toGraphics);
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Bitmap dungeonBitmap = new Bitmap(panel1.Width, panel1.Height);

			Graphics saveGraphics = Graphics.FromImage(dungeonBitmap);
			Render(saveGraphics);
			dungeonBitmap.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
			
			StreamWriter sr = File.CreateText(saveFileDialog1.FileName + ".txt");
			
			for (int i = 0; i < treeView1.Nodes.Count; i++)
			{
				sr.WriteLine(treeView1.Nodes[i].Text);

				for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
				{
					sr.WriteLine(treeView1.Nodes[i].Nodes[j].Text);

					for (int k = 0; k < treeView1.Nodes[i].Nodes[j].Nodes.Count; k++)
					{
						sr.WriteLine(treeView1.Nodes[i].Nodes[j].Nodes[k].Text);
					}
				}
			}
			
			sr.Dispose();

			Directory.SetCurrentDirectory("..\\");
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			Directory.SetCurrentDirectory("Saves");
			saveFileDialog1.ShowDialog();
		}

		private void panel1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int xSelected = e.X / iCell.CellSizeX;
				
				if (xSelected >= 0 && xSelected < 40)
				{
					int ySelected = e.Y / iCell.CellSizeY;
					
					if (ySelected >= 0 && ySelected < 40)
					{
						iCell selectedCell = new iCell(xSelected, ySelected);
						
						if (panelEdit.Visible == true)
						{
							m_theDungeon.Edit(selectedCell, m_picSelected == 0 ? -1 : m_picSelected);
							m_editList[m_theDungeon.GetEditRoom().GetRoomIndex()] = true;
						}
						else
						{
							m_theDungeon.Toggle(selectedCell);
						}
						
						panel1.Invalidate();
					}
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}

		private void EditRoom_click(object sender, EventArgs e)
		{
			// Already in edit mode, don't do anything
			if (buttonGenerate.Enabled == false)
			{
				return;
			}
		
			Room.LoadRoomData(GetFloorType());
			listBoxRooms.Items.Clear();
			m_editList.Clear();
			
			for (int i = 0; i < Room.GetRoomDataCount(); i++)
			{
				listBoxRooms.Items.Add(Room.GetRoomDataName(i));
				m_editList.Add(false);
			}
			
			buttonGenerate.Enabled = false;
			comboBoxStyle.Enabled = false;
			panelEdit.Visible = true;
			panelSelect.Visible = true;
									
			m_theDungeon.ClearDungeon();
			
			m_theDungeon.CreateArea(new iCell(0, 0), new iCell(40, 40));
			m_theDungeon.SetEditRoom(0);
			textBoxWeight.Text = m_theDungeon.GetEditRoom().GetWeight().ToString();
			textBoxRoomName.Text = m_theDungeon.GetEditRoom().GetName();
			
			m_picWidth = (pictureBoxCells.Width - 4) / iCell.CellSizeX;
			m_picHeight = Cell.sCellDataList.Count / m_picWidth + 1;
			
			if (m_picHeight * iCell.CellSizeY > pictureBoxCells.Height)
			{
				// Scroll bar time
			}
						
			BuildPicBitmap();
			
			panel1.Invalidate();
		}

		private void BuildPicBitmap()
		{
			m_picBitmap = new Bitmap(m_picWidth * iCell.CellSizeX + 4, m_picHeight * iCell.CellSizeY + 4);

			int drawY = 0;
			int drawX = 0;
			int curCell = 0;
			
			foreach (CellData cellData in Cell.sCellDataList)
			{
				for (int y = 0; y < cellData.m_bmpImage.Height; y++)
				{
					for (int x = 0; x < cellData.m_bmpImage.Width; x++)
					{
						m_picBitmap.SetPixel(2 + x + drawX * iCell.CellSizeX, 
											2 + y + drawY * iCell.CellSizeY,
											cellData.m_bmpImage.GetPixel(x, y));
					}
				}

				if (curCell == m_picSelected)
				{
					for (int y = 0; y < cellData.m_bmpImage.Height + 4; y++)
					{
						for (int x = 0; x < cellData.m_bmpImage.Width + 4; x++)
						{
							if (x <= 1
								|| x >= cellData.m_bmpImage.Width + 2
								|| y <= 1
								|| y >= cellData.m_bmpImage.Height + 2)
							{
								m_picBitmap.SetPixel(x + drawX * iCell.CellSizeX, 
													y + drawY * iCell.CellSizeY,
													Color.Red);
							}
						}
					}
				}
				
				drawX += 1;
				
				if (drawX == m_picWidth)
				{
					drawX = 0;
					drawY += 1;
				}
				
				curCell += 1;
			}

			pictureBoxCells.Image = m_picBitmap;
		}
		
		private void BuildRoomListBox()
		{
			int prevSelected = listBoxRooms.SelectedIndex;
			listBoxRooms.Items.Clear();
			
			for (int i = 0; i < Room.GetRoomDataCount(); i++)
			{
				string item;
				
				if (m_editList.Count > i && m_editList[i])
				{
					item = "* " + Room.GetRoomDataName(i);
				}
				else
				{
					item = Room.GetRoomDataName(i);
				}
				
				listBoxRooms.Items.Add(item);
			}
			
			listBoxRooms.SelectedIndex = prevSelected;
			listBoxRooms.Invalidate();
		}
		
		private void pictureBoxCells_Click(object sender, EventArgs e)
		{
		}

		private void pictureBoxCells_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int xSelected = (e.X - 2) / iCell.CellSizeX;
				
				if (xSelected >= 0 && xSelected < m_picWidth)
				{
					int ySelected = (e.Y - 2) / iCell.CellSizeY;
					
					if (ySelected >= 0 && ySelected < m_picHeight)
					{
						int picSelected = xSelected + ySelected * m_picWidth;
						
						if (picSelected >= 0 && picSelected < Cell.sCellDataList.Count)
						{
							m_picSelected = picSelected;
							BuildPicBitmap();
							
							pictureBoxCells.Invalidate();
						}
					}
				}
			}
		}

		private void AddRoom_click(object sender, EventArgs e)
		{
			int addIndex = Room.AddNewRoom();
			BuildRoomListBox();
			listBoxRooms.SelectedIndex = addIndex;
			listBoxRooms.Invalidate();
			m_editList.Add(true);
		}

		private void SaveRoom_click(object sender, EventArgs e)
		{
			m_theDungeon.SaveEditRoom();
			m_editList[m_theDungeon.GetEditRoom().GetRoomIndex()] = false;
			BuildRoomListBox();
					
			Room.SaveRoomData(GetFloorType());
		}

		private void ExitRoom_click(object sender, EventArgs e)
		{
			buttonGenerate.Enabled = true;
			comboBoxStyle.Enabled = true;
			panelSelect.Visible = false;
			panelEdit.Visible = false;
		}

		private void textBoxWeight_TextChanged(object sender, EventArgs e)
		{
			if (m_theDungeon.UpdateEditRoom(Convert.ToInt16(textBoxWeight.Text)))
			{
				m_editList[m_theDungeon.GetEditRoom().GetRoomIndex()] = true;
			}
		}

		private void textBoxRoomName_TextChanged(object sender, EventArgs e)
		{
			if (m_theDungeon.UpdateEditRoom(textBoxRoomName.Text))
			{
				m_editList[m_theDungeon.GetEditRoom().GetRoomIndex()] = true;
			}

			BuildRoomListBox();
		}

		private void listBoxRooms_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_theDungeon.SaveEditRoom();
			m_theDungeon.ClearDungeon();
			m_theDungeon.CreateArea(new iCell(0, 0), new iCell(40, 40));
			m_theDungeon.SetEditRoom(listBoxRooms.SelectedIndex);
			textBoxRoomName.Text = m_theDungeon.GetEditRoom().GetName();
			textBoxWeight.Text = m_theDungeon.GetEditRoom().GetWeight().ToString();
			panel1.Invalidate();
		}

		private void comboBoxStyle_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
