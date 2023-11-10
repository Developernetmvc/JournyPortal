using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetShopeWeb.ViewModel
{
    public static class HtmlHelpers
    {
        //private static readonly AppConfiguration configuration;

        //static HtmlHelpers()
        //{
        //    configuration = new AppConfiguration();
        //}

        public static string GetText(this string data, string emptyText = "-")
        {
            if (string.IsNullOrWhiteSpace(data))
                return emptyText;
            else
                return data.ToString();
        }

        public static string GetText(this DateTime? data, string dateFormat = "dd-MMM-yyyy", string emptyText = "-")
        {
            if (!data.HasValue)
                return emptyText;
            else
                return data.Value.ToString(dateFormat);
        }


        //public static IHtmlString JsonOf<T>(this HtmlHelper helper, T data)
        //{
        //    if (data == null)
        //        return new HtmlString("null");

        //    var serializeSettings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml };
        //    return helper.Raw(JsonConvert.SerializeObject(data, serializeSettings));

        //}

        public static IHtmlString JsonOf<T>(this HtmlHelper helper, T data)
        {
            if (data == null)
                return new HtmlString("null");

            var serializeSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Add this line
            };
            return helper.Raw(JsonConvert.SerializeObject(data, serializeSettings));
        }


        public static IHtmlString JsonOf(this HtmlHelper helper, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return helper.Raw("null");
            }

            try
            {
                var serializeSettings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml };
                var json = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(data, serializeSettings));
                return helper.Raw(json);
            }
            catch (Exception)
            {
                return helper.Raw("null");
            }
        }

     

        public static IHtmlString JsonOf(this HtmlHelper helper, List<SelectListItem> items)
        {
            var data = items?.Select(x => new { text = x.Text, value = x.Value });
            return helper.JsonOf(data);
        }


        private static KeyValuePair<string, object> ToVueKeyValue(this KeyValuePair<string, object> item)
        {
            var key = item.Key;
            var value = item.Value;

            key = key
                .Replace("___", ".")
                .Replace("__", ":")
                .Replace("_", "-");

            var valueStr = (value as string);
            if (!string.IsNullOrWhiteSpace(valueStr))
            {
                var vueAttrs = new Dictionary<string, string>();
                vueAttrs.Add(".lazy:", ".lazy");
                vueAttrs.Add(".number:", ".number");
                vueAttrs.Add(".trim:", ".trim");

                foreach (var attr in vueAttrs)
                {
                    if (valueStr.StartsWith(attr.Key))
                    {
                        key += attr.Value;
                        valueStr = valueStr.Substring(attr.Key.Length);
                    }
                }

                value = valueStr;
            }

            return new KeyValuePair<string, object>(key, value);
        }

        /// <summary>
        /// Ref: http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
        /// </summary>
        private const string idsToReuseKey = "__htmlPrefixScopeExtensions_IdsToReuse_";

        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            //nested list support
            //http://www.joe-stevens.com/2011/06/06/editing-and-binding-nested-lists-with-asp-net-mvc-2/
            if (html.ViewData["ContainerPrefix"] != null)
            {
                collectionName = string.Concat(html.ViewData["ContainerPrefix"], ".", collectionName);
            }
            else
            {
                collectionName = string.Concat(html.ViewData.TemplateInfo.HtmlFieldPrefix, ".", collectionName).Trim('.');
            }

            var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            var htmlFieldPrefix = string.Format("{0}[{1}]", collectionName, itemIndex);
            html.ViewData["ContainerPrefix"] = htmlFieldPrefix;

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

            return BeginHtmlFieldPrefixScope(html, string.Format("{0}[{1}]", collectionName, itemIndex));
        }

        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        private static Queue<string> GetIdsToReuse(HttpContextBase httpContext, string collectionName)
        {
            // We need to use the same sequence of IDs following a server-side validation failure,  
            // otherwise the framework won't render the validation error messages next to each item.
            string key = idsToReuseKey + collectionName;
            var queue = (Queue<string>)httpContext.Items[key];
            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();
                var previouslyUsedIds = httpContext.Request.Form[collectionName + ".index"];
                if (!string.IsNullOrEmpty(previouslyUsedIds))
                    foreach (string previouslyUsedId in previouslyUsedIds.Split(','))
                        queue.Enqueue(previouslyUsedId);
            }
            return queue;
        }

        public static void AddOrUpdateCookie(this HttpContextBase httpContext, string name, Action<HttpCookie> setValue)
        {
            var cookie = httpContext.Request.Cookies[name];

            if (cookie == null)
                cookie = new HttpCookie(name);

            setValue(cookie);
            httpContext.Response.Cookies.Add(cookie);
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;

                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}