# Bot.Framework.Utilities.Markdown.Extensions
Markdown syntax generator for bots created using Microsoft Bot Framework.

## Usage
The component provides extension methods on strings and collection of strings to generate markdown syntax based on the bot builder markdown guidelines.
Please refer to the code snippets below to understand how to use this component:

```csharp
// Make sure you are referencing Bot.Builder.Utilities.Markdown.Extensions.

// Bold
var text = "This text should be in bold.";
var boldText = text.GetBoldText();

// Italics
var text = "This text should be in italics.";
var italicsText = text.GetItalicsText();

// Ordered List
var text = new List<string> 
{
  "Ordered list item 1.",
  "Ordered list item 2.",
  "Ordered list item 3."
};
var orderedListText = text.GetOrderedListText();

// Unordered List
var text = new List<string> 
{
  "Unordered list item 1.",
  "Unordered list item 2.",
  "Unordered list item 3."
};
var unorderedListText = text.GetUnorderedListText();

// Link
var text = "https://www.microsoft.com/en-in";
var displayText = "Microsoft";
var linkText = text.GetLinkText(displayText);
```
