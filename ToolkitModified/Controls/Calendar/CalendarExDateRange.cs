﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Diagnostics;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a range of dates in a
    /// <see cref="T:System.Windows.Controls.Calendar" />.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public sealed class CalendarExDateRange
    {
        /// <summary>
        /// Gets the first date in the represented range.
        /// </summary>
        /// <value>The first date in the represented range.</value>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Gets the last date in the represented range.
        /// </summary>
        /// <value>The last date in the represented range.</value>
        public DateTime End { get; private set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:System.Windows.Controls.CalendarDateRange" /> class
        /// with a single date.
        /// </summary>
        /// <param name="day">The date to be represented by the range.</param>
        public CalendarExDateRange(DateTime day)
        {
            Start = day;
            End = day;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:System.Windows.Controls.CalendarDateRange" /> class
        /// with a range of dates.
        /// </summary>
        /// <param name="start">
        /// The start of the range to be represented.
        /// </param>
        /// <param name="end">The end of the range to be represented.</param>
        public CalendarExDateRange(DateTime start, DateTime end)
        {
            if (DateTime.Compare(end, start) >= 0)
            {
                Start = start;
                End = end;
            }
            else
            {
                // Always use the start for ranges on the same day
                Start = start;
                End = start;
            }
        }

        /// <summary>
        /// Returns true if any day in the given DateTime range is contained in
        /// the current CalendarDateRange.
        /// </summary>
        /// <param name="range">Inherited code: Requires comment 1.</param>
        /// <returns>Inherited code: Requires comment 2.</returns>
        internal bool ContainsAny(CalendarExDateRange range)
        {
            Debug.Assert(range != null, "range should not be null!");

            int start = DateTime.Compare(Start, range.Start);

            // Check if any part of the supplied range is contained by this
            // range or if the supplied range completely covers this range.
            return (start <= 0 && DateTime.Compare(End, range.Start) >= 0) ||
                (start >= 0 && DateTime.Compare(Start, range.End) <= 0);
        }
    }
}