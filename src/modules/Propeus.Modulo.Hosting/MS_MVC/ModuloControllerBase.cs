using System.Linq.Expressions;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.Hosting.MS_MVC
{
    [Module(AutoUpdate = true)]
    public class ModuloControllerBase : BaseModule
    {
        #region ControllerBase
        private Microsoft.AspNetCore.Mvc.ControllerContext? _controllerContext;

        private Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider? _metadataProvider;

        private Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory? _modelBinderFactory;

        private Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator? _objectValidator;

        private Microsoft.AspNetCore.Mvc.IUrlHelper? _url;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpContext for the executing action.
        public Microsoft.AspNetCore.Http.HttpContext HttpContext => ControllerContext.HttpContext;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpRequest for the executing action.
        public Microsoft.AspNetCore.Http.HttpRequest Request => HttpContext.Request;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpResponse for the executing action.
        public Microsoft.AspNetCore.Http.HttpResponse Response => HttpContext.Response;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Routing.RouteData for the executing action.
        public Microsoft.AspNetCore.Routing.RouteData RouteData => ControllerContext.RouteData;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary that contains
        //     the state of the model and of model-binding validation.
        public Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState => ControllerContext.ModelState;

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ControllerContext.
        //
        // Comentários:
        //     Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator activates this property
        //     while activating controllers. If user code directly instantiates a controller,
        //     the getter returns an empty Microsoft.AspNetCore.Mvc.ControllerContext.
        [Microsoft.AspNetCore.Mvc.ControllerContext]
        public Microsoft.AspNetCore.Mvc.ControllerContext ControllerContext
        {
            get
            {
                if (_controllerContext == null)
                {
                    _controllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
                }

                return _controllerContext;
            }
            set => _controllerContext = value ?? throw new ArgumentNullException("value");
        }

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider.
        public Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider? MetadataProvider
        {
            get
            {
                if (_metadataProvider == null)
                {
                    Microsoft.AspNetCore.Http.HttpContext httpContext = HttpContext;
                    object? metadataProvider;
                    if (httpContext == null)
                    {
                        metadataProvider = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        metadataProvider = requestServices?.GetRequiredService<Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider>();
                    }

                    _metadataProvider = metadataProvider as Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider;
                }

                return _metadataProvider;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _metadataProvider = value;
            }
        }

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory.
        public Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory? ModelBinderFactory
        {
            get
            {
                if (_modelBinderFactory == null)
                {
                    Microsoft.AspNetCore.Http.HttpContext httpContext = HttpContext;
                    object? modelBinderFactory;
                    if (httpContext == null)
                    {
                        modelBinderFactory = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        modelBinderFactory = requestServices?.GetRequiredService<Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory>();
                    }

                    _modelBinderFactory = modelBinderFactory as Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory;
                }

                return _modelBinderFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _modelBinderFactory = value;
            }
        }

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.IUrlHelper.
        public Microsoft.AspNetCore.Mvc.IUrlHelper? Url
        {
            get
            {
                if (_url is not null)
                {
                    Microsoft.AspNetCore.Http.HttpContext httpContext = HttpContext;
                    object? obj;
                    if (httpContext == null)
                    {
                        obj = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        obj = requestServices?.GetRequiredService<Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory>();
                    }

                    _url = (obj as Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory)?.GetUrlHelper(ControllerContext);
                }

                return _url;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _url = value;
            }
        }

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator.
        public Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator? ObjectValidator
        {
            get
            {
                if (_objectValidator == null)
                {
                    Microsoft.AspNetCore.Http.HttpContext httpContext = HttpContext;
                    object? objectValidator;
                    if (httpContext == null)
                    {
                        objectValidator = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        objectValidator = requestServices?.GetRequiredService<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator>();
                    }

                    _objectValidator = objectValidator as Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator;
                }

                return _objectValidator;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _objectValidator = value;
            }
        }

        //
        // Resumo:
        //     Gets the System.Security.Claims.ClaimsPrincipal for user associated with the
        //     executing action.
        public System.Security.Claims.ClaimsPrincipal User => HttpContext?.User;

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.StatusCodeResult object by specifying a statusCode.
        //
        // Parâmetros:
        //   statusCode:
        //     The status code to set on the response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.StatusCodeResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.StatusCodeResult StatusCode([ActionResultStatusCode] int statusCode)
        {
            return new Microsoft.AspNetCore.Mvc.StatusCodeResult(statusCode);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ObjectResult object by specifying a statusCode
        //     and value
        //
        // Parâmetros:
        //   statusCode:
        //     The status code to set on the response.
        //
        //   value:
        //     The value to set on the Microsoft.AspNetCore.Mvc.ObjectResult.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ObjectResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.ObjectResult(value)
            {
                StatusCode = statusCode
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     by specifying a content string.
        //
        // Parâmetros:
        //   content:
        //     The content to write to the response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ContentResult Content(string content)
        {
            return Content(content, (MediaTypeHeaderValue)null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     by specifying a content string and a content type.
        //
        // Parâmetros:
        //   content:
        //     The content to write to the response.
        //
        //   contentType:
        //     The content type (MIME type).
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ContentResult Content(string content, string contentType)
        {
            return Content(content, MediaTypeHeaderValue.Parse(contentType));
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     by specifying a content string, a contentType, and contentEncoding.
        //
        // Parâmetros:
        //   content:
        //     The content to write to the response.
        //
        //   contentType:
        //     The content type (MIME type).
        //
        //   contentEncoding:
        //     The content encoding.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.
        //
        // Comentários:
        //     If encoding is provided by both the 'charset' and the contentEncoding parameters,
        //     then the contentEncoding parameter is chosen as the final encoding.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            MediaTypeHeaderValue mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);
            mediaTypeHeaderValue.Encoding = contentEncoding ?? mediaTypeHeaderValue.Encoding;
            return Content(content, mediaTypeHeaderValue);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     by specifying a content string and a contentType.
        //
        // Parâmetros:
        //   content:
        //     The content to write to the response.
        //
        //   contentType:
        //     The content type (MIME type).
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            return new Microsoft.AspNetCore.Mvc.ContentResult
            {
                Content = content,
                ContentType = contentType?.ToString()
            };
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.NoContentResult object that produces an empty
        //     Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.NoContentResult object for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.NoContentResult NoContent()
        {
            return new Microsoft.AspNetCore.Mvc.NoContentResult();
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.OkResult object that produces an empty Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.OkResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.OkResult Ok()
        {
            return new Microsoft.AspNetCore.Mvc.OkResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.OkObjectResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     response.
        //
        // Parâmetros:
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.OkObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.OkObjectResult(value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.RedirectResult object that redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found)
        //     to the specified url.
        //
        // Parâmetros:
        //   url:
        //     The URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectResult(url);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent
        //     set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently)
        //     using the specified url.
        //
        // Parâmetros:
        //   url:
        //     The URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectResult RedirectPermanent(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectResult(url, permanent: true);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent
        //     set to false and Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod set to
        //     true (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect) using
        //     the specified url.
        //
        // Parâmetros:
        //   url:
        //     The URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectResult RedirectPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectResult(url, permanent: false, preserveMethod: true);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent
        //     set to true and Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod set to
        //     true (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect) using
        //     the specified url.
        //
        // Parâmetros:
        //   url:
        //     The URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectResult RedirectPermanentPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectResult(url, permanent: true, preserveMethod: true);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object that redirects
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified local
        //     localUrl.
        //
        // Parâmetros:
        //   localUrl:
        //     The local URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.LocalRedirectResult LocalRedirect(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new Microsoft.AspNetCore.Mvc.LocalRedirectResult(localUrl);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        //     set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently)
        //     using the specified localUrl.
        //
        // Parâmetros:
        //   localUrl:
        //     The local URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.LocalRedirectResult LocalRedirectPermanent(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new Microsoft.AspNetCore.Mvc.LocalRedirectResult(localUrl, permanent: true);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        //     set to false and Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod
        //     set to true (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        //     using the specified localUrl.
        //
        // Parâmetros:
        //   localUrl:
        //     The local URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.LocalRedirectResult LocalRedirectPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new Microsoft.AspNetCore.Mvc.LocalRedirectResult(localUrl, permanent: false, preserveMethod: true);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        //     set to true and Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod set
        //     to true (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect) using
        //     the specified localUrl.
        //
        // Parâmetros:
        //   localUrl:
        //     The local URL to redirect to.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.LocalRedirectResult LocalRedirectPermanentPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new Microsoft.AspNetCore.Mvc.LocalRedirectResult(localUrl, permanent: true, preserveMethod: true);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to an action
        //     with the same name as current one. The 'controller' and 'action' names are retrieved
        //     from the ambient values of the current request.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction()
        {
            return RedirectToAction(default);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the actionName.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName)
        {
            return RedirectToAction(actionName, default);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the actionName and routeValues.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName, object routeValues)
        {
            return RedirectToAction(actionName, null, routeValues);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the actionName and the controllerName.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the specified actionName, controllerName, and routeValues.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues)
        {
            return RedirectToAction(actionName, controllerName, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the specified actionName, controllerName, and fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName, string controllerName, string fragment)
        {
            return RedirectToAction(actionName, controllerName, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     action using the specified actionName, controllerName, routeValues, and fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToActionResult(actionName, controllerName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        //     to the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to false and Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod
        //     set to true, using the specified actionName, controllerName, routeValues, and
        //     fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPreserveMethod(string actionName = null, string controllerName = null, object routeValues = null, string fragment = null)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToActionResult(actionName, controllerName, routeValues, permanent: false, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName)
        {
            return RedirectToActionPermanent(actionName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName and routeValues.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName, object routeValues)
        {
            return RedirectToActionPermanent(actionName, null, routeValues);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName and controllerName.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName)
        {
            return RedirectToActionPermanent(actionName, controllerName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName, controllerName, and fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, string fragment)
        {
            return RedirectToActionPermanent(actionName, controllerName, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName, controllerName, and routeValues.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues)
        {
            return RedirectToActionPermanent(actionName, controllerName, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true using the specified actionName, controllerName, routeValues, and
        //     fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        //     to the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        //     set to true and Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod
        //     set to true, using the specified actionName, controllerName, routeValues, and
        //     fragment.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToActionResult RedirectToActionPermanentPreserveMethod(string actionName = null, string controllerName = null, object routeValues = null, string fragment = null)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoute(string routeName)
        {
            return RedirectToRoute(routeName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeValues.
        //
        // Parâmetros:
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoute(object routeValues)
        {
            return RedirectToRoute(null, routeValues);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName and routeValues.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoute(string routeName, object routeValues)
        {
            return RedirectToRoute(routeName, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoute(string routeName, string fragment)
        {
            return RedirectToRoute(routeName, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     route using the specified routeName, routeValues, and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoute(string routeName, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToRouteResult(routeName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        //     to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to false and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        //     set to true, using the specified routeName, routeValues, and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePreserveMethod(string routeName = null, object routeValues = null, string fragment = null)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToRouteResult(routeName, routeValues, permanent: false, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanent(string routeName)
        {
            return RedirectToRoutePermanent(routeName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeValues.
        //
        // Parâmetros:
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanent(object routeValues)
        {
            return RedirectToRoutePermanent(null, routeValues);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName and routeValues.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues)
        {
            return RedirectToRoutePermanent(routeName, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanent(string routeName, string fragment)
        {
            return RedirectToRoutePermanent(routeName, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true using the specified routeName, routeValues, and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToRouteResult(routeName, routeValues, permanent: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        //     to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        //     set to true, using the specified routeName, routeValues, and fragment.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToRouteResult RedirectToRoutePermanentPreserveMethod(string routeName = null, object routeValues = null, string fragment = null)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToRouteResult(routeName, routeValues, permanent: true, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName)
        {
            return RedirectToPage(pageName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName using the specified routeValues.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName, object routeValues)
        {
            return RedirectToPage(pageName, null, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName using the specified pageHandler.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName, string pageHandler)
        {
            return RedirectToPage(pageName, pageHandler, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues)
        {
            return RedirectToPage(pageName, pageHandler, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName using the specified fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName, string pageHandler, string fragment)
        {
            return RedirectToPage(pageName, pageHandler, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        //     pageName using the specified routeValues and fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToPageResult(pageName, pageHandler, routeValues, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified pageName.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent
        //     set.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanent(string pageName)
        {
            return RedirectToPagePermanent(pageName, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified pageName using the specified routeValues.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent
        //     set.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanent(string pageName, object routeValues)
        {
            return RedirectToPagePermanent(pageName, null, routeValues, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified pageName using the specified pageHandler.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent
        //     set.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler)
        {
            return RedirectToPagePermanent(pageName, pageHandler, null, null);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified pageName using the specified fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent
        //     set.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, string fragment)
        {
            return RedirectToPagePermanent(pageName, pageHandler, null, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        //     the specified pageName using the specified routeValues and fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   routeValues:
        //     The parameters for a route.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent
        //     set.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new Microsoft.AspNetCore.Mvc.RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        //     to the specified page with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to false and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        //     set to true, using the specified pageName, routeValues, and fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePreserveMethod(string pageName, string pageHandler = null, object routeValues = null, string fragment = null)
        {
            if (pageName == null)
            {
                throw new ArgumentNullException("pageName");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectToPageResult(pageName, pageHandler, routeValues, permanent: false, preserveMethod: true, fragment);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        //     to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        //     set to true and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        //     set to true, using the specified pageName, routeValues, and fragment.
        //
        // Parâmetros:
        //   pageName:
        //     The name of the page.
        //
        //   pageHandler:
        //     The page handler to redirect to.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   fragment:
        //     The fragment to add to the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult RedirectToPagePermanentPreserveMethod(string pageName, string pageHandler = null, object routeValues = null, string fragment = null)
        {
            if (pageName == null)
            {
                throw new ArgumentNullException("pageName");
            }

            return new Microsoft.AspNetCore.Mvc.RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, preserveMethod: true, fragment);
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType)
        {
            return File(fileContents, contentType, null);
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, bool enableRangeProcessing)
        {
            return File(fileContents, contentType, null, enableRangeProcessing);
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileContents:
        //     The file contents.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType)
        {
            return File(fileStream, contentType, null);
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, bool enableRangeProcessing)
        {
            return File(fileStream, contentType, null, enableRangeProcessing);
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   fileStream:
        //     The System.IO.Stream with the contents of the file.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType)
        {
            return File(virtualPath, contentType, null);
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, bool enableRangeProcessing)
        {
            return File(virtualPath, contentType, null, enableRangeProcessing);
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   virtualPath:
        //     The virtual path of the file to be returned.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType)
        {
            return PhysicalFile(physicalPath, contentType, null);
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, bool enableRangeProcessing)
        {
            return PhysicalFile(physicalPath, contentType, null, enableRangeProcessing);
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        //     with the specified contentType as the Content-Type and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     and the specified contentType as the Content-Type. This supports range requests
        //     (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        //     if the range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        //
        // Resumo:
        //     Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        //     the specified contentType as the Content-Type, and the specified fileDownloadName
        //     as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        //     or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        //     range is not satisfiable).
        //
        // Parâmetros:
        //   physicalPath:
        //     The path to the file. The path must be an absolute path.
        //
        //   contentType:
        //     The Content-Type of the file.
        //
        //   fileDownloadName:
        //     The suggested file name.
        //
        //   lastModified:
        //     The System.DateTimeOffset of when the file was last modified.
        //
        //   entityTag:
        //     The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.
        //
        //   enableRangeProcessing:
        //     Set to true to enable range requests processing.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new Microsoft.AspNetCore.Mvc.PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedResult that produces an Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.UnauthorizedResult Unauthorized()
        {
            return new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult(value);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.NotFoundResult NotFound()
        {
            return new Microsoft.AspNetCore.Mvc.NotFoundResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.NotFoundObjectResult NotFound([ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.NotFoundObjectResult(value);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.BadRequestResult BadRequest()
        {
            return new Microsoft.AspNetCore.Mvc.BadRequestResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Parâmetros:
        //   error:
        //     An error object to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.BadRequestObjectResult BadRequest([ActionResultObjectValue] object? error)
        {
            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(error);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Parâmetros:
        //   modelState:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors
        //     to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.BadRequestObjectResult BadRequest([ActionResultObjectValue] Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException("modelState");
            }

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.UnprocessableEntityResult UnprocessableEntity()
        {
            return new Microsoft.AspNetCore.Mvc.UnprocessableEntityResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        //
        // Parâmetros:
        //   error:
        //     An error object to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult for the
        //     response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] object? error)
        {
            return new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(error);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        //
        // Parâmetros:
        //   modelState:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors
        //     to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult for the
        //     response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException("modelState");
            }

            return new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ConflictResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ConflictResult Conflict()
        {
            return new Microsoft.AspNetCore.Mvc.ConflictResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Parâmetros:
        //   error:
        //     Contains errors to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ConflictObjectResult Conflict([ActionResultObjectValue] object? error)
        {
            return new Microsoft.AspNetCore.Mvc.ConflictObjectResult(error);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Parâmetros:
        //   modelState:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors
        //     to be returned to the client.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ConflictObjectResult Conflict([ActionResultObjectValue] Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            return new Microsoft.AspNetCore.Mvc.ConflictObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ActionResult ValidationProblem([ActionResultObjectValue] Microsoft.AspNetCore.Mvc.ValidationProblemDetails descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(descriptor);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ActionResult ValidationProblem([ActionResultObjectValue] Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException("modelStateDictionary");
            }

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(modelStateDictionary));
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response with validation errors from Microsoft.AspNetCore.Mvc.ControllerModelState.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ActionResult ValidationProblem()
        {
            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(ModelState));
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedResult object that produces a Microsoft.AspNetCore.Http.StatusCodes.Status201Created
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The URI at which the content has been created.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedResult Created(string uri, [ActionResultObjectValue] object? value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new Microsoft.AspNetCore.Mvc.CreatedResult(uri, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedResult object that produces a Microsoft.AspNetCore.Http.StatusCodes.Status201Created
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The URI at which the content has been created.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedResult Created(Uri uri, [ActionResultObjectValue] object? value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new Microsoft.AspNetCore.Mvc.CreatedResult(uri, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtActionResult CreatedAtAction(string actionName, [ActionResultObjectValue] object? value)
        {
            return CreatedAtAction(actionName, null, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtActionResult CreatedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object? value)
        {
            return CreatedAtAction(actionName, null, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   controllerName:
        //     The name of the controller to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtActionResult CreatedAtAction(string actionName, string controllerName, object routeValues, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.CreatedAtActionResult(actionName, controllerName, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtRouteResult CreatedAtRoute(string routeName, [ActionResultObjectValue] object? value)
        {
            return CreatedAtRoute(routeName, null, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtRouteResult CreatedAtRoute(object routeValues, [ActionResultObjectValue] object? value)
        {
            return CreatedAtRoute(null, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        //     a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The content value to format in the entity body.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.CreatedAtRouteResult CreatedAtRoute(string routeName, object routeValues, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.CreatedAtRouteResult(routeName, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted()
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedResult();
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Parâmetros:
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted([ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedResult(default(string), value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The optional URI with the location at which the status of requested content can
        //     be monitored. May be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new Microsoft.AspNetCore.Mvc.AcceptedResult(uri, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The optional URI with the location at which the status of requested content can
        //     be monitored. May be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted(string uri)
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedResult(uri, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The URI with the location at which the status of requested content can be monitored.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted(string uri, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedResult(uri, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Parâmetros:
        //   uri:
        //     The URI with the location at which the status of requested content can be monitored.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedResult Accepted(Uri uri, [ActionResultObjectValue] object? value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new Microsoft.AspNetCore.Mvc.AcceptedResult(uri, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName)
        {
            return AcceptedAtAction(actionName, null, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   controllerName:
        //     The name of the controller to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName)
        {
            return AcceptedAtAction(actionName, controllerName, null, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName, [ActionResultObjectValue] object? value)
        {
            return AcceptedAtAction(actionName, null, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   controllerName:
        //     The name of the controller to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName, string? controllerName, [ActionResultObjectValue] object? routeValues)
        {
            return AcceptedAtAction(actionName, controllerName, routeValues, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object? value)
        {
            return AcceptedAtAction(actionName, null, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   actionName:
        //     The name of the action to use for generating the URL.
        //
        //   controllerName:
        //     The name of the controller to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtActionResult AcceptedAtAction(string actionName, string? controllerName, object? routeValues, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedAtActionResult(actionName, controllerName, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult AcceptedAtRoute([ActionResultObjectValue] object? routeValues)
        {
            return AcceptedAtRoute(null, routeValues, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult AcceptedAtRoute(string routeName)
        {
            return AcceptedAtRoute(routeName, null, value: null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult AcceptedAtRoute(string routeName, object routeValues)
        {
            return AcceptedAtRoute(routeName, routeValues, null);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult AcceptedAtRoute(object routeValues, [ActionResultObjectValue] object? value)
        {
            return AcceptedAtRoute(null, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        //     an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        //
        // Parâmetros:
        //   routeName:
        //     The name of the route to use for generating the URL.
        //
        //   routeValues:
        //     The route data to use for generating the URL.
        //
        //   value:
        //     The optional content value to format in the entity body; may be null.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult AcceptedAtRoute(string? routeName, object? routeValues, [ActionResultObjectValue] object? value)
        {
            return new Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult(routeName, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ChallengeResult.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.
        //
        // Comentários:
        //     The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        //     in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     are among likely status results.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ChallengeResult Challenge()
        {
            return new Microsoft.AspNetCore.Mvc.ChallengeResult();
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified authentication
        //     schemes.
        //
        // Parâmetros:
        //   authenticationSchemes:
        //     The authentication schemes to challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.
        //
        // Comentários:
        //     The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        //     in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     are among likely status results.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ChallengeResult Challenge(params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.ChallengeResult(authenticationSchemes);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified properties.
        //
        // Parâmetros:
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the authentication challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.
        //
        // Comentários:
        //     The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        //     in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     are among likely status results.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ChallengeResult Challenge(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
        {
            return new Microsoft.AspNetCore.Mvc.ChallengeResult(properties);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified authentication
        //     schemes and properties.
        //
        // Parâmetros:
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the authentication challenge.
        //
        //   authenticationSchemes:
        //     The authentication schemes to challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.
        //
        // Comentários:
        //     The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        //     in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     are among likely status results.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ChallengeResult Challenge(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.ChallengeResult(authenticationSchemes, properties);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     by default).
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.
        //
        // Comentários:
        //     Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     to a redirect to show a login page.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ForbidResult Forbid()
        {
            return new Microsoft.AspNetCore.Mvc.ForbidResult();
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     by default) with the specified authentication schemes.
        //
        // Parâmetros:
        //   authenticationSchemes:
        //     The authentication schemes to challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.
        //
        // Comentários:
        //     Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     to a redirect to show a login page.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ForbidResult Forbid(params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.ForbidResult(authenticationSchemes);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     by default) with the specified properties.
        //
        // Parâmetros:
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the authentication challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.
        //
        // Comentários:
        //     Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     to a redirect to show a login page.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ForbidResult Forbid(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
        {
            return new Microsoft.AspNetCore.Mvc.ForbidResult(properties);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     by default) with the specified authentication schemes and properties.
        //
        // Parâmetros:
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the authentication challenge.
        //
        //   authenticationSchemes:
        //     The authentication schemes to challenge.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.
        //
        // Comentários:
        //     Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        //     to a redirect to show a login page.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.ForbidResult Forbid(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.ForbidResult(authenticationSchemes, properties);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.SignInResult with the specified authentication
        //     scheme.
        //
        // Parâmetros:
        //   principal:
        //     The System.Security.Claims.ClaimsPrincipal containing the user claims.
        //
        //   authenticationScheme:
        //     The authentication scheme to use for the sign-in operation.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.SignInResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.SignInResult SignIn(System.Security.Claims.ClaimsPrincipal principal, string authenticationScheme)
        {
            return new Microsoft.AspNetCore.Mvc.SignInResult(authenticationScheme, principal);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.SignInResult with the specified authentication
        //     scheme and properties.
        //
        // Parâmetros:
        //   principal:
        //     The System.Security.Claims.ClaimsPrincipal containing the user claims.
        //
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the sign-in operation.
        //
        //   authenticationScheme:
        //     The authentication scheme to use for the sign-in operation.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.SignInResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.SignInResult SignIn(System.Security.Claims.ClaimsPrincipal principal, Microsoft.AspNetCore.Authentication.AuthenticationProperties properties, string authenticationScheme)
        {
            return new Microsoft.AspNetCore.Mvc.SignInResult(authenticationScheme, principal, properties);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.SignOutResult with the specified authentication
        //     schemes.
        //
        // Parâmetros:
        //   authenticationSchemes:
        //     The authentication schemes to use for the sign-out operation.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.SignOutResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.SignOutResult SignOut(params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.SignOutResult(authenticationSchemes);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.SignOutResult with the specified authentication
        //     schemes and properties.
        //
        // Parâmetros:
        //   properties:
        //     Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform
        //     the sign-out operation.
        //
        //   authenticationSchemes:
        //     The authentication scheme to use for the sign-out operation.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.SignOutResult for the response.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Microsoft.AspNetCore.Mvc.SignOutResult SignOut(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new Microsoft.AspNetCore.Mvc.SignOutResult(authenticationSchemes, properties);
        }

        //
        // Resumo:
        //     Updates the specified model instance using values from the controller's current
        //     Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            return TryUpdateModelAsync(model, string.Empty);
        }

        //
        // Resumo:
        //     Updates the specified model instance using values from the controller's current
        //     Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            return await TryUpdateModelAsync(model, prefix, await Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider.CreateAsync(ControllerContext));
        }

        //
        // Resumo:
        //     Updates the specified model instance using the valueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the valueProvider.
        //
        //   valueProvider:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up
        //     values.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider valueProvider) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator);
        }

        //
        // Resumo:
        //     Updates the specified model instance using values from the controller's current
        //     Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        //
        //   includeExpressions:
        //     System.Linq.Expressions.Expression(s) which represent top-level properties which
        //     need to be included for the current model.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (includeExpressions == null)
            {
                throw new ArgumentNullException("includeExpressions");
            }

            Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider valueProvider = await Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, includeExpressions);
        }

        //
        // Resumo:
        //     Updates the specified model instance using values from the controller's current
        //     Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        //
        //   propertyFilter:
        //     A predicate which can be used to filter properties at runtime.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Func<Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata, bool> propertyFilter) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (propertyFilter == null)
            {
                throw new ArgumentNullException("propertyFilter");
            }

            Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider valueProvider = await Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        //
        // Resumo:
        //     Updates the specified model instance using the valueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the valueProvider.
        //
        //   valueProvider:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up
        //     values.
        //
        //   includeExpressions:
        //     System.Linq.Expressions.Expression(s) which represent top-level properties which
        //     need to be included for the current model.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider valueProvider, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }

            if (includeExpressions == null)
            {
                throw new ArgumentNullException("includeExpressions");
            }

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, includeExpressions: includeExpressions);
        }

        //
        // Resumo:
        //     Updates the specified model instance using the valueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the valueProvider.
        //
        //   valueProvider:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up
        //     values.
        //
        //   propertyFilter:
        //     A predicate which can be used to filter properties at runtime.
        //
        // Parâmetros de ModuleType:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider valueProvider, Func<Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata, bool> propertyFilter) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }

            if (propertyFilter == null)
            {
                throw new ArgumentNullException("propertyFilter");
            }

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        //
        // Resumo:
        //     Updates the specified model instance using values from the controller's current
        //     Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   modelType:
        //     The type of model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual async Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider valueProvider = await Microsoft.AspNetCore.Mvc.ModelBinding.CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator);
        }

        //
        // Resumo:
        //     Updates the specified model instance using the valueProvider and a prefix.
        //
        // Parâmetros:
        //   model:
        //     The model instance to update.
        //
        //   modelType:
        //     The type of model instance to update.
        //
        //   prefix:
        //     The prefix to use when looking up values in the valueProvider.
        //
        //   valueProvider:
        //     The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up
        //     values.
        //
        //   propertyFilter:
        //     A predicate which can be used to filter properties at runtime.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix, Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider valueProvider, Func<Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata, bool> propertyFilter)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }

            if (propertyFilter == null)
            {
                throw new ArgumentNullException("propertyFilter");
            }

            return ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        //
        // Resumo:
        //     Validates the specified model instance.
        //
        // Parâmetros:
        //   model:
        //     The model to validate.
        //
        // Devoluções:
        //     true if the Microsoft.AspNetCore.Mvc.ControllerModelState is valid; false
        //     otherwise.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual bool TryValidateModel(object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            return TryValidateModel(model, null);
        }

        //
        // Resumo:
        //     Validates the specified model instance.
        //
        // Parâmetros:
        //   model:
        //     The model to validate.
        //
        //   prefix:
        //     The key to use when looking up information in Microsoft.AspNetCore.Mvc.ControllerModelState.
        //
        // Devoluções:
        //     true if the Microsoft.AspNetCore.Mvc.ControllerModelState is valid;false
        //     otherwise.
        [Microsoft.AspNetCore.Mvc.NonAction]
        public virtual bool TryValidateModel(object model, string? prefix)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            ObjectValidator.Validate(ControllerContext, null, prefix ?? string.Empty, model);
            return ModelState.IsValid;
        }
        #endregion
    }
}
