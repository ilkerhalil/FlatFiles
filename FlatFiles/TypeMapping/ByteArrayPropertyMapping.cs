﻿using System;
using System.Text;

namespace FlatFiles.TypeMapping
{
    /// <summary>
    /// Represents the mapping from a type property to a byte array column.
    /// </summary>
    public interface IByteArrayPropertyMapping
    {
        /// <summary>
        /// Sets the name of the column in the input or output file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping ColumnName(string name);

        /// <summary>
        /// Sets the encoding to use to read and write the column.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping Encoding(Encoding encoding);

        /// <summary>
        /// Sets the value to treat as null.
        /// </summary>
        /// <param name="value">The value to treat as null.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping NullValue(string value);

        /// <summary>
        /// Sets a custom handler for nulls.
        /// </summary>
        /// <param name="handler">The handler to use to recognize nulls.</param>
        /// <returns>The property mapping for further configuration.</returns>
        /// <remarks>Setting the handler to null with use the default handler.</remarks>
        IByteArrayPropertyMapping NullHandler(INullHandler handler);

        /// <summary>
        /// Sets a function to preprocess in the input before parsing it.
        /// </summary>
        /// <param name="preprocessor">A preprocessor function.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping Preprocessor(Func<string, string> preprocessor);
    }

    internal sealed class ByteArrayPropertyMapping : IByteArrayPropertyMapping, IMemberMapping
    {
        private readonly ByteArrayColumn column;

        public ByteArrayPropertyMapping(ByteArrayColumn column, IMemberAccessor member, int physicalIndex, int logicalIndex)
        {
            this.column = column;
            Member = member;
            PhysicalIndex = physicalIndex;
            LogicalIndex = logicalIndex;
        }

        public IByteArrayPropertyMapping ColumnName(string name)
        {
            column.ColumnName = name;
            return this;
        }

        public IByteArrayPropertyMapping Encoding(Encoding encoding)
        {
            column.Encoding = encoding;
            return this;
        }

        public IByteArrayPropertyMapping NullValue(string value)
        {
            column.NullHandler = new ConstantNullHandler(value);
            return this;
        }

        public IByteArrayPropertyMapping NullHandler(INullHandler handler)
        {
            column.NullHandler = handler;
            return this;
        }

        public IByteArrayPropertyMapping Preprocessor(Func<string, string> preprocessor)
        {
            column.Preprocessor = preprocessor;
            return this;
        }

        public IMemberAccessor Member { get; }

        public Action<IColumnContext, object, object> Reader => null;

        public Action<IColumnContext, object, object[]> Writer => null;

        public IColumnDefinition ColumnDefinition => column;

        public int PhysicalIndex { get; }

        public int LogicalIndex { get; }
    }
}
