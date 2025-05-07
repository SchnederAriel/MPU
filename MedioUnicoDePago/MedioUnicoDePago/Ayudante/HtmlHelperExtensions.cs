using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedioUnicoDePago
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString EnsureSection(this HtmlHelper helper)
        {
            var wp = (WebViewPage)helper.ViewDataContainer;
            Dictionary<string, System.Web.WebPages.SectionWriter> sw = (Dictionary<string, System.Web.WebPages.SectionWriter>)typeof(System.Web.WebPages.WebPageBase).GetProperty("SectionWriters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(wp);
            if (!helper.ViewContext.HttpContext.Items.Contains("SectionWriter"))
            {
                Dictionary<string, Stack<System.Web.WebPages.SectionWriter>> qss = new Dictionary<string, Stack<System.Web.WebPages.SectionWriter>>();
                helper.ViewContext.HttpContext.Items["SectionWriter"] = qss;
            }
            var eqs = (Dictionary<string, Stack<System.Web.WebPages.SectionWriter>>)helper.ViewContext.HttpContext.Items["SectionWriter"];
            foreach (var kp in sw)
            {
                if (!eqs.ContainsKey(kp.Key)) eqs[kp.Key] = new Stack<System.Web.WebPages.SectionWriter>();
                eqs[kp.Key].Push(kp.Value);
            }
            return MvcHtmlString.Create("");
        }

        public static MvcHtmlString RenderSectionEx(this HtmlHelper helper, string section, bool required = false)
        {
            if (helper.ViewContext.HttpContext.Items.Contains("SectionWriter"))
            {
                Dictionary<string, Stack<System.Web.WebPages.SectionWriter>> qss = (Dictionary<string, Stack<System.Web.WebPages.SectionWriter>>)helper.ViewContext.HttpContext.Items["SectionWriter"];
                if (qss.ContainsKey(section))
                {
                    var wp = (WebViewPage)helper.ViewDataContainer;
                    var qs = qss[section];
                    while (qs.Count > 0)
                    {
                        var sw = qs.Pop();
                        var os = ((WebViewPage)sw.Target).OutputStack;
                        if (os.Count == 0) os.Push(wp.Output);
                        sw.Invoke();
                    }
                }
                else if (!qss.ContainsKey(section) && required)
                {
                    throw new Exception(string.Format("'{0}' section is not defined.", section));
                }
            }
            return MvcHtmlString.Create("");
        }

        public static string CurrentViewName(this HtmlHelper html)
        {
            return System.IO.Path.GetFileNameWithoutExtension(
                ((RazorView)html.ViewContext.View).ViewPath
            );
        }

        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

    }
}