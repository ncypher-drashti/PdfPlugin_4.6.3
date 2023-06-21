using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Cypher.Misc.MJSTraders.Infrastructure
{
    public class MJSTradersViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new[] {
                        $"/Plugins/Cypher.MJSTraders/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"/Plugins/Cypher.MJSTraders/Themes/{theme}/Views/Shared/{{0}}.cshtml",

                        $"/Plugins/Cypher.MJSTraders/Views/{{1}}/{{0}}.cshtml",
                        $"/Plugins/Cypher.MJSTraders/Views/Shared/{{0}}.cshtml"
                    }
                    .Concat(viewLocations);
            }

            if (context.AreaName != null)
            {
                if (context.AreaName.Equals(AreaNames.Admin))
                {
                    viewLocations = new[] {
                        $"/Plugins/Cypher.MJSTraders/Areas/Admin/Views/{{1}}/{{0}}.cshtml",
                        $"/Plugins/Cypher.MJSTraders/Areas/Admin/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
                }
            }

            return viewLocations;
        }

        public async void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            var themeContext = (IThemeContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeContext));
            context.Values[THEME_KEY] = await themeContext.GetWorkingThemeNameAsync();
        }
    }
}
