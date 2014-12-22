namespace Medici
{
    internal static class CharArrayExtensions
    {
        public static byte[] ToByteArray(this char[] source)
        {
            Ensure.That(source, "source").IsNotNull();

            return System.Text.Encoding.UTF8.GetBytes(source);
        }

        public static char[] ToCharArray(this byte[] source)
        {
            Ensure.That(source, "source").IsNotNull();

            return System.Text.Encoding.UTF8.GetChars(source);
        }
    }
}
