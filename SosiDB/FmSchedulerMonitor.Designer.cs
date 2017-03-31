namespace SosiDB
{
    partial class FmSchedulerMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmSchedulerMonitor));
            this.rtDeleted = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ltSchedule = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // rtDeleted
            // 
            this.rtDeleted.BackColor = System.Drawing.SystemColors.WindowText;
            this.rtDeleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtDeleted.ForeColor = System.Drawing.Color.Red;
            this.rtDeleted.Location = new System.Drawing.Point(4, 227);
            this.rtDeleted.Name = "rtDeleted";
            this.rtDeleted.Size = new System.Drawing.Size(593, 170);
            this.rtDeleted.TabIndex = 56;
            this.rtDeleted.Text = "Good";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 20);
            this.label1.TabIndex = 57;
            this.label1.Text = "Schedule-DateFileCreated.sch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 20);
            this.label3.TabIndex = 59;
            this.label3.Text = "Deleted-Filename";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(268, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 20);
            this.label4.TabIndex = 60;
            this.label4.Text = "Deleted-Time";
            // 
            // ltSchedule
            // 
            this.ltSchedule.BackColor = System.Drawing.SystemColors.InfoText;
            this.ltSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltSchedule.ForeColor = System.Drawing.Color.Blue;
            this.ltSchedule.FormattingEnabled = true;
            this.ltSchedule.ItemHeight = 24;
            this.ltSchedule.Location = new System.Drawing.Point(4, 29);
            this.ltSchedule.Name = "ltSchedule";
            this.ltSchedule.Size = new System.Drawing.Size(593, 172);
            this.ltSchedule.TabIndex = 61;
            // 
            // FmSchedulerMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 402);
            this.Controls.Add(this.ltSchedule);
            this.Controls.Add(this.rtDeleted);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(625, 441);
            this.MinimumSize = new System.Drawing.Size(625, 441);
            this.Name = "FmSchedulerMonitor";
            this.Text = "Scheduler Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtDeleted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox ltSchedule;
    }
}

