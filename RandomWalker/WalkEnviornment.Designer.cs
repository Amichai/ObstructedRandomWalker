using RandomWalker.Controls;
using System.Drawing;
namespace RandomWalker {
	partial class WalkEnviornment {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent(Size mapSize) {
			this.components = new System.ComponentModel.Container();
			this.label = new System.Windows.Forms.Label();
			this.mapDisplay = new EnvironmentDisplay();
			this.Walk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.BackColor = System.Drawing.Color.White;
			this.label.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.label.Location = new System.Drawing.Point(0, 642);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(35, 13);
			this.label.TabIndex = 1;
			this.label.Text = "label1";
			//
			// mapDisplay
			// 
			this.mapDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapDisplay.Location = new System.Drawing.Point(0, 0);
			this.mapDisplay.Name = "mapDisplay";
			this.mapDisplay.Size = mapSize;
			this.mapDisplay.TabIndex = 3;
			this.mapDisplay.Text = "roboMap1";
			// 
			// Walk
			// 
			this.Walk.Location = new System.Drawing.Point(744, 13);
			this.Walk.Name = "Walk";
			this.Walk.Size = new System.Drawing.Size(75, 23);
			this.Walk.TabIndex = 4;
			this.Walk.Text = "Walk";
			this.Walk.UseVisualStyleBackColor = true;
			this.Walk.Click += new System.EventHandler(this.Walk_Click);
			// 
			// WalkEnviornment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = mapSize;
			this.Controls.Add(this.Walk);
			this.Controls.Add(this.label);
			this.Controls.Add(this.mapDisplay);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "WalkEnviornment";
			this.Text = "FakeRobotHost";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label;
		private Controls.EnvironmentDisplay mapDisplay;
		private System.Windows.Forms.Button Walk;
	}
}