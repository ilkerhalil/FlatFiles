﻿using System;

namespace FlatFiles
{
    /// <summary>
    /// Represents a column containing boolean values.
    /// </summary>
    public class BooleanColumn : ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of a BooleanColumn.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        public BooleanColumn(string columnName)
            : base(columnName)
        {
        }

        /// <summary>
        /// Gets the type of the values in the column.
        /// </summary>
        public override Type ColumnType => typeof(bool);

        /// <summary>
        /// Gets or sets the value representing true.
        /// </summary>
        public string TrueString { get; set; } = Boolean.TrueString;

        /// <summary>
        /// Gets or sets the value representing false.
        /// </summary>
        public string FalseString { get; set; } = Boolean.FalseString;

        /// <summary>
        /// Parses the given value into its equivilent boolean value.
        /// </summary>
        /// <param name="context">Holds information about the column current being processed.</param>
        /// <param name="value">The value to parse.</param>
        /// <returns>True if the value equals the TrueString; otherwise, false.</returns>
        public override object Parse(IColumnContext context, string value)
        {
            if (Preprocessor != null)
            {
                value = Preprocessor(value);
            }
            if (NullHandler.IsNullRepresentation(value))
            {
                return null;
            }
            value = TrimValue(value);
            if (String.Equals(value, TrueString, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            if (String.Equals(value, FalseString, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Formats the given object.
        /// </summary>
        /// <param name="context">Holds information about the column current being processed.</param>
        /// <param name="value">The object to format.</param>
        /// <returns>The formatted value.</returns>
        public override string Format(IColumnContext context, object value)
        {
            if (value == null)
            {
                return NullHandler.GetNullRepresentation();
            }
            bool actual = (bool)value;
            return actual ? TrueString : FalseString;
        }
    }
}
