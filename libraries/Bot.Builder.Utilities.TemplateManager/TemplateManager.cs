using Bot.Builder.Utilities.TemplateManager.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Builder.Utilities.TemplateManager
{
    /// <summary>
    /// TemplateManager manages set of ITemplateRenderer implementations
    /// </summary>
    /// <remarks>
    /// ITemplateRenderer implements 
    /// </remarks>
    public class TemplateManager
    {
        // Constants
        public const string DefaultUsagePolicy = "default";
        public const string DefaultLanguage = "default";

        private List<ITemplateRenderer> templateRenderers = new List<ITemplateRenderer>();
        private List<string> languageFallback = new List<string>();
        private List<string> usagePolicyFallback = new List<string>();

        public TemplateManager()
        {
        }

        /// <summary>
        /// Add a template engine for binding templates
        /// </summary>
        /// <param name="renderer"></param>

        public TemplateManager Register(ITemplateRenderer renderer)
        {
            if (!templateRenderers.Contains(renderer))
                templateRenderers.Add(renderer);
            return this;
        }

        /// <summary>
        /// List registered template engines
        /// </summary>
        /// <returns></returns>
        public IList<ITemplateRenderer> List()
        {
            return templateRenderers;
        }

        public void SetLanguagePolicy(IEnumerable<string> languageFallback)
        {
            languageFallback = new List<string>(languageFallback);
        }

        public void SetUsagePolicy(IEnumerable<string> usagePolicies)
        {
            usagePolicyFallback = new List<string>(usagePolicies);
        }

        public IEnumerable<string> GetLanguagePolicy()
        {
            return languageFallback;
        }

        public IEnumerable<string> GetUsagePolicy()
        {
            return usagePolicyFallback;
        }

        /// <summary>
        /// Send a reply with the template
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="templateId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ReplyWith(ITurnContext turnContext, string usagePolicy, string templateId, object data = null)
        {
            BotAssert.ContextNotNull(turnContext);

            // apply template
            Activity boundActivity = await RenderTemplate(turnContext, usagePolicy, turnContext.Activity?.AsMessageActivity()?.Locale, templateId, data).ConfigureAwait(false);
            if (boundActivity != null)
            {
                await turnContext.SendActivityAsync(boundActivity);
                return;
            }
            return;
        }


        /// <summary>
        /// Render the template
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="language"></param>
        /// <param name="templateId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Activity> RenderTemplate(ITurnContext turnContext, string usagePolicy, string language, string templateId, object data = null)
        {
            var fallbackLocales = new List<string>(languageFallback);
            var fallbackUsagePolicies = new List<string>(usagePolicyFallback);

            if (!string.IsNullOrEmpty(language))
            {
                fallbackLocales.Add(language);
            }

            if (!string.IsNullOrEmpty(usagePolicy))
            {
                fallbackUsagePolicies.Add(usagePolicy);
            }

            fallbackLocales.Add("default");
            fallbackUsagePolicies.Add("default");

            // try each usage policy until successful
            foreach (var uPolicy in fallbackUsagePolicies)
            {
                // try each locale until successful
                foreach (var locale in fallbackLocales)
                {
                    foreach (var renderer in templateRenderers)
                    {
                        object templateOutput = await renderer.RenderTemplate(turnContext, uPolicy, locale, templateId, data);

                        if (templateOutput != null)
                        {
                            if (templateOutput is string)
                            {
                                return new Activity(type: ActivityTypes.Message, text: (string)templateOutput);
                            }
                            else
                            {
                                return templateOutput as Activity;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Render the template string
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="language"></param>
        /// <param name="templateId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> RenderTemplateString(ITurnContext turnContext, string usagePolicy, string language, string templateId, object data = null)
        {
            var fallbackLocales = new List<string>(languageFallback);
            var fallbackUsagePolicies = new List<string>(usagePolicyFallback);

            if (!string.IsNullOrEmpty(language))
            {
                fallbackLocales.Add(language);
            }

            if (!string.IsNullOrEmpty(usagePolicy))
            {
                fallbackUsagePolicies.Add(usagePolicy);
            }

            fallbackLocales.Add("default");
            fallbackUsagePolicies.Add("default");

            // try each usage policy until successful
            foreach (var uPolicy in fallbackUsagePolicies)
            {
                // try each locale until successful
                foreach (var locale in fallbackLocales)
                {
                    foreach (var renderer in templateRenderers)
                    {
                        object templateOutput = await renderer.RenderTemplate(turnContext, uPolicy, locale, templateId, data);

                        if (templateOutput != null)
                        {
                            if (templateOutput is string)
                            {
                                return (string)templateOutput;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
