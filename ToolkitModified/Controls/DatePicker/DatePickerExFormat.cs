// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;

namespace System.Windows.Controls
{
    /// <summary>
    /// Specifies date formats for a
    /// <see cref="T:System.Windows.Controls.DatePicker" />.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public enum DatePickerExFormat
    {
        /// <summary>
        /// Specifies that the date should be displayed using unabbreviated days
        /// of the week and month names.
        /// </summary>
        Long = 0,

        /// <summary>
        /// Specifies that the date should be displayed using abbreviated days
        /// of the week and month names.
        /// </summary>
        Short = 1,

        #region Juan Mejia - Modification
        /// <summary>
        /// Specifies that the date should display only the month and year.
        /// </summary>
        MonthYear = 2

        #endregion

    }
}