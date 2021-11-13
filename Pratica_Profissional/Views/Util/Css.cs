using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Pratica_Profissional.Util.Class
{

    public static class Helper
    {

        const string keyForBlockScript = "__key_For_Js_Block";
        const string keyForBlockStyle = "__key_For_Css_Block";

        public const string Table = "table-hover table table-condensed table-bordered table-striped";
        public const string InputXS = "col-lg-2 col-md-2 col-sm-2 col-xs-12";
        public const string InputSM = "col-lg-3 col-md-3 col-sm-3 col-xs-12";
        public const string InputSMMD = "col-lg-4 col-md-4 col-sm-4 col-xs-12";
        public const string InputMD = "col-lg-6 col-md-6 col-sm-6 col-xs-12";
        public const string InputMDLG = "col-lg-7 col-md-7 col-sm-7 col-xs-12";
        public const string InputLG = "col-lg-9 col-md-9 col-sm-9 col-xs-12";
        public const string Input1 = "col-lg-1 col-md-1 col-sm-1 col-xs-12";
        public const string Input2 = "col-lg-2 col-md-2 col-sm-2 col-xs-12";
        public const string Input3 = "col-lg-3 col-md-3 col-sm-3 col-xs-12";
        public const string Input4 = "col-lg-4 col-md-4 col-sm-4 col-xs-12";
        public const string Input5 = "col-lg-5 col-md-5 col-sm-5 col-xs-12";
        public const string Input6 = "col-lg-6 col-md-6 col-sm-6 col-xs-12";
        public const string Input7 = "col-lg-7 col-md-7 col-sm-7 col-xs-12";
        public const string Input9 = "col-lg-9 col-md-9 col-sm-9 col-xs-12";
        public const string Input10 = "col-lg-10 col-md-10 col-sm-10 col-xs-12";
        public const string Input11 = "col-lg-11 col-md-11 col-sm-11 col-xs-12";
        public const string Input12 = "col-lg-12 col-md-12 col-sm-12 col-xs-12";
        public const string LabelHidenXS = "control-label col-lg-2 col-md-2 col-sm-3 hidden-xs control-label-hiden-xs";
        public const string LabelHiden = "control-label hidden-lg hidden-md hidden-sm visible-xs col-xs-12";
        public const string LabelXXS = "control-label col-lg-1 col-md-1 col-sm-1 col-xs-12";
        public const string LabelXS = "control-label col-lg-2 col-md-2 col-sm-2 col-xs-12";
        public const string LabelSM = "control-label col-lg-3 col-md-3 col-sm-3 col-xs-12";
        public const string LabelMD = "control-label col-lg-4 col-md-4 col-sm-4 col-xs-12";
        public const string LabelTop = "control-label";
        public const string Label = "control-label col-lg-2 col-md-2 col-sm-3 col-xs-12";
        public const string Input8 = "col-lg-8 col-md-8 col-sm-8 col-xs-12";

        public static string getInputId(string id, string prefixo)
        {
            return ((string.IsNullOrEmpty(prefixo) ? "" : prefixo + "_") + id).Replace('.', '_');
        }


    }

    public static partial class HtmlExtensions
    {
        public static MvcHtmlString ClientPrefixName(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix.Replace('.', '_'));
        }
        public static MvcHtmlString ClientPrefix(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix);
        }

        public static MvcHtmlString ClientNameFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression)));
        }

        public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
        }
    }

    public static class FatHtml
    {
        const string keyForBlockScript = "__key_For_Js_Block";
        const string keyForBlockStyle = "__key_For_Css_Block";

        private static MvcHtmlString AddBlock(HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template, string type)
        {
            var stringBuilder = helper.ViewContext.HttpContext.Items[type] as StringBuilder ?? new StringBuilder();
            stringBuilder.Append(template(null).ToHtmlString());
            helper.ViewContext.HttpContext.Items[type] = stringBuilder;
            return new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// Adiciona um bloco JavaScript ao fim da lista de bloco de scripts
        /// </summary> 
        public static MvcHtmlString AddScriptBlock(this HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template)
        {
            return AddBlock(helper, template, keyForBlockScript);
        }

        /// <summary>
        /// Adiciona um bloco Css ao fim da lista de bloco de estilos
        /// </summary>
        public static MvcHtmlString AddStyleBlock(this HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template)
        {
            return AddBlock(helper, template, keyForBlockStyle);
        }

        /// <summary>
        /// Renderiza todos blocos de scripts
        /// </summary>
        public static MvcHtmlString RenderScriptBlocks(this HtmlHelper helper)
        {
            var stringBuilder = helper.ViewContext.HttpContext.Items[keyForBlockScript] as StringBuilder ?? new StringBuilder();
            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>
        /// Renderiza todos blocos de estilos
        /// </summary>
        public static MvcHtmlString RenderStyleBlocks(this HtmlHelper helper)
        {
            var stringBuilder = helper.ViewContext.HttpContext.Items[keyForBlockStyle] as StringBuilder ?? new StringBuilder();
            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
    public class JsonSelect<T>
    {

        public JsonSelect() { }

        public JsonSelect(IQueryable<T> query, int? page, int? pageSize)
        {
            page = page ?? 0;
            pageSize = pageSize ?? 0;

            totalCount = query.Count();
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            more = (page * pageSize) < totalCount;

            results = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        public List<T> results { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }
        public bool more { get; set; }
    }
    public static class ViewExtensions
    {
        public static MvcHtmlString CustomValidationSummary(this HtmlHelper html, bool closeable = true, bool hideProperties = true, string validationMessage = "", object htmlAttributes = null)
        {
            if (!html.ViewData.ModelState.IsValid)
            {

                TagBuilder div = new TagBuilder("div");
                string properties = string.Empty;

                // adiciona os atributos
                if (htmlAttributes != null)
                {
                    var type = htmlAttributes.GetType();
                    var props = type.GetProperties();

                    foreach (var item in props)
                    {
                        div.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
                    }
                }

                if (closeable)
                {
                    div.InnerHtml += @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">×</button>";
                    div.AddCssClass("print");
                }

                // adiciona mensagem na div
                div.InnerHtml += validationMessage;

                if (!hideProperties)
                {
                    foreach (var key in html.ViewData.ModelState.Keys)
                    {
                        foreach (var err in html.ViewData.ModelState[key].Errors)
                        {
                            properties += "<p>" + html.Encode(err.ErrorMessage) + "</p>";
                        }
                    }

                    if (!string.IsNullOrEmpty(properties))
                    {
                        div.InnerHtml += properties;
                    }
                }

                return MvcHtmlString.Create(div.ToString());
            }

            return null;
        }

        public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, object htmlAttributes, object icons = null, bool hideText = false)
        {
            return CustomActionLink(html, linkText, actionName, null, new { }, htmlAttributes, icons, hideText);
        }

        public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, object routeValues, object htmlAttributes, object icons = null, bool hideText = false)
        {
            return CustomActionLink(html, linkText, actionName, null, routeValues, htmlAttributes, icons, hideText);
        }

        public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object icons = null, bool hideText = false)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string iconLeft = string.Empty,
                    iconRight = string.Empty,
                    innerHtml = string.Empty;

            TagBuilder a = new TagBuilder("a");
            TagBuilder i;
            bool hasTitle = false;

            if (string.IsNullOrEmpty(controllerName))
            {
                a.Attributes.Add("href", actionName.StartsWith("#") ? actionName : urlHelper.Action(actionName, routeValues));
            }
            else
            {
                a.Attributes.Add("href", actionName.StartsWith("#") ? actionName : urlHelper.Action(actionName, controllerName, routeValues));
            }

            // adiciona os atributos
            var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            foreach (var attribute in htmlAttributesDictionary)
            {
                if (!hasTitle)
                {
                    hasTitle = attribute.Key.ToLower().Equals("title");
                }

                a.MergeAttribute(attribute.Key, Convert.ToString(attribute.Value));
            }

            // adiciona os icones
            if (icons != null)
            {
                var type = icons.GetType();
                var props = type.GetProperties();
                foreach (var item in props)
                {
                    if (item.Name.ToLower().Equals("left"))
                    {
                        iconLeft = item.GetValue(icons, null).ToString();
                    }
                    else if (item.Name.ToLower().Equals("right"))
                    {
                        iconRight = item.GetValue(icons, null).ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(iconLeft))
            {
                i = new TagBuilder("i");
                i.AddCssClass(iconLeft);
                innerHtml += i.ToString() + " ";
            }

            if (!hideText)
            {
                innerHtml += linkText;
            }

            if (!hasTitle && hideText)
            {
                a.Attributes.Add("title", linkText);
            }

            if (!string.IsNullOrEmpty(iconRight))
            {
                i = new TagBuilder("i");
                i.AddCssClass(iconRight);
                innerHtml += " " + i.ToString();
            }

            a.InnerHtml = innerHtml;

            return MvcHtmlString.Create(a.ToString());
        }

        public static MvcHtmlString CustomButton(this HtmlHelper html, string buttonText, object htmlAttributes, object icons = null, bool hideText = false)
        {
            return CustomButton(html, buttonText, null, htmlAttributes, icons, hideText);
        }

        public static MvcHtmlString CustomButton(this HtmlHelper html, string buttonText, string buttonType, object htmlAttributes, object icons = null, bool hideText = false)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string iconLeft = string.Empty,
                    iconRight = string.Empty,
                    innerHtml = string.Empty;

            TagBuilder button = new TagBuilder("button");
            TagBuilder i;
            bool hasTitle = false;

            if (string.IsNullOrEmpty(buttonType))
            {
                button.Attributes.Add("type", "button");
            }
            else
            {
                button.Attributes.Add("type", buttonType);
            }


            // adiciona os atributos
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var props = type.GetProperties();
                foreach (var item in props)
                {
                    if (!hasTitle)
                    {
                        hasTitle = item.Name.ToLower().Equals("title");
                    }

                    button.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
                }
            }

            // adiciona os icones
            if (icons != null)
            {
                var type = icons.GetType();
                var props = type.GetProperties();
                foreach (var item in props)
                {
                    if (item.Name.ToLower().Equals("left"))
                    {
                        iconLeft = item.GetValue(icons, null).ToString();
                    }
                    else if (item.Name.ToLower().Equals("right"))
                    {
                        iconRight = item.GetValue(icons, null).ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(iconLeft))
            {
                i = new TagBuilder("i");
                i.AddCssClass(iconLeft);
                innerHtml += i.ToString() + " ";
            }

            if (!hideText)
            {
                innerHtml += buttonText;
            }

            if (!hasTitle && hideText)
            {
                button.Attributes.Add("title", buttonText);
            }

            if (!string.IsNullOrEmpty(iconRight))
            {
                i = new TagBuilder("i");
                i.AddCssClass(iconRight);
                innerHtml += " " + i.ToString();
            }

            button.InnerHtml = innerHtml;

            return MvcHtmlString.Create(button.ToString());
        }

        public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            return CustomLabelFor(html, expression, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes = null)
        {
            TagBuilder lbl = new TagBuilder("label");
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);

            if (string.IsNullOrEmpty(labelText))
            {
                labelText = metadata.DisplayName;
            }

            lbl.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
            lbl.SetInnerText(labelText);

            // adiciona os atributos
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var props = type.GetProperties();
                foreach (var item in props)
                {
                    lbl.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
                }
            }

            return MvcHtmlString.Create(lbl.ToString());
        }
    }
}