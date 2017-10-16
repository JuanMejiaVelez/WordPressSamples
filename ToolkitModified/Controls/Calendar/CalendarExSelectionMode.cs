﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;

namespace System.Windows.Controls
{
    /// <summary>
    /// Specifies values that describe the available selection modes for a
    /// <see cref="T:System.Windows.Controls.Calendar" />.
    /// </summary>
    /// <remarks>
    /// This enumeration provides the values that are used by the SelectionMode
    /// property.
    /// </remarks>
    /// <QualityBand>Mature</QualityBand>
    public enum CalendarExSelectionMode
    {
        /// <summary>
        /// Only a single date can be selected. Use the
        /// <see cref="P:System.Windows.Controls.Calendar.SelectedDate" />
        /// property to retrieve the selected date.
        /// </summary>
        SingleDate = 0,

        /// <summary>
        /// A single range of dates can be selected. Use 
        /// <see cref="P:System.Windows.Controls.Calendar.SelectedDates" />
        /// property to retrieve the selected dates.
        /// </summary>
        SingleRange = 1,

        /// <summary>
        /// Multiple non-contiguous ranges of dates can be selected. Use the
        /// <see cref="P:System.Windows.Controls.Calendar.SelectedDates" />
        /// property to retrieve the selected dates.
        /// </summary>
        MultipleRange = 2,

        /// <summary>
        /// No selections are allowed.
        /// </summary>
        None = 3,
    }
}