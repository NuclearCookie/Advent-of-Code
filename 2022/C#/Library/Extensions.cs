using System.Collections;

namespace Library.Extensions
{
    public static class Extensions
    {
        public static RangeEnumerator GetEnumerator(this Range range)
        {
            if (range.Start.IsFromEnd || range.End.IsFromEnd)
            {
                throw new ArgumentException(nameof(range));
            }

            return new RangeEnumerator(range.Start.Value, range.End.Value);
        }

        public struct RangeEnumerator : IEnumerator<int>
        {
            private readonly int _end;
            private int _current;
            private bool countUp;

            public RangeEnumerator(int start, int end)
            {
                _end = end;
                countUp = end >= start;
                if (countUp)
                {
                    _current = start - 1;
                }
                else
                {
                    _current = start + 1;
                }
            }

            public int Current => _current;
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (countUp)
                {
                    return ++_current < _end;
                }
                else
                {
                    return --_current > _end;
                }
            }

            public void Dispose() { }
            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
