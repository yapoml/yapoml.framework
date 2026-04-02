namespace Yapoml.Framework.Workspace.Parsers;

/// <summary>
/// Represents a text region in a source file, defined by a start and end position.
/// </summary>
public struct Region
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> struct.
    /// </summary>
    /// <param name="start">The start position of the region.</param>
    /// <param name="end">The end position of the region.</param>
    public Region(Position start, Position end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Gets the start position of the region.
    /// </summary>
    public Position Start { get; }

    /// <summary>
    /// Gets the end position of the region.
    /// </summary>
    public Position End { get; }

    /// <summary>
    /// Represents a position within a source file as a line and column number.
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> struct.
        /// </summary>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        public Position(uint line, uint column)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        public uint Line { get; }

        /// <summary>
        /// Gets the column number.
        /// </summary>
        public uint Column { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Line}, {Column})";
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Start: {Start}, End: {End}";
    }
}
