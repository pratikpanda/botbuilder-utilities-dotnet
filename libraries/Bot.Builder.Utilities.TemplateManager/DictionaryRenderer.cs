using Bot.Builder.Utilities.TemplateManager.Interfaces;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Builder.Utilities.TemplateManager
{
    /// <summary>
    /// Map of Template Ids-> Template Function()
    /// </summary>
    public class TemplateIdMap : Dictionary<string, Func<ITurnContext, dynamic, object>>
    {
    }

    /// <summary>
    /// Map of language -> template functions
    /// </summary>
    public class LanguageTemplateDictionary : Dictionary<string, TemplateIdMap>
    {
    }

    /// <summary>
    /// Map of usage policy -> language template functions
    /// </summary>
    public class UsagePolicyLanguageTemplateDictionary : Dictionary<string, LanguageTemplateDictionary>
    {
    }

    /// <summary>
    /// This is a simple template engine which has a resource map of template functions
    /// To use, simply register with templateManager
    /// templateManager.Register(new DictionaryRenderer(usagePolicies))
    /// </summary>
    public class DictionaryRenderer : ITemplateRenderer
    {
        private UsagePolicyLanguageTemplateDictionary usagePolicies;

        public DictionaryRenderer(UsagePolicyLanguageTemplateDictionary usagePolicies)
        {
            this.usagePolicies = usagePolicies;
        }

        public Task<object> RenderTemplate(ITurnContext turnContext, string usage, string language, string templateId, object data)
        {
            if (usagePolicies.TryGetValue(usage, out var languageTemplateDictionary))
            {
                if (languageTemplateDictionary.TryGetValue(language, out var templates))
                {
                    if (templates.TryGetValue(templateId, out var template))
                    {
                        dynamic result = template(turnContext, data);
                        if (result != null)
                        {
                            return Task.FromResult(result as object);
                        }
                    }
                }
            }

            return Task.FromResult((object)null);
        }
    }
}
