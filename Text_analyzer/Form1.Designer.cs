namespace Text_analyzer
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_open_file = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_save_sort = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_save_ranjir = new System.Windows.Forms.Button();
            this.btn_sort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_total_words = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            this.lbl_name_description = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_date_final_change_description = new System.Windows.Forms.Label();
            this.lbl_time_creation_description = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_date_creation_description = new System.Windows.Forms.Label();
            this.lbl_size_description = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_open_file
            // 
            this.btn_open_file.Location = new System.Drawing.Point(6, 12);
            this.btn_open_file.Name = "btn_open_file";
            this.btn_open_file.Size = new System.Drawing.Size(96, 31);
            this.btn_open_file.TabIndex = 0;
            this.btn_open_file.Text = "Открыть файл";
            this.btn_open_file.UseVisualStyleBackColor = true;
            this.btn_open_file.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_save_sort);
            this.groupBox1.Location = new System.Drawing.Point(12, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сортировка слов по частоте употребления";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 19);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(205, 112);
            this.dataGridView1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Всего уник. слов:";
            // 
            // btn_save_sort
            // 
            this.btn_save_sort.Location = new System.Drawing.Point(408, 94);
            this.btn_save_sort.Name = "btn_save_sort";
            this.btn_save_sort.Size = new System.Drawing.Size(75, 37);
            this.btn_save_sort.TabIndex = 0;
            this.btn_save_sort.Text = "Сохранить";
            this.btn_save_sort.UseVisualStyleBackColor = true;
            this.btn_save_sort.Click += new System.EventHandler(this.btn_save_sort_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.btn_save_ranjir);
            this.groupBox2.Location = new System.Drawing.Point(12, 366);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(489, 137);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Список всех слов по алфавиту";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(333, 108);
            this.listBox1.TabIndex = 2;
            // 
            // btn_save_ranjir
            // 
            this.btn_save_ranjir.Location = new System.Drawing.Point(408, 94);
            this.btn_save_ranjir.Name = "btn_save_ranjir";
            this.btn_save_ranjir.Size = new System.Drawing.Size(75, 37);
            this.btn_save_ranjir.TabIndex = 1;
            this.btn_save_ranjir.Text = "Сохранить";
            this.btn_save_ranjir.UseVisualStyleBackColor = true;
            this.btn_save_ranjir.Click += new System.EventHandler(this.btn_save_ranjir_Click);
            // 
            // btn_sort
            // 
            this.btn_sort.Location = new System.Drawing.Point(6, 48);
            this.btn_sort.Name = "btn_sort";
            this.btn_sort.Size = new System.Drawing.Size(84, 31);
            this.btn_sort.TabIndex = 1;
            this.btn_sort.Text = "Сортировка";
            this.btn_sort.UseVisualStyleBackColor = true;
            this.btn_sort.Click += new System.EventHandler(this.btn_sort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(120, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Всего слов:";
            // 
            // lbl_total_words
            // 
            this.lbl_total_words.AutoSize = true;
            this.lbl_total_words.BackColor = System.Drawing.Color.Transparent;
            this.lbl_total_words.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_total_words.Location = new System.Drawing.Point(209, 12);
            this.lbl_total_words.Name = "lbl_total_words";
            this.lbl_total_words.Size = new System.Drawing.Size(0, 17);
            this.lbl_total_words.TabIndex = 4;
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(6, 85);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(84, 31);
            this.btn_reset.TabIndex = 5;
            this.btn_reset.Text = "Сброс";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // lbl_name_description
            // 
            this.lbl_name_description.AutoSize = true;
            this.lbl_name_description.BackColor = System.Drawing.Color.Transparent;
            this.lbl_name_description.Location = new System.Drawing.Point(193, 50);
            this.lbl_name_description.Name = "lbl_name_description";
            this.lbl_name_description.Size = new System.Drawing.Size(0, 13);
            this.lbl_name_description.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(120, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Имя файла:";
            // 
            // lbl_date_final_change_description
            // 
            this.lbl_date_final_change_description.AutoSize = true;
            this.lbl_date_final_change_description.BackColor = System.Drawing.Color.Transparent;
            this.lbl_date_final_change_description.Location = new System.Drawing.Point(318, 102);
            this.lbl_date_final_change_description.Name = "lbl_date_final_change_description";
            this.lbl_date_final_change_description.Size = new System.Drawing.Size(0, 13);
            this.lbl_date_final_change_description.TabIndex = 31;
            // 
            // lbl_time_creation_description
            // 
            this.lbl_time_creation_description.AutoSize = true;
            this.lbl_time_creation_description.BackColor = System.Drawing.Color.Transparent;
            this.lbl_time_creation_description.Location = new System.Drawing.Point(255, 89);
            this.lbl_time_creation_description.Name = "lbl_time_creation_description";
            this.lbl_time_creation_description.Size = new System.Drawing.Size(0, 13);
            this.lbl_time_creation_description.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(120, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Размер файла (байт):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(120, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Дата последнего изменения файла:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(120, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Время создания файла:";
            // 
            // lbl_date_creation_description
            // 
            this.lbl_date_creation_description.AutoSize = true;
            this.lbl_date_creation_description.BackColor = System.Drawing.Color.Transparent;
            this.lbl_date_creation_description.Location = new System.Drawing.Point(248, 76);
            this.lbl_date_creation_description.Name = "lbl_date_creation_description";
            this.lbl_date_creation_description.Size = new System.Drawing.Size(0, 13);
            this.lbl_date_creation_description.TabIndex = 29;
            // 
            // lbl_size_description
            // 
            this.lbl_size_description.AutoSize = true;
            this.lbl_size_description.BackColor = System.Drawing.Color.Transparent;
            this.lbl_size_description.Location = new System.Drawing.Point(238, 63);
            this.lbl_size_description.Name = "lbl_size_description";
            this.lbl_size_description.Size = new System.Drawing.Size(0, 13);
            this.lbl_size_description.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(120, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Дата создания файла:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btn_open_file);
            this.groupBox3.Controls.Add(this.lbl_name_description);
            this.groupBox3.Controls.Add(this.btn_sort);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btn_reset);
            this.groupBox3.Controls.Add(this.lbl_date_final_change_description);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbl_total_words);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.lbl_time_creation_description);
            this.groupBox3.Controls.Add(this.lbl_size_description);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.lbl_date_creation_description);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, -1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(489, 131);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 51);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(477, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.progressBar1);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(13, 137);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(488, 80);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Статус процесса";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(330, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Пожалуйста, подождите.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(11, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Идёт сохранение данных в файл.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 508);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(529, 546);
            this.MinimumSize = new System.Drawing.Size(529, 456);
            this.Name = "Form1";
            this.Text = "Анализ текстовых файлов";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_open_file;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_save_sort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_save_ranjir;
        private System.Windows.Forms.Button btn_sort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_total_words;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Label lbl_name_description;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_date_final_change_description;
        private System.Windows.Forms.Label lbl_time_creation_description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_date_creation_description;
        private System.Windows.Forms.Label lbl_size_description;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}

