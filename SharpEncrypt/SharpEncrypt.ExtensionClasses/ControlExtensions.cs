﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpEncrypt.ExtensionClasses
{
    public static class ControlExtensions
    {
        public static IEnumerable<Control> AllControls(this Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            foreach (Control c in control.Controls)
            {
                yield return c;
                foreach (Control child in c.Controls)
                    yield return child;
            }
        }
    }
}
