using System.Collections.Concurrent;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Hosting.Contracts;

namespace Propeus.Module.Hosting
{
    //https://stackoverflow.com/questions/48206993/how-to-load-asp-net-core-razor-view-dynamically-at-runtime
    public class ModuleViewCompiler : IViewCompiler
    {

        public static ModuleViewCompiler Current;

        public ModuleViewCompiler(ApplicationPartManager applicationPartManager, ILoggerFactory loggerFactory)
        {
            ApplicationPartManager = applicationPartManager;
            Logger = loggerFactory.CreateLogger<ModuleViewCompiler>();
            CancellationTokenSources = new Dictionary<string, CancellationTokenSource>();
            NormalizedPathCache = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
            PopulateCompiledViews();
            Current = this;
        }

        protected ApplicationPartManager ApplicationPartManager { get; }

        protected ILogger Logger { get; }

        protected Dictionary<string, CancellationTokenSource> CancellationTokenSources { get; }

        protected ConcurrentDictionary<string, string> NormalizedPathCache { get; }

        protected Dictionary<string, CompiledViewDescriptor> CompiledViews { get; private set; }

        public void LoadModuleCompiledViews(Assembly moduleAssembly)
        {
            if (moduleAssembly == null)
                throw new ArgumentNullException(nameof(moduleAssembly));
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationTokenSources.Add(moduleAssembly.FullName, cancellationTokenSource);
            ViewsFeature feature = new ViewsFeature();
            ApplicationPartManager.PopulateFeature(feature);
            foreach (CompiledViewDescriptor compiledView in feature.ViewDescriptors
                .Where(v => v.Type.Assembly == moduleAssembly))
            {
                if (!CompiledViews.ContainsKey(compiledView.RelativePath))
                {
                    compiledView.ExpirationTokens = new List<IChangeToken>() { new CancellationChangeToken(cancellationTokenSource.Token) };
                    CompiledViews.Add(compiledView.RelativePath, compiledView);
                }
            }
        }

        public void UnloadModuleCompiledViews(Assembly moduleAssembly)
        {
            if (moduleAssembly == null)
                throw new ArgumentNullException(nameof(moduleAssembly));
            foreach (KeyValuePair<string, CompiledViewDescriptor> entry in CompiledViews
                .Where(kvp => kvp.Value.Type.Assembly.ManifestModule.ScopeName == moduleAssembly.ManifestModule.ScopeName))
            {
                CompiledViews.Remove(entry.Key);
            }
            if (CancellationTokenSources.TryGetValue(moduleAssembly.FullName, out CancellationTokenSource cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
                CancellationTokenSources.Remove(moduleAssembly.FullName);
            }
        }

        private void PopulateCompiledViews()
        {
            ViewsFeature feature = new ViewsFeature();
            ApplicationPartManager.PopulateFeature(feature);
            CompiledViews = new Dictionary<string, CompiledViewDescriptor>(feature.ViewDescriptors.Count, StringComparer.OrdinalIgnoreCase);
            foreach (CompiledViewDescriptor compiledView in feature.ViewDescriptors)
            {
                if (CompiledViews.ContainsKey(compiledView.RelativePath))
                    continue;
                CompiledViews.Add(compiledView.RelativePath, compiledView);
            };
        }

        public async Task<CompiledViewDescriptor> CompileAsync(string relativePath)
        {
            if (relativePath == null)
                throw new ArgumentNullException(nameof(relativePath));
            if (CompiledViews.TryGetValue(relativePath, out CompiledViewDescriptor cachedResult))
                return cachedResult;
            string normalizedPath = GetNormalizedPath(relativePath);
            if (CompiledViews.TryGetValue(normalizedPath, out cachedResult))
                return cachedResult;
            return await Task.FromResult(new CompiledViewDescriptor()
            {
                RelativePath = normalizedPath,
                ExpirationTokens = Array.Empty<IChangeToken>(),
            });
        }

        protected string GetNormalizedPath(string relativePath)
        {
            if (relativePath.Length == 0)
                return relativePath;
            if (!NormalizedPathCache.TryGetValue(relativePath, out var normalizedPath))
            {
                normalizedPath = NormalizePath(relativePath);
                NormalizedPathCache[relativePath] = normalizedPath;
            }
            return normalizedPath;
        }

        protected string NormalizePath(string path)
        {
            bool addLeadingSlash = path[0] != '\\' && path[0] != '/';
            bool transformSlashes = path.IndexOf('\\') != -1;
            if (!addLeadingSlash && !transformSlashes)
                return path;
            int length = path.Length;
            if (addLeadingSlash)
                length++;
            return string.Create(length, (path, addLeadingSlash), (span, tuple) =>
            {
                var (pathValue, addLeadingSlashValue) = tuple;
                int spanIndex = 0;
                if (addLeadingSlashValue)
                    span[spanIndex++] = '/';
                foreach (var ch in pathValue)
                {
                    span[spanIndex++] = ch == '\\' ? '/' : ch;
                }
            });
        }

    }
    public class ModuleViewCompilerProvider : IViewCompilerProvider
    {
        private readonly ModuloApplicationPart moduloApplicationPart;

