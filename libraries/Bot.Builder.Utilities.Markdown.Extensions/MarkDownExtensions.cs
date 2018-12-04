using System;
using System.Collections.Generic;

namespace Bot.Builder.Utilities.Markdown.Extensions
{
    public static class MarkDownExtensions
    {
        public static string GetBoldText(this string text)
        {
            text = $"**{text}**";
            return text;
        }

        public static string GetItalicsText(this string text)
        {
            text = $"*{text}*";
            return text;
        }

        public static string GetOrderedListText(this List<string> text)
        {
            int listSequenceNumber = 1;
            string orderedListText = string.Empty;

            foreach (var listItem in text)
            {
                orderedListText += $"{listSequenceNumber}. {listItem}" + Environment.NewLine;
                listSequenceNumber++;
            }

            return orderedListText;
        }

        public static string GetUnOrderedListText(this List<string> text)
        {
            string unOrderedListText = string.Empty;

            foreach (var listItem in text)
            {
                unOrderedListText += $"* {listItem}" + Environment.NewLine;
            }

            return unOrderedListText;
        }

        public static string GetLinkText(this string url, string displayText)
        {
            string linkText = string.Empty;
            linkText = $"[{displayText}]({url})";
            return linkText;
        }
    }
}
