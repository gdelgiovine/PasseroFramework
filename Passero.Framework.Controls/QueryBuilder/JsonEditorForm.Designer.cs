using System;
using Wisej.Web;
namespace Passero.Framework.Controls;

partial class JsonEditorForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        _textBox = new Wisej.Web.TextBox
        {
            Multiline = true,
            ScrollBars = Wisej.Web.ScrollBars.Both,
            WordWrap = false,
            Dock = Wisej.Web.DockStyle.Fill,
            Font = new System.Drawing.Font("Consolas", 10)
        };

        var buttonPanel = new Wisej.Web.Panel
        {
            Height = 48,
            Dock = Wisej.Web.DockStyle.Bottom
        };

        _okButton = new Wisej.Web.Button
        {
            Width = 90,
            Height = 30,
            Left = 590,
            Top = 9,
            Anchor = Wisej.Web.AnchorStyles.Right | Wisej.Web.AnchorStyles.Top
        };

        _cancelButton = new Wisej.Web.Button
        {
            Width = 90,
            Height = 30,
            Left = 690,
            Top = 9,
            Anchor = Wisej.Web.AnchorStyles.Right | Wisej.Web.AnchorStyles.Top
        };

        buttonPanel.Controls.Add(_okButton);
        buttonPanel.Controls.Add(_cancelButton);

        Controls.Add(_textBox);
        Controls.Add(buttonPanel);

        Width = 800;
        Height = 600;
        StartPosition = Wisej.Web.FormStartPosition.CenterParent;
    }

    private Wisej.Web.TextBox _textBox;
    private Wisej.Web.Button _okButton;
    private Wisej.Web.Button _cancelButton;
}