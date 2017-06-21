namespace WindowsFormsApplication1
{
    partial class Form2
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1Play = new System.Windows.Forms.Button();
            this.button2LeaderBoard = new System.Windows.Forms.Button();
            this.button3AI_Show = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1Play
            // 
            this.button1Play.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1Play.Location = new System.Drawing.Point(54, 109);
            this.button1Play.Name = "button1Play";
            this.button1Play.Size = new System.Drawing.Size(174, 41);
            this.button1Play.TabIndex = 0;
            this.button1Play.Text = "Play";
            this.button1Play.UseVisualStyleBackColor = true;
            this.button1Play.Click += new System.EventHandler(this.button1Play_Click);
            // 
            // button2LeaderBoard
            // 
            this.button2LeaderBoard.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2LeaderBoard.Location = new System.Drawing.Point(54, 156);
            this.button2LeaderBoard.Name = "button2LeaderBoard";
            this.button2LeaderBoard.Size = new System.Drawing.Size(174, 41);
            this.button2LeaderBoard.TabIndex = 1;
            this.button2LeaderBoard.Text = "Leaderboard";
            this.button2LeaderBoard.UseVisualStyleBackColor = true;
            this.button2LeaderBoard.Click += new System.EventHandler(this.button2LeaderBoard_Click);
            // 
            // button3AI_Show
            // 
            this.button3AI_Show.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3AI_Show.Location = new System.Drawing.Point(54, 203);
            this.button3AI_Show.Name = "button3AI_Show";
            this.button3AI_Show.Size = new System.Drawing.Size(174, 41);
            this.button3AI_Show.TabIndex = 2;
            this.button3AI_Show.Text = "AI Show";
            this.button3AI_Show.UseVisualStyleBackColor = true;
            this.button3AI_Show.Click += new System.EventHandler(this.button3AI_Show_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 59);
            this.label1.TabIndex = 3;
            this.label1.Text = "The Dodger";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3AI_Show);
            this.Controls.Add(this.button2LeaderBoard);
            this.Controls.Add(this.button1Play);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onFormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1Play;
        private System.Windows.Forms.Button button2LeaderBoard;
        private System.Windows.Forms.Button button3AI_Show;
        private System.Windows.Forms.Label label1;
    }
}