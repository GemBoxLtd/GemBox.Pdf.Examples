partial class Form1
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    System.ComponentModel.IContainer components = null;

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
    void InitializeComponent()
    {
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.LoadFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintPreviewControl = new System.Windows.Forms.PrintPreviewControl();
            this.PageLb = new System.Windows.Forms.Label();
            this.PageUpDown = new System.Windows.Forms.NumericUpDown();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadFileMenuItem,
            this.PrintFileMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MenuStrip.Size = new System.Drawing.Size(1045, 28);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // LoadFileMenuItem
            // 
            this.LoadFileMenuItem.Name = "LoadFileMenuItem";
            this.LoadFileMenuItem.Size = new System.Drawing.Size(54, 24);
            this.LoadFileMenuItem.Text = "Load";
            this.LoadFileMenuItem.Click += new System.EventHandler(this.LoadFileMenuItem_Click);
            // 
            // PrintFileMenuItem
            // 
            this.PrintFileMenuItem.Name = "PrintFileMenuItem";
            this.PrintFileMenuItem.Size = new System.Drawing.Size(51, 24);
            this.PrintFileMenuItem.Text = "Print";
            this.PrintFileMenuItem.Click += new System.EventHandler(this.PrintFileMenuItem_Click);
            // 
            // PrintPreviewControl
            // 
            this.PrintPreviewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintPreviewControl.Location = new System.Drawing.Point(0, 65);
            this.PrintPreviewControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PrintPreviewControl.Name = "PrintPreviewControl";
            this.PrintPreviewControl.Size = new System.Drawing.Size(1045, 441);
            this.PrintPreviewControl.TabIndex = 1;
            // 
            // PageLb
            // 
            this.PageLb.AutoSize = true;
            this.PageLb.Location = new System.Drawing.Point(16, 41);
            this.PageLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PageLb.Name = "PageLb";
            this.PageLb.Size = new System.Drawing.Size(45, 17);
            this.PageLb.TabIndex = 2;
            this.PageLb.Text = "Page:";
            // 
            // PageUpDown
            // 
            this.PageUpDown.Location = new System.Drawing.Point(71, 36);
            this.PageUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PageUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PageUpDown.Name = "PageUpDown";
            this.PageUpDown.Size = new System.Drawing.Size(53, 22);
            this.PageUpDown.TabIndex = 3;
            this.PageUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PageUpDown.ValueChanged += new System.EventHandler(this.PageUpDown_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 506);
            this.Controls.Add(this.PageUpDown);
            this.Controls.Add(this.PageLb);
            this.Controls.Add(this.PrintPreviewControl);
            this.Controls.Add(this.MenuStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Printing in Windows Forms application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    System.Windows.Forms.MenuStrip MenuStrip;
    System.Windows.Forms.ToolStripMenuItem LoadFileMenuItem;
    System.Windows.Forms.ToolStripMenuItem PrintFileMenuItem;
    System.Windows.Forms.PrintPreviewControl PrintPreviewControl;
    System.Windows.Forms.Label PageLb;
    System.Windows.Forms.NumericUpDown PageUpDown;
}
