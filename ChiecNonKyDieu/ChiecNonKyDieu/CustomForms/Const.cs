using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.CustomForms
{
    public enum MessageBoxType
    {
        ConfirmationWithYesNo = 0,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }

    public enum MessageBoxImage
    {
        Warning = 0,
        Question,
        Information,
        Error,
        None
    }
}
