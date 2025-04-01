using System.Collections.Generic;
using System.Drawing;
using System;
using System.Windows.Forms;

namespace Deform
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            radioCylinder = new RadioButton();
            label6 = new Label();
            radioKonus = new RadioButton();
            cBMaterials = new ComboBox();
            radioParrall = new RadioButton();
            label4 = new Label();
            radioTreug = new RadioButton();
            label5 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            label1 = new Label();
            label7 = new Label();
            tBOsnReturn = new TextBox();
            tBHeigReturn = new TextBox();
            label8 = new Label();
            label9 = new Label();
            bTCalc = new Button();
            bTExit = new Button();
            pictureBox1 = new PictureBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioCylinder);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(radioKonus);
            groupBox1.Controls.Add(cBMaterials);
            groupBox1.Controls.Add(radioParrall);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(radioTreug);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(14, 14);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(233, 224);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "Выберите основание";
            // 
            // radioCylinder
            // 
            radioCylinder.AutoSize = true;
            radioCylinder.Location = new Point(18, 22);
            radioCylinder.Margin = new Padding(4, 3, 4, 3);
            radioCylinder.Name = "radioCylinder";
            radioCylinder.Size = new Size(111, 19);
            radioCylinder.TabIndex = 13;
            radioCylinder.TabStop = true;
            radioCylinder.Text = "Круг (Цилиндр)";
            radioCylinder.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(9, 196);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(62, 15);
            label6.TabIndex = 24;
            label6.Text = "Материал";
            // 
            // radioKonus
            // 
            radioKonus.AutoSize = true;
            radioKonus.Location = new Point(18, 48);
            radioKonus.Margin = new Padding(4, 3, 4, 3);
            radioKonus.Name = "radioKonus";
            radioKonus.Size = new Size(94, 19);
            radioKonus.TabIndex = 14;
            radioKonus.TabStop = true;
            radioKonus.Text = "Круг (Конус)";
            radioKonus.UseVisualStyleBackColor = true;
            // 
            // cBMaterials
            // 
            cBMaterials.FormattingEnabled = true;
            cBMaterials.Items.AddRange(new object[] { "Cu", "Al", "Fe", "Ni" });
            cBMaterials.Location = new Point(78, 193);
            cBMaterials.Margin = new Padding(4, 3, 4, 3);
            cBMaterials.Name = "cBMaterials";
            cBMaterials.Size = new Size(116, 23);
            cBMaterials.TabIndex = 23;
            // 
            // radioParrall
            // 
            radioParrall.AutoSize = true;
            radioParrall.Location = new Point(18, 75);
            radioParrall.Margin = new Padding(4, 3, 4, 3);
            radioParrall.Name = "radioParrall";
            radioParrall.Size = new Size(68, 19);
            radioParrall.TabIndex = 15;
            radioParrall.TabStop = true;
            radioParrall.Text = "Квадрат";
            radioParrall.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(202, 162);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(25, 15);
            label4.TabIndex = 22;
            label4.Text = "см.";
            // 
            // radioTreug
            // 
            radioTreug.AutoSize = true;
            radioTreug.Location = new Point(16, 102);
            radioTreug.Margin = new Padding(4, 3, 4, 3);
            radioTreug.Name = "radioTreug";
            radioTreug.Size = new Size(95, 19);
            radioTreug.TabIndex = 16;
            radioTreug.TabStop = true;
            radioTreug.Text = "Треугольник";
            radioTreug.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 162);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(47, 15);
            label5.TabIndex = 21;
            label5.Text = "Высота";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(78, 128);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(116, 23);
            textBox1.TabIndex = 17;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(78, 158);
            textBox2.Margin = new Padding(4, 3, 4, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(116, 23);
            textBox2.TabIndex = 20;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 132);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 18;
            label2.Text = "Диаметр";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(202, 132);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(25, 15);
            label3.TabIndex = 19;
            label3.Text = "см.";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(tBOsnReturn);
            groupBox2.Controls.Add(tBHeigReturn);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label9);
            groupBox2.Location = new Point(14, 257);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(233, 88);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Расчёт деформации";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(202, 55);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 28;
            label1.Text = "см.";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(13, 55);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(47, 15);
            label7.TabIndex = 27;
            label7.Text = "Высота";
            // 
            // tBOsnReturn
            // 
            tBOsnReturn.Enabled = false;
            tBOsnReturn.Location = new Point(78, 22);
            tBOsnReturn.Margin = new Padding(4, 3, 4, 3);
            tBOsnReturn.Name = "tBOsnReturn";
            tBOsnReturn.Size = new Size(116, 23);
            tBOsnReturn.TabIndex = 23;
            // 
            // tBHeigReturn
            // 
            tBHeigReturn.Enabled = false;
            tBHeigReturn.Location = new Point(78, 52);
            tBHeigReturn.Margin = new Padding(4, 3, 4, 3);
            tBHeigReturn.Name = "tBHeigReturn";
            tBHeigReturn.Size = new Size(116, 23);
            tBHeigReturn.TabIndex = 26;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 25);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(55, 15);
            label8.TabIndex = 24;
            label8.Text = "Диаметр";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(202, 25);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(25, 15);
            label9.TabIndex = 25;
            label9.Text = "см.";
            // 
            // bTCalc
            // 
            bTCalc.Location = new Point(14, 352);
            bTCalc.Margin = new Padding(4, 3, 4, 3);
            bTCalc.Name = "bTCalc";
            bTCalc.Size = new Size(233, 36);
            bTCalc.TabIndex = 16;
            bTCalc.Text = "Расчёт";
            bTCalc.UseVisualStyleBackColor = true;
            bTCalc.Click += bTCalc_Click;
            // 
            // bTExit
            // 
            bTExit.Location = new Point(14, 395);
            bTExit.Margin = new Padding(4, 3, 4, 3);
            bTExit.Name = "bTExit";
            bTExit.Size = new Size(233, 36);
            bTExit.TabIndex = 17;
            bTExit.Text = "Выход";
            bTExit.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(268, 36);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(586, 394);
            pictureBox1.TabIndex = 18;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 435);
            Controls.Add(pictureBox1);
            Controls.Add(bTExit);
            Controls.Add(bTCalc);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private GroupBox groupBox1;
        private RadioButton radioCylinder;
        private Label label6;
        private RadioButton radioKonus;
        private ComboBox cBMaterials;
        private RadioButton radioParrall;
        private Label label4;
        private RadioButton radioTreug;
        private Label label5;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Label label3;
        private GroupBox groupBox2;
        private Label label1;
        private Label label7;
        private TextBox tBOsnReturn;
        private TextBox tBHeigReturn;
        private Label label8;
        private Label label9;
        private Button bTCalc;
        private Button bTExit;
        private PictureBox pictureBox1;
    }

    
}