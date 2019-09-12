using BTS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BTS.Web.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Unique : ValidationAttribute
    {
        public Type TargetModelType { get; set; }
        public string TargetPropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (TargetModelType == null || string.IsNullOrEmpty(TargetPropertyName)) ? DirectlyValid(value, validationContext) : ViewModelValid(value, validationContext);
        }


        private ValidationResult DirectlyValid(object value, ValidationContext validationContext)
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                string Name = GetName(validationContext);

                PropertyInfo IdProp = validationContext.ObjectInstance.GetType().GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0);

                int Id = (int)IdProp.GetValue(validationContext.ObjectInstance, null);

                Type entityType = validationContext.ObjectType;


                IQueryable result = db.Set(entityType).Where(Name + "==@0", value);
                int count = 0;

                if (Id > 0)
                {
                    result = result.Where(IdProp.Name + "<>@0", Id);
                }

                count = result.Count();

                if (count == 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

        }
        private string GetName(ValidationContext validationContext)
        {
            string Name = validationContext.MemberName;

            if (string.IsNullOrEmpty(Name))
            {
                string displayName = validationContext.DisplayName;

                PropertyInfo prop = validationContext.ObjectInstance.GetType().GetProperty(displayName);

                if (prop != null)
                {
                    Name = prop.Name;
                }
                else
                {
                    List<PropertyInfo> props = validationContext.ObjectInstance.GetType().GetProperties().Where(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(DisplayAttribute)) > 0).ToList();

                    foreach (PropertyInfo prp in props)
                    {
                        CustomAttributeData attr = prp.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(DisplayAttribute));

                        object val = attr.NamedArguments.FirstOrDefault(p => p.MemberName == "Name").TypedValue.Value;

                        if (val.Equals(displayName))
                        {
                            Name = prp.Name;
                            break;
                        }
                    }
                }
            }

            return Name;


        }
        private ValidationResult ViewModelValid(object value, ValidationContext validationContext)
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                string Name = TargetPropertyName;

                PropertyInfo IdProp = TargetModelType.GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0) ?? TargetModelType.GetProperties().FirstOrDefault();



                string Id = (string)validationContext.ObjectInstance.GetType().GetProperty(IdProp.Name).GetValue(validationContext.ObjectInstance, null);

                //int Id = (int)IdProp.GetValue(validationContext.ObjectInstance, null);

                Type entityType = TargetModelType;


                IQueryable result = db.Set(entityType).Where(Name + "==@0", value);
                int count = 0;

                if (Id.Length > 0)
                {
                    result = result.Where(IdProp.Name + "<>@0", Id);
                }

                count = result.Count();

                if (count == 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

        }

        private ValidationResult ViewModelValid_forIntKey(object value, ValidationContext validationContext)
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                string Name = TargetPropertyName;

                PropertyInfo IdProp = TargetModelType.GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0) ?? TargetModelType.GetProperties().FirstOrDefault();



                int Id = (int)validationContext.ObjectInstance.GetType().GetProperty(IdProp.Name).GetValue(validationContext.ObjectInstance, null);

                //int Id = (int)IdProp.GetValue(validationContext.ObjectInstance, null);

                Type entityType = TargetModelType;


                IQueryable result = db.Set(entityType).Where(Name + "==@0", value);
                int count = 0;

                if (Id > 0)
                {
                    result = result.Where(IdProp.Name + "<>@0", Id);
                }

                count = result.Count();

                if (count == 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

        }
    }

    public class UserNameFilter : ActionFilterAttribute
    {
        private const string parameterName = "userName";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey(parameterName))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.ActionParameters[parameterName] = filterContext.HttpContext.User.Identity.Name;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NoSundayAccessAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                filterContext.Result = new ContentResult
                { Content = "It's Sunday, get some rest" };
            }
        }
    }

    public static class HtmlHelperExtension
    {
        // How to use [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] for @Html.TextBox
        // @Html.TextBoxWithFormatFor(m => m.CustomDate, new Dictionary<string, object> { { "class", "datepicker" } })

        public static MvcHtmlString TextBoxWithFormatFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.TextBox(htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(metadata.PropertyName), string.Format(metadata.DisplayFormatString, metadata.Model), htmlAttributes);
        }

        public static IHtmlString MetaAcceptLanguage<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            string acceptLanguage = HttpUtility.HtmlAttributeEncode(Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(string.Format("<meta name=\"accept-language\" content=\"{0}\">", acceptLanguage));
        }

        public static MvcHtmlString ListArrayItems(this HtmlHelper html, string[] list)
        {
            TagBuilder tag = new TagBuilder("ul");
            foreach (string str in list)
            {
                TagBuilder itemTag = new TagBuilder("li");
                itemTag.SetInnerText(str);
                tag.InnerHtml += itemTag.ToString();
            }
            return new MvcHtmlString(tag.ToString());
        }
    }
}