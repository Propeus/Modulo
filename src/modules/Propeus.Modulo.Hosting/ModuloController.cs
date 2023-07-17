using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Hosting.MS_MVC;

namespace Propeus.Modulo.Hosting
{
    //Obviamente que isto nao vai funcionar...
    internal sealed class ActionResultObjectValueAttribute : Attribute
    {

    }

    //Muito menos isto
    internal sealed class ActionResultStatusCodeAttribute : Attribute
    {

    }


    [Module]
    public class ModuloController : ModuloControllerBase, IActionFilter, IAsyncActionFilter
    {

        #region Controller
        private ITempDataDictionary _tempData;

        private DynamicViewData _viewBag;

        private ViewDataDictionary _viewData;

        //
        // Resumo:
        //     Gets or sets Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary used by
        //     Microsoft.AspNetCore.Mvc.ViewResult and Microsoft.AspNetCore.Mvc.Controller.ViewBag.
        //
        // Comentários:
        //     By default, this property is initialized when Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator
        //     activates controllers.
        //     This property can be accessed after the controller has been activated, for example,
        //     in a controller action or by overriding Microsoft.AspNetCore.Mvc.Controller.OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext).
        //     This property can be also accessed from within a unit test where it is initialized
        //     with Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider.
        [ViewDataDictionary]
        public ViewDataDictionary ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    _viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), ControllerContext.ModelState);
                }

                return _viewData;
            }
            set => _viewData = value ?? throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "ViewData");
        }

        //
        // Resumo:
        //     Gets or sets Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary used by
        //     Microsoft.AspNetCore.Mvc.ViewResult.
        public ITempDataDictionary TempData
        {
            get
            {
                if (_tempData == null)
                {
                    _tempData = (HttpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>())?.GetTempData(HttpContext);
                }

                return _tempData;
            }
            set => _tempData = value ?? throw new ArgumentNullException("value");
        }

        //
        // Resumo:
        //     Gets the dynamic view bag.
        public dynamic ViewBag
        {
            get
            {
                if (_viewBag == null)
                {
                    _viewBag = new DynamicViewData(() => ViewData);
                }

                return _viewBag;
            }
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewResult object that renders a view to the
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewResult View()
        {
            return View(null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewResult object by specifying a viewName.
        //
        // Parâmetros:
        //   viewName:
        //     The name or path of the view that is rendered to the response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewResult View(string viewName)
        {
            return View(viewName, ViewData.Model);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewResult object by specifying a model to
        //     be rendered by the view.
        //
        // Parâmetros:
        //   model:
        //     The model that is rendered by the view.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewResult View(object model)
        {
            return View(null, model);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewResult object by specifying a viewName
        //     and the model to be rendered by the view.
        //
        // Parâmetros:
        //   viewName:
        //     The name or path of the view that is rendered to the response.
        //
        //   model:
        //     The model that is rendered by the view.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewResult View(string viewName, object model)
        {
            //Vamos ter que fazer override na kcta da engine https://github.com/dotnet/aspnetcore/blob/4afe7f612d104b43b690e71d83c18a8bc48aae2d/src/Mvc/Mvc.ViewFeatures/src/ViewResultExecutor.cs#L21
            //Nem fudendo

            ViewData.Model = model;
            return new ViewResult
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.PartialViewResult object that renders a partial
        //     view to the response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PartialViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual PartialViewResult PartialView()
        {
            return PartialView(null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.PartialViewResult object by specifying a viewName.
        //
        // Parâmetros:
        //   viewName:
        //     The name or path of the partial view that is rendered to the response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PartialViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual PartialViewResult PartialView(string viewName)
        {
            return PartialView(viewName, ViewData.Model);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.PartialViewResult object by specifying a model
        //     to be rendered by the partial view.
        //
        // Parâmetros:
        //   model:
        //     The model that is rendered by the partial view.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PartialViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual PartialViewResult PartialView(object model)
        {
            return PartialView(null, model);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.PartialViewResult object by specifying a viewName
        //     and the model to be rendered by the partial view.
        //
        // Parâmetros:
        //   viewName:
        //     The name or path of the partial view that is rendered to the response.
        //
        //   model:
        //     The model that is rendered by the partial view.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PartialViewResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model)
        {
            ViewData.Model = model;
            return new PartialViewResult
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewComponentResult by specifying the name
        //     of a view component to render.
        //
        // Parâmetros:
        //   componentName:
        //     The view component name. Can be a view component Microsoft.AspNetCore.Mvc.ViewComponents.ViewComponentDescriptor.ShortName
        //     or Microsoft.AspNetCore.Mvc.ViewComponents.ViewComponentDescriptor.FullName.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewComponentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewComponentResult ViewComponent(string componentName)
        {
            return ViewComponent(componentName, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewComponentResult by specifying the System.Type
        //     of a view component to render.
        //
        // Parâmetros:
        //   componentType:
        //     The view component System.Type.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewComponentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewComponentResult ViewComponent(Type componentType)
        {
            return ViewComponent(componentType, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewComponentResult by specifying the name
        //     of a view component to render.
        //
        // Parâmetros:
        //   componentName:
        //     The view component name. Can be a view component Microsoft.AspNetCore.Mvc.ViewComponents.ViewComponentDescriptor.ShortName
        //     or Microsoft.AspNetCore.Mvc.ViewComponents.ViewComponentDescriptor.FullName.
        //
        //   arguments:
        //     An System.Object with properties representing arguments to be passed to the invoked
        //     view component method. Alternatively, an System.Collections.Generic.IDictionary`2
        //     instance containing the invocation arguments.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewComponentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewComponentResult ViewComponent(string componentName, object arguments)
        {
            return new ViewComponentResult
            {
                ViewComponentName = componentName,
                Arguments = arguments,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ViewComponentResult by specifying the System.Type
        //     of a view component to render.
        //
        // Parâmetros:
        //   componentType:
        //     The view component System.Type.
        //
        //   arguments:
        //     An System.Object with properties representing arguments to be passed to the invoked
        //     view component method. Alternatively, an System.Collections.Generic.IDictionary`2
        //     instance containing the invocation arguments.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ViewComponentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual ViewComponentResult ViewComponent(Type componentType, object arguments)
        {
            return new ViewComponentResult
            {
                ViewComponentType = componentType,
                Arguments = arguments,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parâmetros:
        //   data:
        //     The object to serialize.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the specified
        //     data to JSON format for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual JsonResult Json(object data)
        {
            return new JsonResult(data);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parâmetros:
        //   data:
        //     The object to serialize.
        //
        //   serializerSettings:
        //     The Newtonsoft.Json.JsonSerializerSettings to be used by the formatter.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the specified
        //     data as JSON format for the response.
        //
        // Comentários:
        //     Callers should cache an instance of Newtonsoft.Json.JsonSerializerSettings to
        //     avoid recreating cached data with each call.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual JsonResult Json(object data, JsonSerializerSettings serializerSettings)
        {
            if (serializerSettings == null)
            {
                throw new ArgumentNullException("serializerSettings");
            }

            return new JsonResult(data, serializerSettings);
        }

        //
        // Resumo:
        //     Called before the action method is invoked.
        //
        // Parâmetros:
        //   context:
        //     The action executing context.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
        }

        //
        // Resumo:
        //     Called after the action method is invoked.
        //
        // Parâmetros:
        //   context:
        //     The action executed context.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
        }

        //
        // Resumo:
        //     Called before the action method is invoked.
        //
        // Parâmetros:
        //   context:
        //     The action executing context.
        //
        //   next:
        //     The Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate to execute. Invoke
        //     this delegate in the body of Microsoft.AspNetCore.Mvc.Controller.OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext,Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate)
        //     to continue execution of the action.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task instance.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            OnActionExecuting(context);
            if (context.Result == null)
            {
                OnActionExecuted(await next());
            }
        }

        #endregion


    }
}
