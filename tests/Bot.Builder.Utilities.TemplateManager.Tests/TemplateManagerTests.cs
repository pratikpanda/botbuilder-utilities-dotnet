using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Bot.Builder.Utilities.TemplateManager.Tests
{
    [TestClass]
    [TestCategory("Unit")]
    [TestCategory("Template Manager")]
    public class TemplateManagerTests
    {
        private static TestContext _testContext;
        private static UsagePolicyLanguageTemplateDictionary templates1;
        private static UsagePolicyLanguageTemplateDictionary templates2;

        [AssemblyInitialize]
        public static void SetupDictionaries(TestContext testContext)
        {
            _testContext = testContext;
            templates1 = new UsagePolicyLanguageTemplateDictionary
            {
                ["default"] = new LanguageTemplateDictionary
                {
                    ["default"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"default-default: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)default-default: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"default-default: Yo { data.name}" }
                    },
                    ["en"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"default-en: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)default-en: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"default-en: Yo { data.name}" }
                    },
                    ["fr"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"default-fr: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)default-fr: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"default-fr: Yo { data.name}" }
                    }
                },
                ["policy1"] = new LanguageTemplateDictionary
                {
                    ["default"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy1-default: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy1-default: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy1-default: Yo { data.name}" }
                    },
                    ["en"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy1-en: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy1-en: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy1-en: Yo { data.name}" }
                    },
                    ["fr"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy1-fr: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy1-fr: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy1-fr: Yo { data.name}" }
                    }
                },
                ["policy2"] = new LanguageTemplateDictionary
                {
                    ["default"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy2-default: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy2-default: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy2-default: Yo { data.name}" }
                    },
                    ["en"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy2-en: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy2-en: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy2-en: Yo { data.name}" }
                    },
                    ["fr"] = new TemplateIdMap
                    {
                        { "stringTemplate", (context, data) => $"policy2-fr: { data.name}" },
                        { "activityTemplate", (context, data) => { return new Activity() { Type = ActivityTypes.Message, Text = $"(Activity)policy2-fr: { data.name}" }; } },
                        { "stringTemplate2", (context, data) => $"policy2-fr: Yo { data.name}" }
                    }
                }
            };

            templates2 = new UsagePolicyLanguageTemplateDictionary
            {
                ["policy1"] = new LanguageTemplateDictionary
                {
                    ["en"] = new TemplateIdMap
                    {
                        { "stringTemplate2", (context, data) => $"policy1-en: StringTemplate2 override {data.name}" }
                    }
                }
            };
        }

        [TestMethod]
        public void TemplateManager_Registration()
        {
            var templateManager = new TemplateManager();
            Assert.AreEqual(templateManager.List().Count, 0, "nothing registered yet");

            var templateEngine1 = new DictionaryRenderer(templates1);
            var templateEngine2 = new DictionaryRenderer(templates2);

            templateManager.Register(templateEngine1);
            Assert.AreEqual(templateManager.List().Count, 1, "one registered");

            templateManager.Register(templateEngine2);
            Assert.AreEqual(templateManager.List().Count, 2, "two registered");
        }

        [TestMethod]
        public void TemplateManager_MultiTemplate()
        {
            var templateManager = new TemplateManager();
            Assert.AreEqual(templateManager.List().Count, 0, "nothing registered yet");
            var templateEngine1 = new DictionaryRenderer(templates1);
            var templateEngine2 = new DictionaryRenderer(templates2);

            templateManager.Register(templateEngine1);
            Assert.AreEqual(templateManager.List().Count, 1, "one registered");

            templateManager.Register(templateEngine1);
            Assert.AreEqual(templateManager.List().Count, 1, "only  one registered");

            templateManager.Register(templateEngine2);
            Assert.AreEqual(templateManager.List().Count, 2, "two registered");
        }

        [TestMethod]
        public async Task DictionaryTemplateEngine_SimpleStringBinding()
        {
            var engine = new DictionaryRenderer(templates1);
            var result = await engine.RenderTemplate(null, "policy1", "en", "stringTemplate", new { name = "joe" });
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("policy1-en: joe", (string)result);
        }

        [TestMethod]
        public async Task DictionaryTemplateEngine_SimpleActivityBinding()
        {
            var engine = new DictionaryRenderer(templates1);
            var result = await engine.RenderTemplate(null, "policy1", "en", "activityTemplate", new { name = "joe" });
            Assert.IsInstanceOfType(result, typeof(Activity));
            var activity = result as Activity;
            Assert.AreEqual(ActivityTypes.Message, activity.Type);
            Assert.AreEqual("(Activity)policy1-en: joe", activity.Text);
        }

        [TestMethod]
        public async Task TemplateManager_defaultPolicy_defaultLookup()
        {

            TestAdapter adapter = new TestAdapter();

            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "default", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("default-default: joe")
                .Send("activityTemplate").AssertReply("(Activity)default-default: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_defaultPolicy_enLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "en"; // force to english
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "default", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("default-en: joe")
                .Send("activityTemplate").AssertReply("(Activity)default-en: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_defaultPolicy_frLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "fr"; // force to french
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "default", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("default-fr: joe")
                .Send("activityTemplate").AssertReply("(Activity)default-fr: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy1Policy_defaultLookup()
        {
            TestAdapter adapter = new TestAdapter();

            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy1", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy1-default: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy1-default: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy1Policy_enLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "en"; // force to english
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy1", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy1-en: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy1-en: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy1Policy_frLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "fr"; // force to french
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy1", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy1-fr: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy1-fr: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy2Policy_defaultLookup()
        {
            TestAdapter adapter = new TestAdapter();

            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy2", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy2-default: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy2-default: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy2Policy_enLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "en"; // force to english
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy2", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy2-en: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy2-en: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_policy2Policy_frLookup()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "fr"; // force to french
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "policy2", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("policy2-fr: joe")
                .Send("activityTemplate").AssertReply("(Activity)policy2-fr: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_Override()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                context.Activity.AsMessageActivity().Locale = "fr"; // force to french
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "default", templateId, new { name = "joe" });
            })
                .Send("stringTemplate2").AssertReply("default-fr: Yo joe")
                .Send("activityTemplate").AssertReply("(Activity)default-fr: joe")
                .StartTestAsync();
        }

        [TestMethod]
        public async Task TemplateManager_UseTemplateEngine()
        {
            TestAdapter adapter = new TestAdapter();
            var templateManager = new TemplateManager()
                .Register(new DictionaryRenderer(templates1))
                .Register(new DictionaryRenderer(templates2));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var templateId = context.Activity.AsMessageActivity().Text.Trim();
                await templateManager.ReplyWith(context, "default", templateId, new { name = "joe" });
            })
                .Send("stringTemplate").AssertReply("default-default: joe")
                .Send("activityTemplate").AssertReply("(Activity)default-default: joe")
                .StartTestAsync();
        }
    }
}
