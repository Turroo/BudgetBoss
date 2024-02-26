using System.Windows.Forms;

namespace BudgetBossClient
{
    partial class FinanzeIniziali
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
            this.FinanzeInizialiLabel = new System.Windows.Forms.Label();
            this.txtContanti = new System.Windows.Forms.TextBox();
            this.LabelContanti = new System.Windows.Forms.Label();
            this.txtCarte = new System.Windows.Forms.TextBox();
            this.LabelCarte = new System.Windows.Forms.Label();
            this.txtFinanzeOnline = new System.Windows.Forms.TextBox();
            this.LabelFinanzeOnline = new System.Windows.Forms.Label();
            this.InviaFinanzeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FinanzeInizialiLabel
            // 
            this.FinanzeInizialiLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FinanzeInizialiLabel.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinanzeInizialiLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.FinanzeInizialiLabel.Location = new System.Drawing.Point(12, 9);
            this.FinanzeInizialiLabel.Name = "FinanzeInizialiLabel";
            this.FinanzeInizialiLabel.Size = new System.Drawing.Size(776, 72);
            this.FinanzeInizialiLabel.TabIndex = 2;
            this.FinanzeInizialiLabel.Text = "Inserimento Finanze Iniziali";
            this.FinanzeInizialiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtContanti
            // 
            this.txtContanti.Location = new System.Drawing.Point(398, 84);
            this.txtContanti.Name = "txtContanti";
            this.txtContanti.Size = new System.Drawing.Size(205, 22);
            this.txtContanti.TabIndex = 9;
            this.txtContanti.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtContanti_KeyPress);
            // 
            // LabelContanti
            // 
            this.LabelContanti.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelContanti.Location = new System.Drawing.Point(193, 80);
            this.LabelContanti.Name = "LabelContanti";
            this.LabelContanti.Size = new System.Drawing.Size(127, 28);
            this.LabelContanti.TabIndex = 8;
            this.LabelContanti.Text = "Contanti";
            this.LabelContanti.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCarte
            // 
            this.txtCarte.Location = new System.Drawing.Point(398, 160);
            this.txtCarte.Name = "txtCarte";
            this.txtCarte.Size = new System.Drawing.Size(205, 22);
            this.txtCarte.TabIndex = 11;
            this.txtCarte.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCarte_KeyPress);
            // 
            // LabelCarte
            // 
            this.LabelCarte.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCarte.Location = new System.Drawing.Point(193, 156);
            this.LabelCarte.Name = "LabelCarte";
            this.LabelCarte.Size = new System.Drawing.Size(127, 26);
            this.LabelCarte.TabIndex = 10;
            this.LabelCarte.Text = "Carte ";
            this.LabelCarte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFinanzeOnline
            // 
            this.txtFinanzeOnline.Location = new System.Drawing.Point(398, 243);
            this.txtFinanzeOnline.Name = "txtFinanzeOnline";
            this.txtFinanzeOnline.Size = new System.Drawing.Size(205, 22);
            this.txtFinanzeOnline.TabIndex = 13;
            this.txtFinanzeOnline.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFinanzeOnline_KeyPress);
            // 
            // LabelFinanzeOnline
            // 
            this.LabelFinanzeOnline.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFinanzeOnline.Location = new System.Drawing.Point(193, 230);
            this.LabelFinanzeOnline.Name = "LabelFinanzeOnline";
            this.LabelFinanzeOnline.Size = new System.Drawing.Size(181, 46);
            this.LabelFinanzeOnline.TabIndex = 12;
            this.LabelFinanzeOnline.Text = "Metodi di pagamento Online";
            this.LabelFinanzeOnline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InviaFinanzeButton
            // 
            this.InviaFinanzeButton.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InviaFinanzeButton.Location = new System.Drawing.Point(347, 318);
            this.InviaFinanzeButton.Name = "InviaFinanzeButton";
            this.InviaFinanzeButton.Size = new System.Drawing.Size(114, 36);
            this.InviaFinanzeButton.TabIndex = 14;
            this.InviaFinanzeButton.Text = "Invia";
            this.InviaFinanzeButton.UseVisualStyleBackColor = true;
            this.InviaFinanzeButton.Click += new System.EventHandler(this.InviaFinanzeButton_Click);
            // 
            // FinanzeIniziali
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InviaFinanzeButton);
            this.Controls.Add(this.txtFinanzeOnline);
            this.Controls.Add(this.LabelFinanzeOnline);
            this.Controls.Add(this.txtCarte);
            this.Controls.Add(this.LabelCarte);
            this.Controls.Add(this.txtContanti);
            this.Controls.Add(this.LabelContanti);
            this.Controls.Add(this.FinanzeInizialiLabel);
            this.Name = "FinanzeIniziali";
            this.Text = "FinanzeIniziali";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FinanzeInizialiLabel;
        private System.Windows.Forms.TextBox txtContanti;
        private System.Windows.Forms.Label LabelContanti;
        private System.Windows.Forms.TextBox txtCarte;
        private System.Windows.Forms.Label LabelCarte;
        private System.Windows.Forms.TextBox txtFinanzeOnline;
        private System.Windows.Forms.Label LabelFinanzeOnline;
        private System.Windows.Forms.Button InviaFinanzeButton;
    }
}