﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a set of selected dates in a
    /// <see cref="T:System.Windows.Controls.Calendar" />.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public sealed class SelectedDatesExCollection : ObservableCollection<DateTime>
    {
        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private Collection<DateTime> _addedItems;

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private Thread _dispatcherThread;

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private bool _isCleared;

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private bool _isRangeAdded;

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        private CalendarEx _owner;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:System.Windows.Controls.SelectedDatesCollection" />
        /// class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="T:System.Windows.Controls.Calendar" /> associated
        /// with this object.
        /// </param>
        public SelectedDatesExCollection(CalendarEx owner)
        {
            _owner = owner;
            _addedItems = new Collection<DateTime>();
            _dispatcherThread = Thread.CurrentThread;
        }

        /// <summary>
        /// Adds all the dates in the specified range, which includes the first
        /// and last dates, to the collection.
        /// </summary>
        /// <param name="start">The first date to add to the collection.</param>
        /// <param name="end">The last date to add to the collection.</param>
        public void AddRange(DateTime start, DateTime end)
        {
            DateTime? rangeStart;

            // increment parameter specifies if the Days were selected in
            // Descending order or Ascending order based on this value, we add 
            // the days in the range either in Ascending order or in Descending
            // order
            int increment = (DateTime.Compare(end, start) >= 0) ? 1 : -1;

            _addedItems.Clear();

            rangeStart = start;
            _isRangeAdded = true;

            if (_owner.IsMouseSelection)
            {
                // In Mouse Selection we allow the user to be able to add
                // multiple ranges in one action in MultipleRange Mode.  In
                // SingleRange Mode, we only add the first selected range.
                while (rangeStart.HasValue && DateTime.Compare(end, rangeStart.Value) != -increment)
                {
                    if (CalendarEx.IsValidDateSelection(_owner, rangeStart))
                    {
                        Add(rangeStart.Value);
                    }
                    else
                    {
                        if (_owner.SelectionMode == CalendarExSelectionMode.SingleRange)
                        {
                            _owner.HoverEnd = rangeStart.Value.AddDays(-increment);
                            break;
                        }
                    }

                    rangeStart = DateTimeHelper.AddDays(rangeStart.Value, increment);
                }
            }
            else
            {
                // If CalendarSelectionMode.SingleRange and a user
                // programmatically tries to add multiple ranges, we will throw
                // away the old range and replace it with the new one.  In order
                // to provide the removed items without an additional event, we
                // are calling ClearInternal
                if (_owner.SelectionMode == CalendarExSelectionMode.SingleRange && Count > 0)
                {
                    foreach (DateTime item in this)
                    {
                        _owner.RemovedItems.Add(item);
                    }
                    ClearInternal();
                }

                while (rangeStart.HasValue && DateTime.Compare(end, rangeStart.Value) != -increment)
                {
                    Add(rangeStart.Value);
                    rangeStart = DateTimeHelper.AddDays(rangeStart.Value, increment);
                }
            }

            _owner.OnSelectedDatesCollectionChanged(new SelectionChangedEventArgs(_owner.RemovedItems, _addedItems));
            _owner.RemovedItems.Clear();
            _owner.UpdateMonths();
            _isRangeAdded = false;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>
        /// This implementation raises the CollectionChanged event.
        /// </remarks>
        protected override void ClearItems()
        {
            if (!IsValidThread())
            {
                throw new NotSupportedException(System.Windows.Controls.Properties.Resources.CalendarCollection_MultiThreadedCollectionChangeNotSupported);
            }
            Collection<DateTime> addedItems = new Collection<DateTime>();
            Collection<DateTime> removedItems = new Collection<DateTime>();

            foreach (DateTime item in this)
            {
                removedItems.Add(item);
            }

            base.ClearItems();

            // The event fires after SelectedDate changes
            if (_owner.SelectionMode != CalendarExSelectionMode.None && _owner.SelectedDate != null)
            {
                _owner.SelectedDate = null;
            }

            if (removedItems.Count != 0)
            {
                _owner.OnSelectedDatesCollectionChanged(new SelectionChangedEventArgs(removedItems, addedItems));
            }
            _owner.UpdateMonths();
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which item should be inserted.
        /// </param>
        /// <param name="item">The object to insert.</param>
        /// <remarks>
        /// This implementation raises the CollectionChanged event.
        /// </remarks>
        protected override void InsertItem(int index, DateTime item)
        {
            if (!IsValidThread())
            {
                throw new NotSupportedException(System.Windows.Controls.Properties.Resources.CalendarCollection_MultiThreadedCollectionChangeNotSupported);
            }

            if (!Contains(item))
            {
                Collection<DateTime> addedItems = new Collection<DateTime>();

                if (CheckSelectionMode())
                {
                    if (CalendarEx.IsValidDateSelection(_owner, item))
                    {
                        // If the Collection is cleared since it is SingleRange
                        // and it had another range set the index to 0
                        if (_isCleared)
                        {
                            index = 0;
                            _isCleared = false;
                        }

                        base.InsertItem(index, item);

                        // The event fires after SelectedDate changes
                        if (index == 0 && !(_owner.SelectedDate.HasValue && DateTime.Compare(_owner.SelectedDate.Value, item) == 0))
                        {
                            _owner.SelectedDate = item;
                        }

                        if (!_isRangeAdded)
                        {
                            addedItems.Add(item);

                            _owner.OnSelectedDatesCollectionChanged(new SelectionChangedEventArgs(_owner.RemovedItems, addedItems));
                            _owner.RemovedItems.Clear();
                            int monthDifference = DateTimeHelper.CompareYearMonth(item, _owner.DisplayDateInternal);

                            if (monthDifference < 2 && monthDifference > -2)
                            {
                                _owner.UpdateMonths();
                            }
                        }
                        else
                        {
                            _addedItems.Add(item);
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(System.Windows.Controls.Properties.Resources.Calendar_OnSelectedDateChanged_InvalidValue);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <remarks>
        /// This implementation raises the CollectionChanged event.
        /// </remarks>
        protected override void RemoveItem(int index)
        {
            if (!IsValidThread())
            {
                throw new NotSupportedException(System.Windows.Controls.Properties.Resources.CalendarCollection_MultiThreadedCollectionChangeNotSupported);
            }

            if (index >= Count)
            {
                base.RemoveItem(index);
            }
            else
            {
                Collection<DateTime> addedItems = new Collection<DateTime>();
                Collection<DateTime> removedItems = new Collection<DateTime>();
                int monthDifference = DateTimeHelper.CompareYearMonth(this[index], _owner.DisplayDateInternal);

                removedItems.Add(this[index]);
                base.RemoveItem(index);

                // The event fires after SelectedDate changes
                if (index == 0)
                {
                    if (Count > 0)
                    {
                        _owner.SelectedDate = this[0];
                    }
                    else
                    {
                        _owner.SelectedDate = null;
                    }
                }

                _owner.OnSelectedDatesCollectionChanged(new SelectionChangedEventArgs(removedItems, addedItems));

                if (monthDifference < 2 && monthDifference > -2)
                {
                    _owner.UpdateMonths();
                }
            }
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to replace.
        /// </param>
        /// <param name="item">
        /// The new value for the element at the specified index.
        /// </param>
        /// <remarks>
        /// This implementation raises the CollectionChanged event.
        /// </remarks>
        protected override void SetItem(int index, DateTime item)
        {
            if (!IsValidThread())
            {
                throw new NotSupportedException(System.Windows.Controls.Properties.Resources.CalendarCollection_MultiThreadedCollectionChangeNotSupported);
            }

            if (!Contains(item))
            {
                Collection<DateTime> addedItems = new Collection<DateTime>();
                Collection<DateTime> removedItems = new Collection<DateTime>();

                if (index >= Count)
                {
                    base.SetItem(index, item);
                }
                else
                {
                    if (item != null && DateTime.Compare(this[index], item) != 0 && CalendarEx.IsValidDateSelection(_owner, item))
                    {
                        removedItems.Add(this[index]);
                        base.SetItem(index, item);
                        addedItems.Add(item);

                        // The event fires after SelectedDate changes
                        if (index == 0 && !(_owner.SelectedDate.HasValue && DateTime.Compare(_owner.SelectedDate.Value, item) == 0))
                        {
                            _owner.SelectedDate = item;
                        }
                        _owner.OnSelectedDatesCollectionChanged(new SelectionChangedEventArgs(removedItems, addedItems));

                        int monthDifference = DateTimeHelper.CompareYearMonth(item, _owner.DisplayDateInternal);

                        if (monthDifference < 2 && monthDifference > -2)
                        {
                            _owner.UpdateMonths();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        internal void ClearInternal()
        {
            base.ClearItems();
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <returns>Inherited code: Requires comment 1.</returns>
        private bool CheckSelectionMode()
        {
            if (_owner.SelectionMode == CalendarExSelectionMode.None)
            {
                throw new InvalidOperationException(System.Windows.Controls.Properties.Resources.Calendar_OnSelectedDateChanged_InvalidOperation);
            }
            if (_owner.SelectionMode == CalendarExSelectionMode.SingleDate && Count > 0)
            {
                throw new InvalidOperationException(System.Windows.Controls.Properties.Resources.Calendar_CheckSelectionMode_InvalidOperation);
            }

            // if user tries to add an item into the SelectedDates in
            // SingleRange mode, we throw away the old range and replace it with
            // the new one in order to provide the removed items without an
            // additional event, we are calling ClearInternal
            if (_owner.SelectionMode == CalendarExSelectionMode.SingleRange && !_isRangeAdded && Count > 0)
            {
                foreach (DateTime item in this)
                {
                    _owner.RemovedItems.Add(item);
                }
                ClearInternal();
                _isCleared = true;
            }
            return true;
        }

        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <returns>Inherited code: Requires comment 1.</returns>
        private bool IsValidThread()
        {
            return Thread.CurrentThread == _dispatcherThread;
        }
    }
}