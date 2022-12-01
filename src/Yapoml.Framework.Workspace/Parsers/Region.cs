namespace Yapoml.Framework.Workspace.Parsers
{
    public struct Region
    {
        public Region(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public Position Start { get; }

        public Position End { get; }

        public struct Position
        {
            public Position(uint line, uint column)
            {
                Line = line;
                Column = column;
            }

            public uint Line { get; }

            public uint Column { get; }

            public override string ToString()
            {
                return $"({Line}, {Column})";
            }
        }

        public override string ToString()
        {
            return $"Start: {Start}, End: {End}";
        }
    }
}
