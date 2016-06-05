﻿using System;
using System.Text;

namespace FlatFiles
{
    /// <summary>
    /// Represents a column containing strings.
    /// </summary>
    public class StringColumn : ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of a StringColumnDefinition.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        public StringColumn(string columnName)
            : base(columnName)
        {
            Trim = true;
        }

        /// <summary>
        /// Gets the type of the values in the column.
        /// </summary>
        public override Type ColumnType
        {
            get { return typeof(String); }
        }

        /// <summary>
        /// Gets or sets whether the value should be trimmed.
        /// </summary>
        public bool Trim { get; set; }

        /// <summary>
        /// Returns the given value trimmed.
        /// </summary>
        /// <param name="value">The value to trim.</param>
        /// <param name="encoding">The encoding of the outer document.</param>
        /// <returns>The value trimmed.</returns>
        public override object Parse(string value, Encoding encoding)
        {
            if (Trim && value != null)
            {
                value = TrimValue(value);
            }
            if (NullHandler.IsNullRepresentation(value))
            {
                return null;
            }
            return value;
        }

        /// <summary>
        /// Formats the given object.
        /// </summary>
        /// <param name="value">The object to format.</param>
        /// <param name="encoding">The encoding of the outer document.</param>
        /// <returns>The formatted value.</returns>
        public override string Format(object value, Encoding encoding)
        {
            if (value == null)
            {
                return NullHandler.GetNullRepresentation();
            }
            string actual = (string)value;
            return actual;
        }
    }
}
