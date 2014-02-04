namespace ScanProgram
{
    partial class FrmScanProgram
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmScanProgram));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectScanningDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getImageFromDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eidtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertColoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greyscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setGreyscaleThresholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allBlackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl2Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl3Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl4Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl5Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl6Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl7Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl8Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lvl9Toolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.allWhiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actuallSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitToPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodeMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decodeMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.eidtToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(742, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectScanningDeviceToolStripMenuItem,
            this.getImageFromDeviceToolStripMenuItem,
            this.toolStripSeparator6,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem1,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.pageSetupToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // selectScanningDeviceToolStripMenuItem
            // 
            this.selectScanningDeviceToolStripMenuItem.Name = "selectScanningDeviceToolStripMenuItem";
            this.selectScanningDeviceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.selectScanningDeviceToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.selectScanningDeviceToolStripMenuItem.Text = "Select Scanning Device";
            this.selectScanningDeviceToolStripMenuItem.Click += new System.EventHandler(this.FindDevicesToolStripMenuItemClick);
            // 
            // getImageFromDeviceToolStripMenuItem
            // 
            this.getImageFromDeviceToolStripMenuItem.Name = "getImageFromDeviceToolStripMenuItem";
            this.getImageFromDeviceToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.getImageFromDeviceToolStripMenuItem.Text = "Scan Image From Device";
            this.getImageFromDeviceToolStripMenuItem.Click += new System.EventHandler(this.GetImageFromDeviceToolStripMenuItemClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(232, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripItemClick);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(232, 6);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
            this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(235, 22);
            this.saveToolStripMenuItem1.Text = "&Save As";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.SaveToolStripMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintDocPrintPage);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.FilePrintPreviewMenuItemClick);
            // 
            // pageSetupToolStripMenuItem
            // 
            this.pageSetupToolStripMenuItem.Name = "pageSetupToolStripMenuItem";
            this.pageSetupToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.pageSetupToolStripMenuItem.Text = "Page Setup";
            this.pageSetupToolStripMenuItem.Click += new System.EventHandler(this.FilePageSetupMenuItemClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripItemClick);
            // 
            // eidtToolStripMenuItem
            // 
            this.eidtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invertColoursToolStripMenuItem,
            this.greyscaleToolStripMenuItem,
            this.setGreyscaleThresholdToolStripMenuItem,
            this.scanningToolStripMenuItem,
            this.resetImageToolStripMenuItem,
            this.encodeMessageToolStripMenuItem,
            this.decodeMessageToolStripMenuItem});
            this.eidtToolStripMenuItem.Name = "eidtToolStripMenuItem";
            this.eidtToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.eidtToolStripMenuItem.Text = "Edit";
            // 
            // invertColoursToolStripMenuItem
            // 
            this.invertColoursToolStripMenuItem.Name = "invertColoursToolStripMenuItem";
            this.invertColoursToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.invertColoursToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.invertColoursToolStripMenuItem.Text = "Invert Colours";
            this.invertColoursToolStripMenuItem.Click += new System.EventHandler(this.InvertColoursToolStripMenuItemClick);
            // 
            // greyscaleToolStripMenuItem
            // 
            this.greyscaleToolStripMenuItem.Name = "greyscaleToolStripMenuItem";
            this.greyscaleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.greyscaleToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.greyscaleToolStripMenuItem.Text = "Greyscale";
            this.greyscaleToolStripMenuItem.Click += new System.EventHandler(this.GreyscaleToolStripMenuItemClick);
            // 
            // setGreyscaleThresholdToolStripMenuItem
            // 
            this.setGreyscaleThresholdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allBlackToolStripMenuItem,
            this.lvl2Toolstrip,
            this.lvl3Toolstrip,
            this.lvl4Toolstrip,
            this.lvl5Toolstrip,
            this.lvl6Toolstrip,
            this.lvl7Toolstrip,
            this.lvl8Toolstrip,
            this.lvl9Toolstrip,
            this.allWhiteToolStripMenuItem});
            this.setGreyscaleThresholdToolStripMenuItem.Name = "setGreyscaleThresholdToolStripMenuItem";
            this.setGreyscaleThresholdToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.setGreyscaleThresholdToolStripMenuItem.Text = "Set Greyscale Threshold...";
            // 
            // allBlackToolStripMenuItem
            // 
            this.allBlackToolStripMenuItem.Name = "allBlackToolStripMenuItem";
            this.allBlackToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.allBlackToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.allBlackToolStripMenuItem.Text = "1 (dark)";
            this.allBlackToolStripMenuItem.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl2Toolstrip
            // 
            this.lvl2Toolstrip.Name = "lvl2Toolstrip";
            this.lvl2Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.lvl2Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl2Toolstrip.Text = "2";
            this.lvl2Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl3Toolstrip
            // 
            this.lvl3Toolstrip.Name = "lvl3Toolstrip";
            this.lvl3Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.lvl3Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl3Toolstrip.Text = "3";
            this.lvl3Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl4Toolstrip
            // 
            this.lvl4Toolstrip.Name = "lvl4Toolstrip";
            this.lvl4Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.lvl4Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl4Toolstrip.Text = "4";
            this.lvl4Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl5Toolstrip
            // 
            this.lvl5Toolstrip.Name = "lvl5Toolstrip";
            this.lvl5Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
            this.lvl5Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl5Toolstrip.Text = "5 (normal)";
            this.lvl5Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl6Toolstrip
            // 
            this.lvl6Toolstrip.Name = "lvl6Toolstrip";
            this.lvl6Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
            this.lvl6Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl6Toolstrip.Text = "6";
            this.lvl6Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl7Toolstrip
            // 
            this.lvl7Toolstrip.Name = "lvl7Toolstrip";
            this.lvl7Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D7)));
            this.lvl7Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl7Toolstrip.Text = "7";
            this.lvl7Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl8Toolstrip
            // 
            this.lvl8Toolstrip.Name = "lvl8Toolstrip";
            this.lvl8Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
            this.lvl8Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl8Toolstrip.Text = "8";
            this.lvl8Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // lvl9Toolstrip
            // 
            this.lvl9Toolstrip.Name = "lvl9Toolstrip";
            this.lvl9Toolstrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
            this.lvl9Toolstrip.Size = new System.Drawing.Size(169, 22);
            this.lvl9Toolstrip.Text = "9";
            this.lvl9Toolstrip.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // allWhiteToolStripMenuItem
            // 
            this.allWhiteToolStripMenuItem.Name = "allWhiteToolStripMenuItem";
            this.allWhiteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
            this.allWhiteToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.allWhiteToolStripMenuItem.Text = "10 (light)";
            this.allWhiteToolStripMenuItem.Click += new System.EventHandler(this.SetGrayThreshold);
            // 
            // scanningToolStripMenuItem
            // 
            this.scanningToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actuallSizeToolStripMenuItem,
            this.fitToPageToolStripMenuItem,
            this.customSizeToolStripMenuItem});
            this.scanningToolStripMenuItem.Name = "scanningToolStripMenuItem";
            this.scanningToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.scanningToolStripMenuItem.Text = "Scaling...";
            // 
            // actuallSizeToolStripMenuItem
            // 
            this.actuallSizeToolStripMenuItem.Name = "actuallSizeToolStripMenuItem";
            this.actuallSizeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.actuallSizeToolStripMenuItem.Text = "Actual Size";
            this.actuallSizeToolStripMenuItem.Click += new System.EventHandler(this.ScalingPage);
            // 
            // fitToPageToolStripMenuItem
            // 
            this.fitToPageToolStripMenuItem.Name = "fitToPageToolStripMenuItem";
            this.fitToPageToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.fitToPageToolStripMenuItem.Text = "Fit to Sheet";
            this.fitToPageToolStripMenuItem.Click += new System.EventHandler(this.ScalingPage);
            // 
            // customSizeToolStripMenuItem
            // 
            this.customSizeToolStripMenuItem.Name = "customSizeToolStripMenuItem";
            this.customSizeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.customSizeToolStripMenuItem.Text = "Custom Size";
            this.customSizeToolStripMenuItem.Click += new System.EventHandler(this.ScalingPage);
            // 
            // resetImageToolStripMenuItem
            // 
            this.resetImageToolStripMenuItem.Name = "resetImageToolStripMenuItem";
            this.resetImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetImageToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.resetImageToolStripMenuItem.Text = "Reset Image";
            this.resetImageToolStripMenuItem.Click += new System.EventHandler(this.ResetImageToolStripMenuItemClick);
            // 
            // encodeMessageToolStripMenuItem
            // 
            this.encodeMessageToolStripMenuItem.Name = "encodeMessageToolStripMenuItem";
            this.encodeMessageToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.encodeMessageToolStripMenuItem.Text = "Encode Message";
            this.encodeMessageToolStripMenuItem.Click += new System.EventHandler(this.EncodeMessageToolStripMenuItemClick);
            // 
            // decodeMessageToolStripMenuItem
            // 
            this.decodeMessageToolStripMenuItem.Name = "decodeMessageToolStripMenuItem";
            this.decodeMessageToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.decodeMessageToolStripMenuItem.Text = "Decode Message";
            this.decodeMessageToolStripMenuItem.Click += new System.EventHandler(this.DecodeMessageToolStripMenuItemClick);
            // 
            // picImage
            // 
            this.picImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImage.Location = new System.Drawing.Point(0, 24);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(742, 576);
            this.picImage.TabIndex = 1;
            this.picImage.TabStop = false;
            // 
            // FrmScanProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 600);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmScanProgram";
            this.Text = "ScanProgram";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem pageSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectScanningDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getImageFromDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eidtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertColoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greyscaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setGreyscaleThresholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allBlackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lvl2Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl3Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl4Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl5Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl6Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl7Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl8Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem lvl9Toolstrip;
        private System.Windows.Forms.ToolStripMenuItem allWhiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actuallSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitToPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encodeMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decodeMessageToolStripMenuItem;

    }
}

