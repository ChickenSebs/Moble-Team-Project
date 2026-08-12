#nullable enable

namespace calendar4;

partial class ddayalarm
{
    private System.ComponentModel.IContainer? components;
    private Label titleLabel = null!;
    private Label messageLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        titleLabel = new Label();
        messageLabel = new Label();
        SuspendLayout();
        //
        // titleLabel
        //
        titleLabel.AutoEllipsis = true;
        titleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        titleLabel.Location = new Point(16, 14);
        titleLabel.Size = new Size(268, 24);
        //
        // messageLabel
        //
        messageLabel.AutoEllipsis = true;
        messageLabel.Font = new Font("맑은 고딕", 9F);
        messageLabel.Location = new Point(16, 48);
        messageLabel.Size = new Size(268, 58);
        //
        // ddayalarm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(300, 122);
        ControlBox = false;
        Controls.Add(messageLabel);
        Controls.Add(titleLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ddayalarm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        TopMost = true;
        ResumeLayout(false);
    }
}
