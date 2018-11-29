using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Builder.Utilities.TemplateManager.Interfaces
{
    /// <summary>
    /// Defines the interface for binding data to template and rendering a string.
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// render a template to an activity or string
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="usage">The usage preference used to render.</param>
        /// <param name="language">The language used to render.</param>
        /// <param name="templateId">The template to render.</param>
        /// <param name="data">The data object to use to render.</param>
        /// <returns></returns>
        Task<object> RenderTemplate(ITurnContext turnContext, string usage, string language, string templateId, object data);
    }
}
