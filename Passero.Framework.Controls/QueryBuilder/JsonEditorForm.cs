using Wisej.Web;

namespace Passero.Framework.Controls;

internal  partial class JsonEditorForm : Form
{
    public JsonEditorForm(string title, string json, bool editable)
    {
        InitializeComponent();

        Text = title;

        _textBox.Text = json;
        _textBox.ReadOnly = !editable;

        _okButton.Text = editable ? "OK" : "Chiudi";
        _okButton.DialogResult = DialogResult.OK;

        _cancelButton.Text = "Annulla";
        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.Visible = editable;

        AcceptButton = _okButton;
        CancelButton = editable ? _cancelButton : _okButton;
    }

    public string JsonText => _textBox.Text;
}
