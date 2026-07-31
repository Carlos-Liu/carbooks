using System.Text;

namespace CarBooks.Database.Ef.Seeding;

/// <summary>
/// Builds the locally stored cover artwork used by the seed data. SVG keeps the payload tiny and
/// text-based, so the sample catalog renders without shipping binary assets or reaching the
/// internet.
/// </summary>
internal static class PlaceholderCover
{
    public const string ContentType = "image/svg+xml";

    private const int Width = 240;
    private const int Height = 320;
    private const int MaxCharactersPerLine = 16;
    private const int MaxTitleLines = 4;

    public static byte[] Create(string title, string author, string topColor, string bottomColor)
    {
        var lines = Wrap(title, MaxCharactersPerLine).Take(MaxTitleLines).ToArray();
        var firstLineY = 148 - ((lines.Length - 1) * 14);

        var svg = new StringBuilder();
        svg.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"").Append(Width)
           .Append("\" height=\"").Append(Height)
           .Append("\" viewBox=\"0 0 ").Append(Width).Append(' ').Append(Height).Append("\">");
        svg.Append("<defs><linearGradient id=\"bg\" x1=\"0\" y1=\"0\" x2=\"0\" y2=\"1\">")
           .Append("<stop offset=\"0%\" stop-color=\"").Append(topColor).Append("\"/>")
           .Append("<stop offset=\"100%\" stop-color=\"").Append(bottomColor).Append("\"/>")
           .Append("</linearGradient></defs>");
        svg.Append("<rect width=\"").Append(Width).Append("\" height=\"").Append(Height).Append("\" fill=\"url(#bg)\"/>");
        svg.Append("<rect x=\"12\" y=\"12\" width=\"").Append(Width - 24).Append("\" height=\"").Append(Height - 24)
           .Append("\" fill=\"none\" stroke=\"#ffffff\" stroke-opacity=\"0.35\" stroke-width=\"2\"/>");

        svg.Append("<text x=\"120\" text-anchor=\"middle\" fill=\"#ffffff\" ")
           .Append("font-family=\"Segoe UI, Helvetica, Arial, sans-serif\" font-size=\"20\" font-weight=\"600\">");
        for (var index = 0; index < lines.Length; index++)
        {
            svg.Append("<tspan x=\"120\" y=\"").Append(firstLineY + (index * 28)).Append("\">")
               .Append(Escape(lines[index]))
               .Append("</tspan>");
        }

        svg.Append("</text>");

        svg.Append("<text x=\"120\" y=\"").Append(Height - 44)
           .Append("\" text-anchor=\"middle\" fill=\"#ffffff\" fill-opacity=\"0.78\" ")
           .Append("font-family=\"Segoe UI, Helvetica, Arial, sans-serif\" font-size=\"14\">")
           .Append(Escape(author))
           .Append("</text>");

        svg.Append("</svg>");

        return Encoding.UTF8.GetBytes(svg.ToString());
    }

    private static IEnumerable<string> Wrap(string text, int maxCharactersPerLine)
    {
        var line = new StringBuilder();
        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Length > 0 && line.Length + 1 + word.Length > maxCharactersPerLine)
            {
                yield return line.ToString();
                line.Clear();
            }

            if (line.Length > 0)
            {
                line.Append(' ');
            }

            line.Append(word);
        }

        if (line.Length > 0)
        {
            yield return line.ToString();
        }
    }

    private static string Escape(string value) =>
        value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
