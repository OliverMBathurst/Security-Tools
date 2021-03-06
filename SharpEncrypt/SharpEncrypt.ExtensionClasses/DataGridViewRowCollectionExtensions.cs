﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SharpEncrypt.ExtensionClasses
{
    public static class DataGridViewRowCollectionExtensions
    {
        public static void RemoveAll(this DataGridViewRowCollection collection, IEnumerable<DataGridViewRow> rows)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            foreach (var row in rows)
                collection.Remove(row);
        }

        public static IEnumerable<T> Select<T>(this DataGridViewRowCollection collection, Func<DataGridViewRow, T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (DataGridViewRow row in collection)
            {
                yield return predicate(row);
            }
        }

        public static IEnumerable<DataGridViewRow> Where(this DataGridViewRowCollection collection, Func<DataGridViewRow, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (DataGridViewRow row in collection)
            {
                if (predicate(row))
                    yield return row;
            }
        }

        public static void RemoveAll(this DataGridViewRowCollection collection, Func<DataGridViewRow, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            collection.RemoveAll(collection.Cast<DataGridViewRow>().Where(predicate));
        }

        public static void RemoveAll(this DataGridViewRowCollection collection, DataGridViewSelectedRowCollection rowCollection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (rowCollection == null)
                throw new ArgumentNullException(nameof(rowCollection));

            foreach (DataGridViewRow row in rowCollection)
                collection.Remove(row);
        }

        public static void AddRange(this DataGridViewRowCollection collection, IEnumerable<DataGridViewRow> rows)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            foreach (var row in rows)
            {
                collection.Add(row);
            }
        }
    }
}