        public ModuleViewCompilerProvider(ApplicationPartManager applicationPartManager, ILoggerFactory loggerFactory, IModuleManager moduleManager)
        {
            Compiler = new ModuleViewCompiler(applicationPartManager, loggerFactory);
            moduloApplicationPart = new ModuloApplicationPart(Compiler, moduleManager, applicationPartManager);
        }

        protected IViewCompiler Compiler { get; }

        public IViewCompiler GetCompiler()
        {
            return Compiler;
        }

    }
    //https://stackoverflow.com/questions/48206993/how-to-load-asp-net-core-razor-view-dynamically-at-runtime

    public class ModuleActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        public static ModuleActionDescriptorChangeProvider Instance { get; } = new ModuleActionDescriptorChangeProvider();

        public CancellationTokenSource TokenSource { get; private set; }

        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }

    [Module(Singleton = true)]
    internal class ModuloApplicationPart : BaseModule
    {
        private IViewCompiler ViewCompiler { get; set; }
        public ApplicationPartManager ApplicationPartManager { get; }

        private readonly bool _sincronizado = false;

        public ModuloApplicationPart(IViewCompiler ViewCompiler, IModuleManager moduleManager, ApplicationPartManager applicationPartManager) : base()
        {
            this.ViewCompiler = ViewCompiler;
            ApplicationPartManager = applicationPartManager;
            IModuleProviderModuleContract moduleProviderModuleContract;
            if (!moduleManager.ExistsModule(typeof(IModuleProviderModuleContract)))
            {
                moduleProviderModuleContract = moduleManager.CreateModule<IModuleProviderModuleContract>();
            }
            else
            {
                moduleProviderModuleContract = moduleManager.GetModule<IModuleProviderModuleContract>();
            }

            IEnumerable<Type> modules = moduleProviderModuleContract.GetAllModules();

            foreach (Type moduleType in modules)
            {
                OnLoadModuleController(moduleType);
            }
            CommitChange();


            moduleProviderModuleContract.SetOnLoadModule(OnLoadModuleController);
            moduleProviderModuleContract.SetOnUnloadModule(OnUnloadModuleController);
            moduleProviderModuleContract.SetOnRebuildModule(OnRebuildModuleController);

        }

        private void OnRebuildModuleController(Type type)
        {
            UnloadModuleController(type.Assembly);
            LoadModuleController(type.Assembly);
            CommitChange();
        }

        private void CommitChange()
        {
            ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
        }
        private void UnloadModuleController(Assembly moduleAssembly)
        {
            (ViewCompiler as ModuleViewCompiler).UnloadModuleCompiledViews(moduleAssembly);
            List<ApplicationPart> parts = new List<ApplicationPart>();
            foreach (ApplicationPart appApplicationPart in ApplicationPartManager.ApplicationParts)
            {
                if (appApplicationPart.Name == moduleAssembly.ManifestModule.ScopeName[0..^4])
                {
                    parts.Add(appApplicationPart);
                }
            }

            foreach (ApplicationPart part in parts)
            {
                ApplicationPartManager.ApplicationParts.Remove(part);
            }

        }
        private void LoadModuleController(Assembly moduleAssembly)
        {

            if (!ApplicationPartManager.ApplicationParts.Any(x => x.Name == moduleAssembly.ManifestModule.ScopeName[0..^4]))
            {
                AssemblyPart app = new AssemblyPart(moduleAssembly);
                CompiledRazorAssemblyPart razorPart = new CompiledRazorAssemblyPart(moduleAssembly);
                //Tem que carregar os dois
                ApplicationPartManager.ApplicationParts.Add(app);
                ApplicationPartManager.ApplicationParts.Add(razorPart);

                (ViewCompiler as ModuleViewCompiler).LoadModuleCompiledViews(moduleAssembly);
            }



        }

        private void OnUnloadModuleController(Type moduleType)
        {
            if (moduleType.Name.Contains("Controller"))
            {

                UnloadModuleController(moduleType.Assembly);
                CommitChange();

            }
        }

        private void OnLoadModuleController(Type moduleType)
        {
            if (moduleType.Name.Contains("Controller"))
            {
                LoadModuleController(moduleType.Assembly);

                CommitChange();
            }
        }



    }
}
