namespace Text_analyzer
{
    partial class Activator
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_key = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите ключ активации:";
            // 
            // tbx_key
            // 
            this.tbx_key.Location = new System.Drawing.Point(175, 25);
            this.tbx_key.Name = "tbx_key";
            this.tbx_key.Size = new System.Drawing.Size(230, 20);
            this.tbx_key.TabIndex = 1;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(237, 59);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 46);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "Отмена";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(318, 59);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(87, 46);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "Подтвердить";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // Activator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 124);
            this.ControlBox = false;
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.tbx_key);
            this.Controls.Add(this.label1);
            this.Name = "Activator";
            this.Text = "Activator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_key;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_ok;
    }
}