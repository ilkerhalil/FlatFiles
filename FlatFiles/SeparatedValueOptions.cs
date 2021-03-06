﻿using System;
using FlatFiles.Properties;

namespace FlatFiles
{
    /// <inheritdoc />
    /// <summary>
    /// Holds configuration options for the SeparatedValueParser.
    /// </summary>
    public sealed class SeparatedValueOptions : IOptions
    {
        private string separator = ",";

        /// <summary>
        /// Initializes a new instance of a SeparatedValueParserOptions.
        /// </summary>
        public SeparatedValueOptions()
        {
        }

        /// <summary>
        /// Gets or sets the character or characters used to separate the columns.
        /// </summary>
        public string Separator
        {
            get => separator;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(Resources.EmptySeparator);
                }
                separator = value;
            }
        }

        /// <summary>
        /// Gets or sets the character or characters used to separate the records.
        /// </summary>
        /// <remarks>
        /// By default, FlatFiles will look a combination of /r, /n, or /r/n. Setting
        /// the record separator to null will enable this default behavior. When writing,
        /// FlatFiles will use Environment.NewLine as the default record separator.
        /// </remarks>
        public string RecordSeparator { get; set; }

        /// <summary>
        /// Gets or sets the character used to quote records containing special characters.
        /// </summary>
        public char Quote { get; set; } = '"';

        /// <summary>
        /// Gets or sets how FlatFiles will handle quoting values.
        /// </summary>
        public QuoteBehavior QuoteBehavior { get; set; } = QuoteBehavior.Default;

        /// <summary>
        /// Gets or sets whether the first record is the schema.
        /// </summary>
        public bool IsFirstRecordSchema { get; set; }

        /// <summary>
        /// Gets or sets whether leading and trailing whitespace should be preserved when reading.
        /// </summary>
        public bool PreserveWhiteSpace { get; set; }

        /// <summary>
        /// Gets whether column-level metadata should be disabled for non-metadata columns.
        /// </summary>
        public bool IsColumnContextDisabled { get; set; }

        /// <summary>
        /// Duplicates the options.
        /// </summary>
        /// <returns>The new options.</returns>
        public SeparatedValueOptions Clone()
        {
            return (SeparatedValueOptions)MemberwiseClone();
        }
    }
}
