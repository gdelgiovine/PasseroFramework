using System;

namespace Passero.Framework.Controls;

public sealed class QueryBuilderRequestEventArgs : EventArgs
{
    public bool Handled { get; set; }
}