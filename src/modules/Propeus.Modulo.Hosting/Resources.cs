﻿using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Propeus.Modulo.Hosting
{
    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.AspNetCore.Mvc.ViewFeatures.Resources", typeof(Resources).GetTypeInfo().Assembly);

        //
        // Resumo:
        //     The view component name '{0}' matched multiple types:{1}{2}
        internal static string ViewComponent_AmbiguousTypeMatch => GetString("ViewComponent_AmbiguousTypeMatch");

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' should be declared to return {2}<T>.
        internal static string ViewComponent_AsyncMethod_ShouldReturnTask => GetString("ViewComponent_AsyncMethod_ShouldReturnTask");

        //
        // Resumo:
        //     A view component must return a non-null value.
        internal static string ViewComponent_MustReturnValue => GetString("ViewComponent_MustReturnValue");

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' should be declared to return a value.
        internal static string ViewComponent_SyncMethod_ShouldReturnValue => GetString("ViewComponent_SyncMethod_ShouldReturnValue");

        //
        // Resumo:
        //     A view component named '{0}' could not be found. A view component must be a public
        //     non-abstract class, not contain any generic parameters, and either be decorated
        //     with '{1}' or have a class name ending with the '{2}' suffix. A view component
        //     must not be decorated with '{3}'.
        internal static string ViewComponent_CannotFindComponent => GetString("ViewComponent_CannotFindComponent");

        //
        // Resumo:
        //     An invoker could not be created for the view component '{0}'.
        internal static string ViewComponent_IViewComponentFactory_ReturnedNull => GetString("ViewComponent_IViewComponentFactory_ReturnedNull");

        //
        // Resumo:
        //     Could not find an '{0}' or '{1}' method for the view component '{2}'.
        internal static string ViewComponent_CannotFindMethod => GetString("ViewComponent_CannotFindMethod");

        //
        // Resumo:
        //     View components only support returning {0}, {1} or {2}.
        internal static string ViewComponent_InvalidReturnValue => GetString("ViewComponent_InvalidReturnValue");

        //
        // Resumo:
        //     Value cannot be null or empty.
        internal static string ArgumentCannotBeNullOrEmpty => GetString("ArgumentCannotBeNullOrEmpty");

        //
        // Resumo:
        //     The '{0}' property of '{1}' must not be null.
        internal static string PropertyOfTypeCannotBeNull => GetString("PropertyOfTypeCannotBeNull");

        //
        // Resumo:
        //     The '{0}' method of type '{1}' cannot return a null value.
        internal static string TypeMethodMustReturnNotNullValue => GetString("TypeMethodMustReturnNotNullValue");

        //
        // Resumo:
        //     Property '{0}' is of type '{1}', but this method requires a value of type '{2}'.
        internal static string ArgumentPropertyUnexpectedType => GetString("ArgumentPropertyUnexpectedType");

        //
        // Resumo:
        //     The partial view '{0}' was not found or no view engine supports the searched
        //     locations. The following locations were searched:{1}
        internal static string Common_PartialViewNotFound => GetString("Common_PartialViewNotFound");

        //
        // Resumo:
        //     False
        internal static string Common_TriState_False => GetString("Common_TriState_False");

        //
        // Resumo:
        //     Not Set
        internal static string Common_TriState_NotSet => GetString("Common_TriState_NotSet");

        //
        // Resumo:
        //     True
        internal static string Common_TriState_True => GetString("Common_TriState_True");

        //
        // Resumo:
        //     ViewData value must not be null.
        internal static string DynamicViewData_ViewDataNull => GetString("DynamicViewData_ViewDataNull");

        //
        // Resumo:
        //     The expression compiler was unable to evaluate the indexer expression '{0}' because
        //     it references the model parameter '{1}' which is unavailable.
        internal static string ExpressionHelper_InvalidIndexerExpression => GetString("ExpressionHelper_InvalidIndexerExpression");

        //
        // Resumo:
        //     The IModelMetadataProvider was unable to provide metadata for expression '{0}'.
        internal static string HtmlHelper_NullModelMetadata => GetString("HtmlHelper_NullModelMetadata");

        //
        // Resumo:
        //     Must call 'Contextualize' method before using this HtmlHelper instance.
        internal static string HtmlHelper_NotContextualized => GetString("HtmlHelper_NotContextualized");

        //
        // Resumo:
        //     There is no ViewData item of type '{0}' that has the key '{1}'.
        internal static string HtmlHelper_MissingSelectData => GetString("HtmlHelper_MissingSelectData");

        //
        // Resumo:
        //     The parameter '{0}' must evaluate to an IEnumerable when multiple selection is
        //     allowed.
        internal static string HtmlHelper_SelectExpressionNotEnumerable => GetString("HtmlHelper_SelectExpressionNotEnumerable");

        //
        // Resumo:
        //     The type '{0}' is not supported. Type must be an {1} that does not have an associated
        //     {2}.
        internal static string HtmlHelper_TypeNotSupported_ForGetEnumSelectList => GetString("HtmlHelper_TypeNotSupported_ForGetEnumSelectList");

        //
        // Resumo:
        //     The ViewData item that has the key '{0}' is of type '{1}' but must be of type
        //     '{2}'.
        internal static string HtmlHelper_WrongSelectDataType => GetString("HtmlHelper_WrongSelectDataType");

        //
        // Resumo:
        //     The '{0}' template was used with an object of type '{1}', which does not implement
        //     '{2}'.
        internal static string Templates_TypeMustImplementIEnumerable => GetString("Templates_TypeMustImplementIEnumerable");

        //
        // Resumo:
        //     Templates can be used only with field access, property access, single-dimension
        //     array index, or single-parameter custom indexer expressions.
        internal static string TemplateHelpers_TemplateLimitations => GetString("TemplateHelpers_TemplateLimitations");

        //
        // Resumo:
        //     Unable to locate an appropriate template for type {0}.
        internal static string TemplateHelpers_NoTemplate => GetString("TemplateHelpers_NoTemplate");

        //
        // Resumo:
        //     The model item passed is null, but this ViewDataDictionary instance requires
        //     a non-null model item of type '{0}'.
        internal static string ViewData_ModelCannotBeNull => GetString("ViewData_ModelCannotBeNull");

        //
        // Resumo:
        //     The model item passed into the ViewDataDictionary is of type '{0}', but this
        //     ViewDataDictionary instance requires a model item of type '{1}'.
        internal static string ViewData_WrongTModelType => GetString("ViewData_WrongTModelType");

        //
        // Resumo:
        //     The partial view '{0}' was not found. The following locations were searched:{1}
        internal static string ViewEngine_PartialViewNotFound => GetString("ViewEngine_PartialViewNotFound");

        //
        // Resumo:
        //     The view '{0}' was not found. The following locations were searched:{1}
        internal static string ViewEngine_ViewNotFound => GetString("ViewEngine_ViewNotFound");

        //
        // Resumo:
        //     The value must be greater than or equal to zero.
        internal static string HtmlHelper_TextAreaParameterOutOfRange => GetString("HtmlHelper_TextAreaParameterOutOfRange");

        //
        // Resumo:
        //     Validation parameter names in unobtrusive client validation rules cannot be empty.
        //     Client rule type: {0}
        internal static string UnobtrusiveJavascript_ValidationParameterCannotBeEmpty => GetString("UnobtrusiveJavascript_ValidationParameterCannotBeEmpty");

        //
        // Resumo:
        //     Validation parameter names in unobtrusive client validation rules must start
        //     with a lowercase letter and consist of only lowercase letters or digits. Validation
        //     parameter name: {0}, client rule type: {1}
        internal static string UnobtrusiveJavascript_ValidationParameterMustBeLegal => GetString("UnobtrusiveJavascript_ValidationParameterMustBeLegal");

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules cannot be empty.
        //     Client rule type: {0}
        internal static string UnobtrusiveJavascript_ValidationTypeCannotBeEmpty => GetString("UnobtrusiveJavascript_ValidationTypeCannotBeEmpty");

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules must consist of
        //     only lowercase letters. Invalid name: "{0}", client rule type: {1}
        internal static string UnobtrusiveJavascript_ValidationTypeMustBeLegal => GetString("UnobtrusiveJavascript_ValidationTypeMustBeLegal");

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules must be unique.
        //     The following validation type was seen more than once: {0}
        internal static string UnobtrusiveJavascript_ValidationTypeMustBeUnique => GetString("UnobtrusiveJavascript_ValidationTypeMustBeUnique");

        //
        // Resumo:
        //     The type '{0}' must derive from '{1}'.
        internal static string TypeMustDeriveFromType => GetString("TypeMustDeriveFromType");

        //
        // Resumo:
        //     Could not find a replacement for view expansion token '{0}'.
        internal static string TemplatedViewLocationExpander_NoReplacementToken => GetString("TemplatedViewLocationExpander_NoReplacementToken");

        //
        // Resumo:
        //     {0} must be executed before {1} can be invoked.
        internal static string TemplatedExpander_PopulateValuesMustBeInvokedFirst => GetString("TemplatedExpander_PopulateValuesMustBeInvokedFirst");

        //
        // Resumo:
        //     The result of value factory cannot be null.
        internal static string TemplatedExpander_ValueFactoryCannotReturnNull => GetString("TemplatedExpander_ValueFactoryCannotReturnNull");

        //
        // Resumo:
        //     Type: '{0}' - ModuleName: '{1}'
        internal static string ViewComponent_AmbiguousTypeMatch_Item => GetString("ViewComponent_AmbiguousTypeMatch_Item");

        //
        // Resumo:
        //     The property {0}.{1} could not be found.
        internal static string Common_PropertyNotFound => GetString("Common_PropertyNotFound");

        //
        // Resumo:
        //     No URL for remote validation could be found.
        internal static string RemoteAttribute_NoUrlFound => GetString("RemoteAttribute_NoUrlFound");

        //
        // Resumo:
        //     '{0}' is invalid.
        internal static string RemoteAttribute_RemoteValidationFailed => GetString("RemoteAttribute_RemoteValidationFailed");

        //
        // Resumo:
        //     The name of an HTML field cannot be null or empty. Instead use methods {0}.{1}
        //     or {2}.{3} with a non-empty {4} argument value.
        internal static string HtmlGenerator_FieldNameCannotBeNullOrEmpty => GetString("HtmlGenerator_FieldNameCannotBeNullOrEmpty");

        //
        // Resumo:
        //     Either the '{0}' or '{1}' property must be set in order to invoke a view component.
        internal static string ViewComponentResult_NameOrTypeMustBeSet => GetString("ViewComponentResult_NameOrTypeMustBeSet");

        //
        // Resumo:
        //     Cannot deserialize {0} of type '{1}'.
        internal static string TempData_CannotDeserializeToken => GetString("TempData_CannotDeserializeToken");

        //
        // Resumo:
        //     The '{0}' cannot serialize a dictionary with a key of type '{1}'. The key must
        //     be of type '{2}'.
        internal static string TempData_CannotSerializeDictionary => GetString("TempData_CannotSerializeDictionary");

        //
        // Resumo:
        //     The '{0}' cannot serialize an object of type '{1}'.
        internal static string TempData_CannotSerializeType => GetString("TempData_CannotSerializeType");

        //
        // Resumo:
        //     The collection already contains an entry with key '{0}'.
        internal static string Dictionary_DuplicateKey => GetString("Dictionary_DuplicateKey");

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' cannot return a {2}.
        internal static string ViewComponent_SyncMethod_CannotReturnTask => GetString("ViewComponent_SyncMethod_CannotReturnTask");

        //
        // Resumo:
        //     View component '{0}' must have exactly one public method named '{1}' or '{2}'.
        internal static string ViewComponent_AmbiguousMethods => GetString("ViewComponent_AmbiguousMethods");

        //
        // Resumo:
        //     The type '{0}' cannot be activated by '{1}' because it is either a value type,
        //     an interface, an abstract class or an open generic type.
        internal static string ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated => GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated");

        //
        // Resumo:
        //     The {0} was unable to provide metadata for expression '{1}'.
        internal static string CreateModelExpression_NullModelMetadata => GetString("CreateModelExpression_NullModelMetadata");

        //
        // Resumo:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to locate a view
        //     for rendering.
        internal static string ViewEnginesAreRequired => GetString("ViewEnginesAreRequired");

        //
        // Resumo:
        //     The '{0}.{1}' property with {2} is invalid.
        internal static string TempDataProperties_InvalidType => GetString("TempDataProperties_InvalidType");

        //
        // Resumo:
        //     The '{0}.{1}' property with {2} is invalid. A property using {2} must have a
        //     public getter and setter.
        internal static string TempDataProperties_PublicGetterSetter => GetString("TempDataProperties_PublicGetterSetter");

        //
        // Resumo:
        //     The view component name '{0}' matched multiple types:{1}{2}
        internal static string FormatViewComponent_AmbiguousTypeMatch(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_AmbiguousTypeMatch"), p0, p1, p2);
        }

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' should be declared to return {2}<T>.
        internal static string FormatViewComponent_AsyncMethod_ShouldReturnTask(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_AsyncMethod_ShouldReturnTask"), p0, p1, p2);
        }

        //
        // Resumo:
        //     A view component must return a non-null value.
        internal static string FormatViewComponent_MustReturnValue()
        {
            return GetString("ViewComponent_MustReturnValue");
        }

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' should be declared to return a value.
        internal static string FormatViewComponent_SyncMethod_ShouldReturnValue(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_SyncMethod_ShouldReturnValue"), p0, p1);
        }

        //
        // Resumo:
        //     A view component named '{0}' could not be found. A view component must be a public
        //     non-abstract class, not contain any generic parameters, and either be decorated
        //     with '{1}' or have a class name ending with the '{2}' suffix. A view component
        //     must not be decorated with '{3}'.
        internal static string FormatViewComponent_CannotFindComponent(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_CannotFindComponent"), p0, p1, p2, p3);
        }

        //
        // Resumo:
        //     An invoker could not be created for the view component '{0}'.
        internal static string FormatViewComponent_IViewComponentFactory_ReturnedNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_IViewComponentFactory_ReturnedNull"), p0);
        }

        //
        // Resumo:
        //     Could not find an '{0}' or '{1}' method for the view component '{2}'.
        internal static string FormatViewComponent_CannotFindMethod(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_CannotFindMethod"), p0, p1, p2);
        }

        //
        // Resumo:
        //     View components only support returning {0}, {1} or {2}.
        internal static string FormatViewComponent_InvalidReturnValue(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_InvalidReturnValue"), p0, p1, p2);
        }

        //
        // Resumo:
        //     Value cannot be null or empty.
        internal static string FormatArgumentCannotBeNullOrEmpty()
        {
            return GetString("ArgumentCannotBeNullOrEmpty");
        }

        //
        // Resumo:
        //     The '{0}' property of '{1}' must not be null.
        internal static string FormatPropertyOfTypeCannotBeNull(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyOfTypeCannotBeNull"), p0, p1);
        }

        //
        // Resumo:
        //     The '{0}' method of type '{1}' cannot return a null value.
        internal static string FormatTypeMethodMustReturnNotNullValue(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMethodMustReturnNotNullValue"), p0, p1);
        }

        //
        // Resumo:
        //     Property '{0}' is of type '{1}', but this method requires a value of type '{2}'.
        internal static string FormatArgumentPropertyUnexpectedType(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ArgumentPropertyUnexpectedType"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The partial view '{0}' was not found or no view engine supports the searched
        //     locations. The following locations were searched:{1}
        internal static string FormatCommon_PartialViewNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Common_PartialViewNotFound"), p0, p1);
        }

        //
        // Resumo:
        //     False
        internal static string FormatCommon_TriState_False()
        {
            return GetString("Common_TriState_False");
        }

        //
        // Resumo:
        //     Not Set
        internal static string FormatCommon_TriState_NotSet()
        {
            return GetString("Common_TriState_NotSet");
        }

        //
        // Resumo:
        //     True
        internal static string FormatCommon_TriState_True()
        {
            return GetString("Common_TriState_True");
        }

        //
        // Resumo:
        //     ViewData value must not be null.
        internal static string FormatDynamicViewData_ViewDataNull()
        {
            return GetString("DynamicViewData_ViewDataNull");
        }

        //
        // Resumo:
        //     The expression compiler was unable to evaluate the indexer expression '{0}' because
        //     it references the model parameter '{1}' which is unavailable.
        internal static string FormatExpressionHelper_InvalidIndexerExpression(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ExpressionHelper_InvalidIndexerExpression"), p0, p1);
        }

        //
        // Resumo:
        //     The IModelMetadataProvider was unable to provide metadata for expression '{0}'.
        internal static string FormatHtmlHelper_NullModelMetadata(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlHelper_NullModelMetadata"), p0);
        }

        //
        // Resumo:
        //     Must call 'Contextualize' method before using this HtmlHelper instance.
        internal static string FormatHtmlHelper_NotContextualized()
        {
            return GetString("HtmlHelper_NotContextualized");
        }

        //
        // Resumo:
        //     There is no ViewData item of type '{0}' that has the key '{1}'.
        internal static string FormatHtmlHelper_MissingSelectData(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlHelper_MissingSelectData"), p0, p1);
        }

        //
        // Resumo:
        //     The parameter '{0}' must evaluate to an IEnumerable when multiple selection is
        //     allowed.
        internal static string FormatHtmlHelper_SelectExpressionNotEnumerable(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlHelper_SelectExpressionNotEnumerable"), p0);
        }

        //
        // Resumo:
        //     The type '{0}' is not supported. Type must be an {1} that does not have an associated
        //     {2}.
        internal static string FormatHtmlHelper_TypeNotSupported_ForGetEnumSelectList(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlHelper_TypeNotSupported_ForGetEnumSelectList"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The ViewData item that has the key '{0}' is of type '{1}' but must be of type
        //     '{2}'.
        internal static string FormatHtmlHelper_WrongSelectDataType(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlHelper_WrongSelectDataType"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The '{0}' template was used with an object of type '{1}', which does not implement
        //     '{2}'.
        internal static string FormatTemplates_TypeMustImplementIEnumerable(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Templates_TypeMustImplementIEnumerable"), p0, p1, p2);
        }

        //
        // Resumo:
        //     Templates can be used only with field access, property access, single-dimension
        //     array index, or single-parameter custom indexer expressions.
        internal static string FormatTemplateHelpers_TemplateLimitations()
        {
            return GetString("TemplateHelpers_TemplateLimitations");
        }

        //
        // Resumo:
        //     Unable to locate an appropriate template for type {0}.
        internal static string FormatTemplateHelpers_NoTemplate(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TemplateHelpers_NoTemplate"), p0);
        }

        //
        // Resumo:
        //     The model item passed is null, but this ViewDataDictionary instance requires
        //     a non-null model item of type '{0}'.
        internal static string FormatViewData_ModelCannotBeNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewData_ModelCannotBeNull"), p0);
        }

        //
        // Resumo:
        //     The model item passed into the ViewDataDictionary is of type '{0}', but this
        //     ViewDataDictionary instance requires a model item of type '{1}'.
        internal static string FormatViewData_WrongTModelType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewData_WrongTModelType"), p0, p1);
        }

        //
        // Resumo:
        //     The partial view '{0}' was not found. The following locations were searched:{1}
        internal static string FormatViewEngine_PartialViewNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewEngine_PartialViewNotFound"), p0, p1);
        }

        //
        // Resumo:
        //     The view '{0}' was not found. The following locations were searched:{1}
        internal static string FormatViewEngine_ViewNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewEngine_ViewNotFound"), p0, p1);
        }

        //
        // Resumo:
        //     The value must be greater than or equal to zero.
        internal static string FormatHtmlHelper_TextAreaParameterOutOfRange()
        {
            return GetString("HtmlHelper_TextAreaParameterOutOfRange");
        }

        //
        // Resumo:
        //     Validation parameter names in unobtrusive client validation rules cannot be empty.
        //     Client rule type: {0}
        internal static string FormatUnobtrusiveJavascript_ValidationParameterCannotBeEmpty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnobtrusiveJavascript_ValidationParameterCannotBeEmpty"), p0);
        }

        //
        // Resumo:
        //     Validation parameter names in unobtrusive client validation rules must start
        //     with a lowercase letter and consist of only lowercase letters or digits. Validation
        //     parameter name: {0}, client rule type: {1}
        internal static string FormatUnobtrusiveJavascript_ValidationParameterMustBeLegal(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnobtrusiveJavascript_ValidationParameterMustBeLegal"), p0, p1);
        }

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules cannot be empty.
        //     Client rule type: {0}
        internal static string FormatUnobtrusiveJavascript_ValidationTypeCannotBeEmpty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnobtrusiveJavascript_ValidationTypeCannotBeEmpty"), p0);
        }

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules must consist of
        //     only lowercase letters. Invalid name: "{0}", client rule type: {1}
        internal static string FormatUnobtrusiveJavascript_ValidationTypeMustBeLegal(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnobtrusiveJavascript_ValidationTypeMustBeLegal"), p0, p1);
        }

        //
        // Resumo:
        //     Validation type names in unobtrusive client validation rules must be unique.
        //     The following validation type was seen more than once: {0}
        internal static string FormatUnobtrusiveJavascript_ValidationTypeMustBeUnique(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnobtrusiveJavascript_ValidationTypeMustBeUnique"), p0);
        }

        //
        // Resumo:
        //     The type '{0}' must derive from '{1}'.
        internal static string FormatTypeMustDeriveFromType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMustDeriveFromType"), p0, p1);
        }

        //
        // Resumo:
        //     Could not find a replacement for view expansion token '{0}'.
        internal static string FormatTemplatedViewLocationExpander_NoReplacementToken(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TemplatedViewLocationExpander_NoReplacementToken"), p0);
        }

        //
        // Resumo:
        //     {0} must be executed before {1} can be invoked.
        internal static string FormatTemplatedExpander_PopulateValuesMustBeInvokedFirst(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TemplatedExpander_PopulateValuesMustBeInvokedFirst"), p0, p1);
        }

        //
        // Resumo:
        //     The result of value factory cannot be null.
        internal static string FormatTemplatedExpander_ValueFactoryCannotReturnNull()
        {
            return GetString("TemplatedExpander_ValueFactoryCannotReturnNull");
        }

        //
        // Resumo:
        //     Type: '{0}' - ModuleName: '{1}'
        internal static string FormatViewComponent_AmbiguousTypeMatch_Item(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_AmbiguousTypeMatch_Item"), p0, p1);
        }

        //
        // Resumo:
        //     The property {0}.{1} could not be found.
        internal static string FormatCommon_PropertyNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Common_PropertyNotFound"), p0, p1);
        }

        //
        // Resumo:
        //     No URL for remote validation could be found.
        internal static string FormatRemoteAttribute_NoUrlFound()
        {
            return GetString("RemoteAttribute_NoUrlFound");
        }

        //
        // Resumo:
        //     '{0}' is invalid.
        internal static string FormatRemoteAttribute_RemoteValidationFailed(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("RemoteAttribute_RemoteValidationFailed"), p0);
        }

        //
        // Resumo:
        //     The name of an HTML field cannot be null or empty. Instead use methods {0}.{1}
        //     or {2}.{3} with a non-empty {4} argument value.
        internal static string FormatHtmlGenerator_FieldNameCannotBeNullOrEmpty(object p0, object p1, object p2, object p3, object p4)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlGenerator_FieldNameCannotBeNullOrEmpty"), p0, p1, p2, p3, p4);
        }

        //
        // Resumo:
        //     Either the '{0}' or '{1}' property must be set in order to invoke a view component.
        internal static string FormatViewComponentResult_NameOrTypeMustBeSet(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponentResult_NameOrTypeMustBeSet"), p0, p1);
        }

        //
        // Resumo:
        //     Cannot deserialize {0} of type '{1}'.
        internal static string FormatTempData_CannotDeserializeToken(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TempData_CannotDeserializeToken"), p0, p1);
        }

        //
        // Resumo:
        //     The '{0}' cannot serialize a dictionary with a key of type '{1}'. The key must
        //     be of type '{2}'.
        internal static string FormatTempData_CannotSerializeDictionary(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TempData_CannotSerializeDictionary"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The '{0}' cannot serialize an object of type '{1}'.
        internal static string FormatTempData_CannotSerializeType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TempData_CannotSerializeType"), p0, p1);
        }

        //
        // Resumo:
        //     The collection already contains an entry with key '{0}'.
        internal static string FormatDictionary_DuplicateKey(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Dictionary_DuplicateKey"), p0);
        }

        //
        // Resumo:
        //     Method '{0}' of view component '{1}' cannot return a {2}.
        internal static string FormatViewComponent_SyncMethod_CannotReturnTask(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_SyncMethod_CannotReturnTask"), p0, p1, p2);
        }

        //
        // Resumo:
        //     View component '{0}' must have exactly one public method named '{1}' or '{2}'.
        internal static string FormatViewComponent_AmbiguousMethods(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewComponent_AmbiguousMethods"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The type '{0}' cannot be activated by '{1}' because it is either a value type,
        //     an interface, an abstract class or an open generic type.
        internal static string FormatValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated"), p0, p1);
        }

        //
        // Resumo:
        //     The {0} was unable to provide metadata for expression '{1}'.
        internal static string FormatCreateModelExpression_NullModelMetadata(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CreateModelExpression_NullModelMetadata"), p0, p1);
        }

        //
        // Resumo:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to locate a view
        //     for rendering.
        internal static string FormatViewEnginesAreRequired(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ViewEnginesAreRequired"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The '{0}.{1}' property with {2} is invalid.
        internal static string FormatTempDataProperties_InvalidType(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TempDataProperties_InvalidType"), p0, p1, p2);
        }

        //
        // Resumo:
        //     The '{0}.{1}' property with {2} is invalid. A property using {2} must have a
        //     public getter and setter.
        internal static string FormatTempDataProperties_PublicGetterSetter(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TempDataProperties_PublicGetterSetter"), p0, p1, p2);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            string text = _resourceManager.GetString(name);
            if (formatterNames != null)
            {
                for (int i = 0; i < formatterNames.Length; i++)
                {
                    text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return text;
        }
    }
}
