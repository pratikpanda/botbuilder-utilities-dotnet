﻿# Bot.Framework.Utilities.TemplateManager
A template manager for generating conversation message for bots created using Microsoft Bot Framework.

## Usage
The component allows the bot developer to store conversation messages in resource files and create custom template manager to generate text from the resource files based on language and usage policy.
Please refer to the code snippets below to understand how to use create and component.

To create a custom template manager:

```csharp
// This example uses SampleDialog as an example dialog and SampleResponses as the associated template manager for the dialog.
// Inside sample dialog create SampleResponses as a static field and initialize it using field initializer syntax.
public static SampleResponses Responder = new SampleResponses();

// SampleResponses.cs
public class RootResponses : TemplateManager
{
    // Constants
    public const string SampleConstant1 = "SampleConstant1";
    public const string SampleConstant2 = "SampleConstant2";

    // There are two usage policies named internal and customer.
    // If the usage policy is not specified at the time of retrieving the text using the template, the default is used.
    // The key for UsagePolicyLanguageTemplateDictionary signifies the name of the usage policy.
    // The key for LanguageTemplateDictionary signifies the name of the language.
    private static UsagePolicyLanguageTemplateDictionary responseTemplates = new UsagePolicyLanguageTemplateDictionary
    {
        ["default"] = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                //
                { SampleConstant1, (context, data) => RootStrings.SAMPLE1 },
                { SampleConstant2, (context, data) => RootStrings.SAMPLE2 }
            }
        },
        ["internal"] = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                { SampleConstant1, (context, data) => RootInternalStrings.SAMPLE1 },
                { SampleConstant2, (context, data) => RootInternalStrings.SAMPLE2 }
            },
            ["fr"] = new TemplateIdMap
            {
                { SampleConstant1, (context, data) => RootInternalFrStrings.SAMPLE1 },
                { SampleConstant2, (context, data) => RootInternalFrStrings.SAMPLE2 }
            }
        },
        ["customer"] = new LanguageTemplateDictionary
        {
            ["de"] = new TemplateIdMap
            {
                { SampleConstant1, (context, data) => RootCustomerDeStrings.SAMPLE1 },
                { SampleConstant2, (context, data) => RootCustomerDeStrings.SAMPLE2 }
            }
        }
    };

    public RootResponses()
    {
        Register(new DictionaryRenderer(responseTemplates));
    }
}
```

To use the custom template manager:

```csharp
// To send an activity using the template manager.
await Responder.ReplyWith(dialogContext.Context, "usage-policy-name", SampleResponses.SampleConstant1);

// To render the activity using the template manager.
await Responder.RenderTemplate(dialogContext.Context, "usage-policy-name", "language", SampleResponses.SampleConstant1);

// To render the string using the template manager.
await Responder.RenderTemplateString(dialogContext.Context, "usage-policy-name", "language", SampleResponses.SampleConstant1);
```
