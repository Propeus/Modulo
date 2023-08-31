using Propeus.Module.DependencyInjection;

using Propeus.Modulo.WorkerService;
/**
* Este exemplo serve para mostrar como o sistema de modulo dinâmico funciona
* 
* Observe que na pasta 'examples' existem dois projetos chamado 'Propeus.Module.WorkerService.Exmaple' e 'Propeus.Module.WorkerService.Exmaple.DLL'.
* O 'Propeus.Module.WorkerService.Exmaple.DLL' e o projeto que contem o worker que sera utilizando em nossos testes.
* 
* Antes de começar, voce deve realizar o build completo caso tenha somente baixado o projeto.
* Agora para inicializar a demonstração, siga os seguintes passos:
* 
* 1 - Abra o terminal ou PowerShell no projeto 'Propeus.Module.WorkerService.Exmaple.DLL'
* 2 - Va na pasta 'Propeus.Module.WorkerService.Exmaple' e execute o programa 'Propeus.Module.WorkerService.Exmaple.exe' para iniciar este programa
* 3 - Copie a dll atual para dentro do projeto em execução executando o seguinte comando no terminal do item 1 'cp .\bin\Debug\net7.0\Propeus.Module.WorkerService.Exmaple.DLL.dll ..\Propeus.Module.WorkerService.Exmaple\bin\Debug\net7.0\' e perceba que o modulo começou a executar no momento em que foi copiado para dentro da pasta do executável
* 4 - Abra o arquivo 'Worker.cs' do projeto 'Propeus.Module.WorkerService.Exmaple.DLL' e modifique o texto 'Worker running at time: {0}' para 'Meu novo modulo funcionando no tempo: {0}'
* 5 - Salve o arquivo e execute os seguintes comandos no terminal do item 1, 'dotnet build' e depois 'cp .\bin\Debug\net7.0\Propeus.Module.WorkerService.Exmaple.DLL.dll ..\Propeus.Module.WorkerService.Exmaple\bin\Debug\net7.0\'
* 6 - Perceba que o texto antigo parou de ser exibido e o novo começou em seu lugar.
* 
* Resumo:
* O gerenciador dinâmico fica observando qualquer mudança em seus arquivos, uma vez que ele encontra um modulo valido,
* ele verifica duas propriedades no atributo 'ModuloAttribute' que sao, 'AutoInicializavel' que permite iniciar um modulo automaticamente e 'PermitirReinicializacao',
* que indica se deve parar o modulo antigo e inicializar o novo imediatamente. Ambos sao muito uteis em diversas ocasiões, entretanto deve tomar cuidado coma sua utilizacao,
* pois podem haver casos onde um modulo depende de outro modulo 'AutoInicializavel' de instancia unica, isto pode causar erros de regra de negocio por parte do gerenciador em sua aplicação.
* Ha casos também onde e desejável que um modulo antigo permaneça ativo enquanto for utilizado, mas durante novas solicitações o novo modulo sera carregado, permitindo
* que haja uma transição de modulo sem paralisar o fluxo de processamento.
* 
* Observação:
* Caso crie um modulo worker 'AutoInicializavel' definindo a propriedade 'PermitirReinicializacao' como false, vai acontecer de o gerenciador nunca parar 
* o modulo ate que ele mesmo pare por alguma circunstancia que nao seja a atualização de modulo.
* **/
IHost host = Host.CreateDefaultBuilder(args)
    //Pode usar a extensão UseModuleManager passando o modulo dinâmico
    .UseModuleManager(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager()))
    .ConfigureServices(services =>
    {
        /**
         * Ou pode adicionar diretamente na injeção de dependência do .NET
         * services.AddSingleton(Propeus.Module.Dinamico.Gerenciador.Atual(Propeus.Module.Core.Gerenciador.Atual));
         * **/

        /**
         * Voce pode também adicionar o worker do modulo no AddHostedService como um worker normal do .NET (sem uso de gerenciadores)
         * services.AddHostedService<Propeus.Modulo.WorkerService.Exmaple.DLL.Worker>();
         * **/

    })
.Build();

host.Run();
