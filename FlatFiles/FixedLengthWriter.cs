﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace FlatFiles
{
    /// <summary>
    /// Builds textual representations of data by giving each field a fixed width.
    /// </summary>
    public sealed class FixedLengthWriter : IWriter, IWriterWithMetadata
    {
        private readonly FixedLengthRecordWriter recordWriter;
        private bool isSchemaWritten;

        /// <summary>
        /// Initializes a new FixedLengthBuilder with the given schema.
        /// </summary>
        /// <param name="writer">A writer over the fixed-length document.</param>
        /// <param name="schema">The schema of the fixed-length document.</param>
        /// <param name="options">The options used to format the output.</param>
        /// <exception cref="ArgumentNullException">The writer is null.</exception>
        /// <exception cref="ArgumentNullException">The schema is null.</exception>
        public FixedLengthWriter(TextWriter writer, FixedLengthSchema schema, FixedLengthOptions options = null)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            if (options == null)
            {
                options = new FixedLengthOptions();
            }
            recordWriter = new FixedLengthRecordWriter(writer, schema, options);
        }

        /// <summary>
        /// Initializes a new FixedLengthBuilder with the given schema.
        /// </summary>
        /// <param name="writer">A writer over the fixed-length document.</param>
        /// <param name="injector">The schema injector to use to determine the schema.</param>
        /// <param name="options">The options used to format the output.</param>
        /// <exception cref="ArgumentNullException">The writer is null.</exception>
        /// <exception cref="ArgumentNullException">The schema injector is null.</exception>
        public FixedLengthWriter(TextWriter writer, FixedLengthSchemaInjector injector, FixedLengthOptions options = null)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (injector == null)
            {
                throw new ArgumentNullException(nameof(injector));
            }
            if (options == null)
            {
                options = new FixedLengthOptions();
            }
            recordWriter = new FixedLengthRecordWriter(writer, injector, options);
        }

        /// <summary>
        /// Gets the schema used to build the output.
        /// </summary>
        /// <returns>The schema used to build the output.</returns>
        public FixedLengthSchema GetSchema()
        {
            return recordWriter.Metadata.ExecutionContext.Schema;
        }

        ISchema IWriter.GetSchema()
        {
            return GetSchema();
        }

        /// <summary>
        /// Write the textual representation of the record schema to the writer.
        /// </summary>
        /// <remarks>If the header or records have already been written, this call is ignored.</remarks>
        public void WriteSchema()
        {
            if (isSchemaWritten)
            {
                return;
            }
            recordWriter.WriteSchema();
            recordWriter.WriteRecordSeparator();
            ++recordWriter.Metadata.PhysicalRecordNumber;
            isSchemaWritten = true;
        }

        /// <summary>
        /// Write the textual representation of the record schema to the writer.
        /// </summary>
        /// <remarks>If the header or records have already been written, this call is ignored.</remarks>
        public async Task WriteSchemaAsync()
        {
            if (isSchemaWritten)
            {
                return;
            }
            await recordWriter.WriteSchemaAsync().ConfigureAwait(false);
            await recordWriter.WriteRecordSeparatorAsync().ConfigureAwait(false);
            ++recordWriter.Metadata.PhysicalRecordNumber;
            isSchemaWritten = true;
        }

        /// <summary>
        /// Writes the textual representation of the given values to the writer.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <exception cref="ArgumentNullException">The values array is null.</exception>
        public void Write(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (!isSchemaWritten)
            {
                if (recordWriter.Metadata.ExecutionContext.Options.IsFirstRecordHeader)
                {
                    recordWriter.WriteSchema();
                    recordWriter.WriteRecordSeparator();
                    ++recordWriter.Metadata.PhysicalRecordNumber;
                }
                isSchemaWritten = true;
            }
            recordWriter.WriteRecord(values);
            recordWriter.WriteRecordSeparator();
            ++recordWriter.Metadata.PhysicalRecordNumber;
            ++recordWriter.Metadata.LogicalRecordNumber;
        }

        /// <summary>
        /// Writes the textual representation of the given values to the writer.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <exception cref="ArgumentNullException">The values array is null.</exception>
        public async Task WriteAsync(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (!isSchemaWritten)
            {
                if (recordWriter.Metadata.ExecutionContext.Options.IsFirstRecordHeader)
                {
                    await recordWriter.WriteSchemaAsync().ConfigureAwait(false);
                    await recordWriter.WriteRecordSeparatorAsync().ConfigureAwait(false);
                    ++recordWriter.Metadata.PhysicalRecordNumber;
                }
                isSchemaWritten = true;
            }
            await recordWriter.WriteRecordAsync(values).ConfigureAwait(false);
            await recordWriter.WriteRecordSeparatorAsync().ConfigureAwait(false);
            ++recordWriter.Metadata.PhysicalRecordNumber;
            ++recordWriter.Metadata.LogicalRecordNumber;
        }

        IRecordContext IWriterWithMetadata.GetMetadata()
        {
            return recordWriter.Metadata;
        }
    }
}
