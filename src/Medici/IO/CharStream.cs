using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medici.IO
{
    public class CharStream
    {
        private readonly char[] _buffer;
        private int _index = 0;

        public CharStream(string data)
        {
            _buffer = data.ToCharArray();
        }

        public CharStream(IEnumerable<char> data)
        {
            _buffer = data.ToArray();
        }

        public char Peek()
        {
            return _buffer[_index];
        }

        public char Next
        {
            get { return _buffer[_index++]; }
        }

        public bool EndOfStream
        {
            get { return (_index >= _buffer.Length); }
        }

        public string ReadUntilExclusive(char stoppingChar, int maxCharacters = 2048)
        {
            var charBuffer = new List<char>(maxCharacters);

            while (!EndOfStream && charBuffer.Count < maxCharacters)
            {
                char nextChar = _buffer[_index];

                if (nextChar == stoppingChar)
                    break;

                charBuffer.Add(nextChar);
                _index++;
            }

            return new String(charBuffer.ToArray());
        }

        public string ReadUntilInclusive(char stoppingChar, int maxCharacters = 2048)
        {
            var charBuffer = new List<char>(maxCharacters);

            while (!EndOfStream && charBuffer.Count < maxCharacters)
            {
                char nextChar = _buffer[_index];

                charBuffer.Add(nextChar);
                _index++;

                if (nextChar == stoppingChar)
                    break;
            }

            return new String(charBuffer.ToArray());
        }
    }
}
