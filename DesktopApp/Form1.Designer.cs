namespace DesktopApp;

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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        richText = new System.Windows.Forms.RichTextBox();
        button1 = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // richText
        // 
        richText.Location = new System.Drawing.Point(24, 69);
        richText.Name = "richText";
        richText.Size = new System.Drawing.Size(764, 356);
        richText.TabIndex = 1;
        richText.Text = "";
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(576, 455);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(75, 23);
        button1.TabIndex = 2;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click_1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(929, 544);
        Controls.Add(button1);
        Controls.Add(richText);
        Text = "Form1";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.RichTextBox richText;

    #endregion
}