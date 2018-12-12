using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bot.Builder.Utilities.Markdown.Extensions.Tests
{
    [TestClass]
    [TestCategory("Unit")]
    [TestCategory("MarkDown")]
    public class MarkDownExtensionsTests
    {
        [TestMethod]
        public void GetBoldText_Returns_BoldText()
        {
            // Arrange
            var expected = "**Text**";
            var inputText = "Text";

            // Act
            var actual = inputText.GetBoldText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetItalicsText_Returns_ItalicsText()
        {
            // Arrange
            var expected = "*Text*";
            var inputText = "Text";

            // Act
            var actual = inputText.GetItalicsText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStrikethroughText_Returns_StrikethroughText()
        {
            // Arrange
            var expected = "~~Text~~";
            var inputText = "Text";

            // Act
            var actual = inputText.GetStrikethroughText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetOrderedListText_Returns_OrderedListText()
        {
            // Arrange
            var expected = "1. Text1" + Environment.NewLine +
                "2. Text2" + Environment.NewLine +
                "3. Text3" + Environment.NewLine;

            var inputText = new List<string> { "Text1", "Text2", "Text3" };

            // Act
            var actual = inputText.GetOrderedListText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUnOrderedListText_Returns_UnOrderedListText()
        {
            // Arrange
            var expected = "* Text1" + Environment.NewLine +
                "* Text2" + Environment.NewLine +
                "* Text3" + Environment.NewLine;

            var inputText = new List<string> { "Text1", "Text2", "Text3" };

            // Act
            var actual = inputText.GetUnOrderedListText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPreformattedText_Returns_PreformattedText()
        {
            // Arrange
            var expected = "`Text`";
            var inputText = "Text";

            // Act
            var actual = inputText.GetPreformattedText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetBlockQuotedText_Returns_BlockQuotedText()
        {
            // Arrange
            var expected = "> Text";
            var inputText = "Text";

            // Act
            var actual = inputText.GetBlockQuotedText();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLinkText_Returns_LinkText()
        {
            // Arrange
            var expected = "[DisplayText](UrlText)";

            var displayText = "DisplayText";
            var urlText = "UrlText";

            // Act
            var actual = urlText.GetLinkText(displayText);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetImageLinkText_Returns_ImageLinkText()
        {
            // Arrange
            var expected = "![DisplayText](ImageUrlText)";

            var displayText = "DisplayText";
            var urlText = "ImageUrlText";

            // Act
            var actual = urlText.GetImageLinkText(displayText);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
