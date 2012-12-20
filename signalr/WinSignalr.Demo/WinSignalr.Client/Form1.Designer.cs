namespace WinSignalr.Client
{
    partial class Form1
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
            this.btnAfirmativo = new System.Windows.Forms.Button();
            this.btnNegativo = new System.Windows.Forms.Button();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAfirmativo
            // 
            this.btnAfirmativo.Location = new System.Drawing.Point(28, 72);
            this.btnAfirmativo.Name = "btnAfirmativo";
            this.btnAfirmativo.Size = new System.Drawing.Size(134, 52);
            this.btnAfirmativo.TabIndex = 0;
            this.btnAfirmativo.Text = "Afirmativo";
            this.btnAfirmativo.UseVisualStyleBackColor = true;
            this.btnAfirmativo.Click += new System.EventHandler(this.btnAfirmativo_Click);
            // 
            // btnNegativo
            // 
            this.btnNegativo.Location = new System.Drawing.Point(28, 130);
            this.btnNegativo.Name = "btnNegativo";
            this.btnNegativo.Size = new System.Drawing.Size(134, 52);
            this.btnNegativo.TabIndex = 1;
            this.btnNegativo.Text = "Negativo";
            this.btnNegativo.UseVisualStyleBackColor = true;
            this.btnNegativo.Click += new System.EventHandler(this.btnNegativo_Click);
            // 
            // txtNombreUsuario
            // 
            this.txtNombreUsuario.Location = new System.Drawing.Point(28, 30);
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.Size = new System.Drawing.Size(127, 20);
            this.txtNombreUsuario.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre Usuario";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 208);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNombreUsuario);
            this.Controls.Add(this.btnNegativo);
            this.Controls.Add(this.btnAfirmativo);
            this.Name = "Form1";
            this.Text = "Vote!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAfirmativo;
        private System.Windows.Forms.Button btnNegativo;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.Label label1;
    }
}

