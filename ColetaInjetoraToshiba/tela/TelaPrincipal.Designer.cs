using System;
using System.Collections.Generic;
using System.Text;

namespace ColetaInjetoraToshiba.tela
{
    partial class TelaPrincipal
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listMaquinas = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMac = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIpServidor = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(133, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = " Iniciar Coleta";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(293, 13);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "Finalizar Coleta";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listMaquinas
            // 
            this.listMaquinas.FormattingEnabled = true;
            this.listMaquinas.ItemHeight = 16;
            this.listMaquinas.Location = new System.Drawing.Point(303, 85);
            this.listMaquinas.Margin = new System.Windows.Forms.Padding(4);
            this.listMaquinas.Name = "listMaquinas";
            this.listMaquinas.Size = new System.Drawing.Size(256, 148);
            this.listMaquinas.TabIndex = 2;
            this.listMaquinas.SelectedIndexChanged += new System.EventHandler(this.listMaquinas_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(351, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Máquinas Cadastradas";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxMac
            // 
            this.textBoxMac.Location = new System.Drawing.Point(17, 211);
            this.textBoxMac.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxMac.Name = "textBoxMac";
            this.textBoxMac.Size = new System.Drawing.Size(132, 22);
            this.textBoxMac.TabIndex = 4;
            this.textBoxMac.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 190);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ip da Máquina";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 13);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 6;
            this.button3.Text = "AtualizarOpcServer";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Programa da Máquina";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(17, 98);
            this.txtCodigo.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(132, 22);
            this.txtCodigo.TabIndex = 7;
            this.txtCodigo.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 135);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Ip do Servidor";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtIpServidor
            // 
            this.txtIpServidor.Location = new System.Drawing.Point(17, 156);
            this.txtIpServidor.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpServidor.Name = "txtIpServidor";
            this.txtIpServidor.Size = new System.Drawing.Size(132, 22);
            this.txtIpServidor.TabIndex = 9;
            this.txtIpServidor.TextChanged += new System.EventHandler(this.txtIpServidor_TextChanged);
            // 
            // TelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 246);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIpServidor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxMac);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listMaquinas);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TelaPrincipal";
            this.Text = "ColetaInjetoraHaitian";
            this.Load += new System.EventHandler(this.TelaPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listMaquinas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMac;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIpServidor;
    }
}