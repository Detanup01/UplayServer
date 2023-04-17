namespace Creator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            serverpath = new TextBox();
            label1 = new Label();
            label2 = new Label();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button1 = new Button();
            productbox = new TextBox();
            manifestbox = new TextBox();
            label3 = new Label();
            button2 = new Button();
            button3 = new Button();
            label4 = new Label();
            sliceverbox = new TextBox();
            label5 = new Label();
            button4 = new Button();
            button5 = new Button();
            maxsize = new TextBox();
            label6 = new Label();
            button6 = new Button();
            chunkbox = new ListBox();
            button7 = new Button();
            button8 = new Button();
            chunktextbox = new TextBox();
            compressionbox = new ComboBox();
            SuspendLayout();
            // 
            // serverpath
            // 
            serverpath.Location = new Point(12, 53);
            serverpath.Name = "serverpath";
            serverpath.Size = new Size(776, 23);
            serverpath.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 35);
            label1.Name = "label1";
            label1.Size = new Size(127, 15);
            label1.TabIndex = 1;
            label1.Text = "Path for the ServerFiles";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 88);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 2;
            label2.Text = "Product Id";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            button1.Location = new Point(12, 194);
            button1.Name = "button1";
            button1.Size = new Size(127, 23);
            button1.TabIndex = 3;
            button1.Text = "Select File";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // productbox
            // 
            productbox.Location = new Point(12, 115);
            productbox.Name = "productbox";
            productbox.Size = new Size(100, 23);
            productbox.TabIndex = 4;
            // 
            // manifestbox
            // 
            manifestbox.Location = new Point(166, 115);
            manifestbox.Name = "manifestbox";
            manifestbox.Size = new Size(196, 23);
            manifestbox.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(166, 88);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 5;
            label3.Text = "Manifest Id";
            // 
            // button2
            // 
            button2.Location = new Point(12, 223);
            button2.Name = "button2";
            button2.Size = new Size(127, 23);
            button2.TabIndex = 7;
            button2.Text = "Select Direrctory";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(637, 192);
            button3.Name = "button3";
            button3.Size = new Size(151, 31);
            button3.TabIndex = 8;
            button3.Text = "Make Manifest";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(407, 88);
            label4.Name = "label4";
            label4.Size = new Size(77, 15);
            label4.TabIndex = 9;
            label4.Text = "Compression";
            // 
            // sliceverbox
            // 
            sliceverbox.Location = new Point(561, 115);
            sliceverbox.Name = "sliceverbox";
            sliceverbox.Size = new Size(100, 23);
            sliceverbox.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(561, 88);
            label5.Name = "label5";
            label5.Size = new Size(72, 15);
            label5.TabIndex = 11;
            label5.Text = "Slice Version";
            // 
            // button4
            // 
            button4.Location = new Point(12, 252);
            button4.Name = "button4";
            button4.Size = new Size(127, 23);
            button4.TabIndex = 13;
            button4.Text = "Load ManifestBytes";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(637, 229);
            button5.Name = "button5";
            button5.Size = new Size(151, 46);
            button5.TabIndex = 14;
            button5.Text = "Make Manifest (JSON+MBytes)";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // maxsize
            // 
            maxsize.Location = new Point(688, 115);
            maxsize.Name = "maxsize";
            maxsize.Size = new Size(100, 23);
            maxsize.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(688, 88);
            label6.Name = "label6";
            label6.Size = new Size(105, 15);
            label6.TabIndex = 15;
            label6.Text = "Max Size (in bytes)";
            // 
            // button6
            // 
            button6.Location = new Point(12, 415);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 17;
            button6.Text = "Clean";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // chunkbox
            // 
            chunkbox.FormattingEnabled = true;
            chunkbox.ItemHeight = 15;
            chunkbox.Items.AddRange(new object[] { "Basic" });
            chunkbox.Location = new Point(166, 194);
            chunkbox.Name = "chunkbox";
            chunkbox.Size = new Size(152, 244);
            chunkbox.TabIndex = 18;
            // 
            // button7
            // 
            button7.Location = new Point(358, 415);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 19;
            button7.Text = "Remove";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(358, 386);
            button8.Name = "button8";
            button8.Size = new Size(75, 23);
            button8.TabIndex = 20;
            button8.Text = "Add";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // chunktextbox
            // 
            chunktextbox.Location = new Point(324, 357);
            chunktextbox.Name = "chunktextbox";
            chunktextbox.Size = new Size(140, 23);
            chunktextbox.TabIndex = 21;
            // 
            // compressionbox
            // 
            compressionbox.DropDownStyle = ComboBoxStyle.DropDownList;
            compressionbox.FormattingEnabled = true;
            compressionbox.Items.AddRange(new object[] { "None", "Deflate", "Zstd", "Lzham" });
            compressionbox.Location = new Point(407, 115);
            compressionbox.Name = "compressionbox";
            compressionbox.Size = new Size(100, 23);
            compressionbox.TabIndex = 22;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(compressionbox);
            Controls.Add(chunktextbox);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(chunkbox);
            Controls.Add(button6);
            Controls.Add(maxsize);
            Controls.Add(label6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(sliceverbox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(manifestbox);
            Controls.Add(label3);
            Controls.Add(productbox);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(serverpath);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox serverpath;
        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button1;
        private TextBox productbox;
        private TextBox manifestbox;
        private Label label3;
        private Button button2;
        private Button button3;
        private Label label4;
        private TextBox sliceverbox;
        private Label label5;
        private Button button4;
        private Button button5;
        private TextBox maxsize;
        private Label label6;
        private Button button6;
        private ListBox chunkbox;
        private Button button7;
        private Button button8;
        private TextBox chunktextbox;
        private ComboBox compressionbox;
    }
}