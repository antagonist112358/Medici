using System;
using System.Collections.Generic;
using System.Text;
using Medici.IO;

namespace Medici
{
    public static class ColorConsole
    {
        private static object syncLock = new object();

        public static void NewLine() { Console.WriteLine(); }

        public static void WriteBar(ConsoleColor? color = null)
        {
            StringBuilder sb = new StringBuilder();
            int consoleLength = Console.WindowWidth;
            sb.Append('=', consoleLength - 1);

            if (color == null)
                Console.WriteLine(sb.ToString());
            else
                Write(color.Value, sb.ToString());
        }

        public static void Write(string message) { WriteConsoleInternal(false, message, null, null); }
        public static void Write(string message, params object[] args) { WriteConsoleInternal(false, message, null, args); }

        public static void Write(ConsoleColor color, string message) { WriteConsoleInternal(false, message, color, null); }
        public static void Write(ConsoleColor color, string message, params object[] args) { WriteConsoleInternal(false, message, color, args); }

        public static void WriteLine(string message) { WriteConsoleInternal(true, message, null, null); }
        public static void WriteLine(string message, params object[] args) { WriteConsoleInternal(true, message, null, args); }

        public static void WriteLine(ConsoleColor color, string message) { WriteConsoleInternal(true, message, color, null); }
        public static void WriteLine(ConsoleColor color, string message, params object[] args) { WriteConsoleInternal(true, message, color, null); }

        private static void WriteConsoleInternal(bool useNewline, string message, ConsoleColor? messageColor, params object[] args)
        {
            lock (syncLock)
            {
                ConsoleColor prevColor = Console.ForegroundColor;

                if (messageColor != null)
                {
                    Console.ForegroundColor = messageColor.Value;
                }

                string outMessage = (args != null) ? String.Format(message, args) : message;

                if (useNewline)
                {
                    ColorizedWrite(outMessage, Environment.NewLine);
                }
                else
                {
                    ColorizedWrite(outMessage, null);
                }

                if (messageColor != null)
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }

        private static void ColorizedWrite(string message, string endingChar)
        {
            // Last character to write
            string lastPiece = endingChar ?? String.Empty;

            // Get the default color
            ConsoleColor defaultColor = Console.ForegroundColor;

            // The word color stack
            Stack<ConsoleColor> blockColors = new Stack<ConsoleColor>();

            // Character stream
            CharStream messageCharacters = new CharStream(message);

            // Next string
            string next = null;

            // Until we have read the entire stream
            while (!messageCharacters.EndOfStream)
            {
                // Is the next character a left bracket?
                if (messageCharacters.Peek() == '<')
                {
                    // Read until a right bracket (or end of string)
                    next = messageCharacters.ReadUntilInclusive('>');
                }
                else
                {
                    // Get the next string of characters
                    next = messageCharacters.ReadUntilExclusive('<');
                }

                // Is the string a color tag?
                if (next.StartsWith("<") && next.EndsWith(">"))
                {
                    // Is it a closing tag?
                    bool isClosing = (next[1] == '/') ? true : false;

                    // Get the color name
                    string colorName = next.TrimEnd('>').TrimStart('<').TrimStart('/').Trim();

                    // Check for a valid color name
                    try
                    {
                        // Try to parse the color name
                        ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName, true);

                        // If this is a closing tag
                        if (isClosing)
                        {
                            // Pop the current color off the stack
                            var topColor = blockColors.Pop();

                            // Make sure they match
                            if (color != topColor)
                                blockColors.Push(topColor);
                        }
                        // It is an opening tag
                        else
                            blockColors.Push(color);
                    }
                    catch
                    {
                        // Write the text
                        Console.Write(next);
                    }

                } // <-- End Color-tag
                else
                {
                    if (blockColors.Count == 0)
                    {
                        // If the stack is empty, use the default color
                        Console.ForegroundColor = defaultColor;
                    }
                    else
                    {
                        // Use the top color in the stack
                        Console.ForegroundColor = blockColors.Peek();
                    }

                    // Write the text
                    Console.Write(next);
                }

            } // <-- End While

            // Reset the foreground color
            Console.ForegroundColor = defaultColor;

            // Write the last character
            Console.Write(lastPiece);
        }
    }
}
