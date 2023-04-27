using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Newtonsoft.Json;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Modulos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;

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


    [Modulo(AutoAtualizavel = true)]
    public class ModuloControllerBase : ModuloBase
    {
        #region ControllerBase
        private ControllerContext _controllerContext;

        private IModelMetadataProvider _metadataProvider;

        private IModelBinderFactory _modelBinderFactory;

        private IObjectModelValidator _objectValidator;

        private IUrlHelper _url;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpContext for the executing action.
        public HttpContext HttpContext => ControllerContext.HttpContext;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpRequest for the executing action.
        public HttpRequest Request => HttpContext?.Request;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Http.HttpResponse for the executing action.
        public HttpResponse Response => HttpContext?.Response;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Routing.RouteData for the executing action.
        public RouteData RouteData => ControllerContext.RouteData;

        //
        // Resumo:
        //     Gets the Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary that contains
        //     the state of the model and of model-binding validation.
        public ModelStateDictionary ModelState => ControllerContext.ModelState;

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ControllerContext.
        //
        // Comentários:
        //     Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator activates this property
        //     while activating controllers. If user code directly instantiates a controller,
        //     the getter returns an empty Microsoft.AspNetCore.Mvc.ControllerContext.
        [ControllerContext]
        public ControllerContext ControllerContext
        {
            get
            {
                if (_controllerContext == null)
                {
                    _controllerContext = new ControllerContext();
                }

                return _controllerContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _controllerContext = value;
            }
        }

        //
        // Resumo:
        //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider.
        public IModelMetadataProvider MetadataProvider
        {
            get
            {
                if (_metadataProvider == null)
                {
                    HttpContext httpContext = HttpContext;
                    object metadataProvider;
                    if (httpContext == null)
                    {
                        metadataProvider = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        metadataProvider = ((requestServices != null) ? ServiceProviderServiceExtensions.GetRequiredService<IModelMetadataProvider>(requestServices) : null);
                    }

                    _metadataProvider = (IModelMetadataProvider)metadataProvider;
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
        public IModelBinderFactory ModelBinderFactory
        {
            get
            {
                if (_modelBinderFactory == null)
                {
                    HttpContext httpContext = HttpContext;
                    object modelBinderFactory;
                    if (httpContext == null)
                    {
                        modelBinderFactory = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        modelBinderFactory = ((requestServices != null) ? ServiceProviderServiceExtensions.GetRequiredService<IModelBinderFactory>(requestServices) : null);
                    }

                    _modelBinderFactory = (IModelBinderFactory)modelBinderFactory;
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
        public IUrlHelper Url
        {
            get
            {
                if (_url == null)
                {
                    HttpContext httpContext = HttpContext;
                    object obj;
                    if (httpContext == null)
                    {
                        obj = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        obj = ((requestServices != null) ? ServiceProviderServiceExtensions.GetRequiredService<IUrlHelperFactory>(requestServices) : null);
                    }

                    _url = ((IUrlHelperFactory)obj)?.GetUrlHelper((ActionContext)ControllerContext);
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
        public IObjectModelValidator ObjectValidator
        {
            get
            {
                if (_objectValidator == null)
                {
                    HttpContext httpContext = HttpContext;
                    object objectValidator;
                    if (httpContext == null)
                    {
                        objectValidator = null;
                    }
                    else
                    {
                        IServiceProvider requestServices = httpContext.RequestServices;
                        objectValidator = ((requestServices != null) ? ServiceProviderServiceExtensions.GetRequiredService<IObjectModelValidator>(requestServices) : null);
                    }

                    _objectValidator = (IObjectModelValidator)objectValidator;
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
        public ClaimsPrincipal User => HttpContext?.User;

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
        [NonAction]
        public virtual StatusCodeResult StatusCode([ActionResultStatusCode] int statusCode)
        {
            return new StatusCodeResult(statusCode);
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
        [NonAction]
        public virtual ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value)
        {
            return new ObjectResult(value)
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
        [NonAction]
        public virtual ContentResult Content(string content)
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
        [NonAction]
        public virtual ContentResult Content(string content, string contentType)
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
        [NonAction]
        public virtual ContentResult Content(string content, string contentType, Encoding contentEncoding)
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
        [NonAction]
        public virtual ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            return new ContentResult
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
        [NonAction]
        public virtual NoContentResult NoContent()
        {
            return new NoContentResult();
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.OkResult object that produces an empty Microsoft.AspNetCore.Http.StatusCodes.Status200OK
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.OkResult for the response.
        [NonAction]
        public virtual OkResult Ok()
        {
            return new OkResult();
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
        [NonAction]
        public virtual OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return new OkObjectResult(value);
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
        [NonAction]
        public virtual RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url);
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
        [NonAction]
        public virtual RedirectResult RedirectPermanent(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url, permanent: true);
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
        [NonAction]
        public virtual RedirectResult RedirectPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url, permanent: false, preserveMethod: true);
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
        [NonAction]
        public virtual RedirectResult RedirectPermanentPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url, permanent: true, preserveMethod: true);
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
        [NonAction]
        public virtual LocalRedirectResult LocalRedirect(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new LocalRedirectResult(localUrl);
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
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPermanent(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new LocalRedirectResult(localUrl, permanent: true);
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
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new LocalRedirectResult(localUrl, permanent: false, preserveMethod: true);
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
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPermanentPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "localUrl");
            }

            return new LocalRedirectResult(localUrl, permanent: true, preserveMethod: true);
        }

        //
        // Resumo:
        //     Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to an action
        //     with the same name as current one. The 'controller' and 'action' names are retrieved
        //     from the ambient values of the current request.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction()
        {
            return RedirectToAction(null);
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName)
        {
            return RedirectToAction(actionName, (object)null);
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, object routeValues)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName, (object)null);
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, string fragment)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, fragment)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPreserveMethod(string actionName = null, string controllerName = null, object routeValues = null, string fragment = null)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: false, preserveMethod: true, fragment)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName)
        {
            return RedirectToActionPermanent(actionName, (object)null);
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, object routeValues)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName)
        {
            return RedirectToActionPermanent(actionName, controllerName, (object)null);
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, string fragment)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, fragment)
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
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanentPreserveMethod(string actionName = null, string controllerName = null, object routeValues = null, string fragment = null)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, preserveMethod: true, fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName)
        {
            return RedirectToRoute(routeName, (object)null);
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(object routeValues)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, object routeValues)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, string fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, object routeValues, string fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePreserveMethod(string routeName = null, object routeValues = null, string fragment = null)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: false, preserveMethod: true, fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName)
        {
            return RedirectToRoutePermanent(routeName, (object)null);
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(object routeValues)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, string fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues, string fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: true, fragment)
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
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanentPreserveMethod(string routeName = null, object routeValues = null, string fragment = null)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: true, preserveMethod: true, fragment)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName)
        {
            return RedirectToPage(pageName, (object)null);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, object routeValues)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler)
        {
            return RedirectToPage(pageName, pageHandler, (object)null);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, string fragment)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new RedirectToPageResult(pageName, pageHandler, routeValues, fragment);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName)
        {
            return RedirectToPagePermanent(pageName, (object)null);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, object routeValues)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, string fragment)
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, fragment);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePreserveMethod(string pageName, string pageHandler = null, object routeValues = null, string fragment = null)
        {
            if (pageName == null)
            {
                throw new ArgumentNullException("pageName");
            }

            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: false, preserveMethod: true, fragment);
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
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanentPreserveMethod(string pageName, string pageHandler = null, object routeValues = null, string fragment = null)
        {
            if (pageName == null)
            {
                throw new ArgumentNullException("pageName");
            }

            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, preserveMethod: true, fragment);
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, bool enableRangeProcessing)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, bool enableRangeProcessing)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, bool enableRangeProcessing)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, bool enableRangeProcessing)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
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
        [NonAction]
        public virtual UnauthorizedResult Unauthorized()
        {
            return new UnauthorizedResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response.
        [NonAction]
        public virtual UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object value)
        {
            return new UnauthorizedObjectResult(value);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundResult for the response.
        [NonAction]
        public virtual NotFoundResult NotFound()
        {
            return new NotFoundResult();
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.NotFoundObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.NotFoundObjectResult for the response.
        [NonAction]
        public virtual NotFoundObjectResult NotFound([ActionResultObjectValue] object value)
        {
            return new NotFoundObjectResult(value);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestResult for the response.
        [NonAction]
        public virtual BadRequestResult BadRequest()
        {
            return new BadRequestResult();
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
        [NonAction]
        public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
        {
            return new BadRequestObjectResult(error);
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
        [NonAction]
        public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException("modelState");
            }

            return new BadRequestObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the response.
        [NonAction]
        public virtual UnprocessableEntityResult UnprocessableEntity()
        {
            return new UnprocessableEntityResult();
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
        [NonAction]
        public virtual UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] object error)
        {
            return new UnprocessableEntityObjectResult(error);
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
        [NonAction]
        public virtual UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException("modelState");
            }

            return new UnprocessableEntityObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.ConflictResult for the response.
        [NonAction]
        public virtual ConflictResult Conflict()
        {
            return new ConflictResult();
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
        [NonAction]
        public virtual ConflictObjectResult Conflict([ActionResultObjectValue] object error)
        {
            return new ConflictObjectResult(error);
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
        [NonAction]
        public virtual ConflictObjectResult Conflict([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            return new ConflictObjectResult(modelState);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [NonAction]
        public virtual ActionResult ValidationProblem([ActionResultObjectValue] ValidationProblemDetails descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            return new BadRequestObjectResult(descriptor);
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [NonAction]
        public virtual ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException("modelStateDictionary");
            }

            return new BadRequestObjectResult(new ValidationProblemDetails(modelStateDictionary));
        }

        //
        // Resumo:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response with validation errors from Microsoft.AspNetCore.Mvc.ControllerModelState.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.
        [NonAction]
        public virtual ActionResult ValidationProblem()
        {
            return new BadRequestObjectResult(new ValidationProblemDetails(ModelState));
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
        [NonAction]
        public virtual CreatedResult Created(string uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new CreatedResult(uri, value);
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
        [NonAction]
        public virtual CreatedResult Created(Uri uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new CreatedResult(uri, value);
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
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, string controllerName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, value);
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
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(string routeName, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(object routeValues, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(string routeName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new CreatedAtRouteResult(routeName, routeValues, value);
        }

        //
        // Resumo:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // Devoluções:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        [NonAction]
        public virtual AcceptedResult Accepted()
        {
            return new AcceptedResult();
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
        [NonAction]
        public virtual AcceptedResult Accepted([ActionResultObjectValue] object value)
        {
            return new AcceptedResult((string)null, value);
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
        [NonAction]
        public virtual AcceptedResult Accepted(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new AcceptedResult(uri, null);
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
        [NonAction]
        public virtual AcceptedResult Accepted(string uri)
        {
            return new AcceptedResult(uri, null);
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
        [NonAction]
        public virtual AcceptedResult Accepted(string uri, [ActionResultObjectValue] object value)
        {
            return new AcceptedResult(uri, value);
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
        [NonAction]
        public virtual AcceptedResult Accepted(Uri uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new AcceptedResult(uri, value);
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName)
        {
            return AcceptedAtAction(actionName, (object)null, (object)null);
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName)
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, [ActionResultObjectValue] object value)
        {
            return AcceptedAtAction(actionName, (object)null, value);
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName, [ActionResultObjectValue] object routeValues)
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new AcceptedAtActionResult(actionName, controllerName, routeValues, value);
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
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute([ActionResultObjectValue] object routeValues)
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
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName)
        {
            return AcceptedAtRoute(routeName, null, null);
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
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName, object routeValues)
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
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(object routeValues, [ActionResultObjectValue] object value)
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
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new AcceptedAtRouteResult(routeName, routeValues, value);
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
        [NonAction]
        public virtual ChallengeResult Challenge()
        {
            return new ChallengeResult();
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
        [NonAction]
        public virtual ChallengeResult Challenge(params string[] authenticationSchemes)
        {
            return new ChallengeResult(authenticationSchemes);
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
        [NonAction]
        public virtual ChallengeResult Challenge(AuthenticationProperties properties)
        {
            return new ChallengeResult(properties);
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
        [NonAction]
        public virtual ChallengeResult Challenge(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new ChallengeResult(authenticationSchemes, properties);
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
        [NonAction]
        public virtual ForbidResult Forbid()
        {
            return new ForbidResult();
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
        [NonAction]
        public virtual ForbidResult Forbid(params string[] authenticationSchemes)
        {
            return new ForbidResult(authenticationSchemes);
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
        [NonAction]
        public virtual ForbidResult Forbid(AuthenticationProperties properties)
        {
            return new ForbidResult(properties);
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
        [NonAction]
        public virtual ForbidResult Forbid(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new ForbidResult(authenticationSchemes, properties);
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
        [NonAction]
        public virtual SignInResult SignIn(ClaimsPrincipal principal, string authenticationScheme)
        {
            return new SignInResult(authenticationScheme, principal);
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
        [NonAction]
        public virtual SignInResult SignIn(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
        {
            return new SignInResult(authenticationScheme, principal, properties);
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
        [NonAction]
        public virtual SignOutResult SignOut(params string[] authenticationSchemes)
        {
            return new SignOutResult(authenticationSchemes);
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
        [NonAction]
        public virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new SignOutResult(authenticationSchemes, properties);
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
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

            return await TryUpdateModelAsync(model, prefix, await CompositeValueProvider.CreateAsync(ControllerContext));
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider) where TModel : class
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
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

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
        public async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Func<ModelMetadata, bool> propertyFilter) where TModel : class
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (propertyFilter == null)
            {
                throw new ArgumentNullException("propertyFilter");
            }

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
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

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, includeExpressions);
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
        // Parâmetros de Tipo:
        //   TModel:
        //     The type of the model object.
        //
        // Devoluções:
        //     A System.Threading.Tasks.Task that on completion returns true if the update is
        //     successful.
        [NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider, Func<ModelMetadata, bool> propertyFilter) where TModel : class
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
        [NonAction]
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

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
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
        [NonAction]
        public Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix, IValueProvider valueProvider, Func<ModelMetadata, bool> propertyFilter)
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
        [NonAction]
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
        [NonAction]
        public virtual bool TryValidateModel(object model, string prefix)
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


    [Modulo]
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
            set
            {
                if (value == null)
                {
                    throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "ViewData");
                }

                _viewData = value;
            }
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
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _tempData = value;
            }
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
        public virtual ViewResult View(string viewName, object model)
        {
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
