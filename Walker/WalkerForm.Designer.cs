using System.Drawing;
using System.ComponentModel;
namespace Walker {
	partial class WalkerForm {
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
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "Walker";
			this.mapDisplay = new MapDisplay();
			this.Walk = new System.Windows.Forms.Button();
			this.Reset = new System.Windows.Forms.Button();
			this.Print = new System.Windows.Forms.Button();
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
			// Reset
			// 
			this.Reset.Location = new System.Drawing.Point(825, 13);
			this.Reset.Name = "Reset";
			this.Reset.Size = new System.Drawing.Size(75, 23);
			this.Reset.TabIndex = 5;
			this.Reset.Text = "Reset";
			this.Reset.UseVisualStyleBackColor = true;
			this.Reset.Click += new System.EventHandler(this.Reset_Click);
			//
			// Print
			//
			this.Print.Location = new System.Drawing.Point(825, 40);
			this.Print.Name = "Print";
			this.Print.Size = new System.Drawing.Size(75, 23);
			this.Print.TabIndex = 6;
			this.Print.Text = "Print";
			this.Print.UseVisualStyleBackColor = true;
			this.Print.Click += new System.EventHandler(this.Print_Click);

			// 
			// WalkEnviornment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = mapSize;
			this.Controls.Add(this.Walk);
			this.Controls.Add(this.Reset);
			this.Controls.Add(this.Print);
			this.Controls.Add(this.mapDisplay);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "WalkEnviornment";
			this.Text = "RandomWalker";
			this.ResumeLayout(false);
			this.PerformLayout();
		}





		#endregion

		private MapDisplay mapDisplay;
		private System.Windows.Forms.Button Walk;
		private System.Windows.Forms.Button Reset;
		private System.Windows.Forms.Button Print;
	}
}

