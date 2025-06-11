namespace 上野迅_インターン_20250513
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.directoryEntry2 = new System.DirectoryServices.DirectoryEntry();
            this.label1 = new System.Windows.Forms.Label();
            this.panelDrop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.play = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.box = new System.Windows.Forms.GroupBox();
            this.trackBarSeek = new System.Windows.Forms.TrackBar();
            this.totalTime = new System.Windows.Forms.Label();
            this.currentTime = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.radioRepeatNone = new System.Windows.Forms.RadioButton();
            this.radioRepeatOne = new System.Windows.Forms.RadioButton();
            this.radioRepeatAll = new System.Windows.Forms.RadioButton();
            this.panelDrop.SuspendLayout();
            this.box.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSeek)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.Location = new System.Drawing.Point(542, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.referance);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Location = new System.Drawing.Point(530, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "コンピュータから参照する場合はこちらから";
            // 
            // panelDrop
            // 
            this.panelDrop.BackColor = System.Drawing.Color.White;
            this.panelDrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDrop.Controls.Add(this.label2);
            this.panelDrop.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelDrop.Location = new System.Drawing.Point(21, 12);
            this.panelDrop.Name = "panelDrop";
            this.panelDrop.Size = new System.Drawing.Size(464, 151);
            this.panelDrop.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(306, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "ここにwavファイルまたはmp3ファイルをドラッグ＆ドロップしてください";
            // 
            // listViewFiles
            // 
            this.listViewFiles.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileName,
            this.date,
            this.type,
            this.size});
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.Location = new System.Drawing.Point(9, 12);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(550, 158);
            this.listViewFiles.TabIndex = 3;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            // 
            // fileName
            // 
            this.fileName.Text = "ファイル名";
            this.fileName.Width = 259;
            // 
            // date
            // 
            this.date.Text = "日付";
            this.date.Width = 130;
            // 
            // type
            // 
            this.type.Text = "種類";
            this.type.Width = 101;
            // 
            // size
            // 
            this.size.Text = "サイズ";
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(578, 37);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(75, 23);
            this.play.TabIndex = 4;
            this.play.Text = "再生";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(659, 37);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 5;
            this.delete.Text = "削除";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // box
            // 
            this.box.Controls.Add(this.radioRepeatAll);
            this.box.Controls.Add(this.radioRepeatOne);
            this.box.Controls.Add(this.radioRepeatNone);
            this.box.Controls.Add(this.delete);
            this.box.Controls.Add(this.play);
            this.box.Controls.Add(this.listViewFiles);
            this.box.Cursor = System.Windows.Forms.Cursors.Default;
            this.box.Location = new System.Drawing.Point(12, 169);
            this.box.Name = "box";
            this.box.Size = new System.Drawing.Size(749, 170);
            this.box.TabIndex = 6;
            this.box.TabStop = false;
            this.box.Text = "選択ファイル";
            // 
            // trackBarSeek
            // 
            this.trackBarSeek.BackColor = System.Drawing.Color.White;
            this.trackBarSeek.Location = new System.Drawing.Point(12, 369);
            this.trackBarSeek.Maximum = 100;
            this.trackBarSeek.Name = "trackBarSeek";
            this.trackBarSeek.Size = new System.Drawing.Size(749, 45);
            this.trackBarSeek.TabIndex = 6;
            this.trackBarSeek.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSeek.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarSeek_MouseDown);
            this.trackBarSeek.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarSeek_MouseUp);
            // 
            // totalTime
            // 
            this.totalTime.AutoSize = true;
            this.totalTime.Location = new System.Drawing.Point(724, 354);
            this.totalTime.Name = "totalTime";
            this.totalTime.Size = new System.Drawing.Size(37, 12);
            this.totalTime.TabIndex = 7;
            this.totalTime.Text = "/--:--";
            // 
            // currentTime
            // 
            this.currentTime.AllowDrop = true;
            this.currentTime.AutoSize = true;
            this.currentTime.Location = new System.Drawing.Point(697, 354);
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(31, 12);
            this.currentTime.TabIndex = 8;
            this.currentTime.Text = "--:--";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Cursor = System.Windows.Forms.Cursors.Default;
            this.title.Location = new System.Drawing.Point(25, 349);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(0, 12);
            this.title.TabIndex = 9;
            // 
            // radioRepeatNone
            // 
            this.radioRepeatNone.AutoSize = true;
            this.radioRepeatNone.Checked = true;
            this.radioRepeatNone.Location = new System.Drawing.Point(613, 83);
            this.radioRepeatNone.Name = "radioRepeatNone";
            this.radioRepeatNone.Size = new System.Drawing.Size(76, 16);
            this.radioRepeatNone.TabIndex = 6;
            this.radioRepeatNone.TabStop = true;
            this.radioRepeatNone.Text = "リピートなし";
            this.radioRepeatNone.UseVisualStyleBackColor = true;
            this.radioRepeatNone.CheckedChanged += new System.EventHandler(this.radioRepeatNone_CheckedChanged);
            // 
            // radioRepeatOne
            // 
            this.radioRepeatOne.AutoSize = true;
            this.radioRepeatOne.Location = new System.Drawing.Point(613, 105);
            this.radioRepeatOne.Name = "radioRepeatOne";
            this.radioRepeatOne.Size = new System.Drawing.Size(75, 16);
            this.radioRepeatOne.TabIndex = 7;
            this.radioRepeatOne.TabStop = true;
            this.radioRepeatOne.Text = "1曲リピート";
            this.radioRepeatOne.UseVisualStyleBackColor = true;
            this.radioRepeatOne.CheckedChanged += new System.EventHandler(this.radioRepeatOne_CheckedChanged);
            // 
            // radioRepeatAll
            // 
            this.radioRepeatAll.AutoSize = true;
            this.radioRepeatAll.Location = new System.Drawing.Point(613, 127);
            this.radioRepeatAll.Name = "radioRepeatAll";
            this.radioRepeatAll.Size = new System.Drawing.Size(81, 16);
            this.radioRepeatAll.TabIndex = 8;
            this.radioRepeatAll.TabStop = true;
            this.radioRepeatAll.Text = "全曲リピート";
            this.radioRepeatAll.UseVisualStyleBackColor = true;
            this.radioRepeatAll.CheckedChanged += new System.EventHandler(this.radioRepeatAll_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(773, 426);
            this.Controls.Add(this.title);
            this.Controls.Add(this.currentTime);
            this.Controls.Add(this.totalTime);
            this.Controls.Add(this.trackBarSeek);
            this.Controls.Add(this.panelDrop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.box);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Text = "VoiShredder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelDrop.ResumeLayout(false);
            this.panelDrop.PerformLayout();
            this.box.ResumeLayout(false);
            this.box.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSeek)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.DirectoryServices.DirectoryEntry directoryEntry2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelDrop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader fileName;
        private System.Windows.Forms.ColumnHeader date;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader size;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.GroupBox box;
        private System.Windows.Forms.TrackBar trackBarSeek;
        private System.Windows.Forms.Label totalTime;
        private System.Windows.Forms.Label currentTime;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.RadioButton radioRepeatAll;
        private System.Windows.Forms.RadioButton radioRepeatOne;
        private System.Windows.Forms.RadioButton radioRepeatNone;
    }
}

