using System;
using System.Windows.Markup;

namespace HauntedHunch
{
    public abstract class BaseMarkupExtension<TMarkupExtension> : MarkupExtension
        where TMarkupExtension : BaseMarkupExtension<TMarkupExtension>, new()
    {
        static TMarkupExtension Instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance ?? (Instance = new TMarkupExtension());
        }
    }
}
