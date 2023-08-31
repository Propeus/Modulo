using Propeus.Module.DependencyInjection;

using Propeus.Modulo.WorkerService;
/**
* Este exemplo serve para mostrar como o sistema de modulo din�mico funciona
* 
* Observe que na pasta 'examples' existem dois projetos chamado 'Propeus.Module.WorkerService.Exmaple' e 'Propeus.Module.WorkerService.Exmaple.DLL'.
* O 'Propeus.Module.WorkerService.Exmaple.DLL' e o projeto que contem o worker que sera utilizando em nossos testes.
* 
* Antes de come�ar, voce deve realizar o build completo caso tenha somente baixado o projeto.
* Agora para inicializar a demonstra��o, siga os seguintes passos:
* 
* 1 - Abra o terminal ou PowerShell no projeto 'Propeus.Module.WorkerService.Exmaple.DLL'
* 2 - Va na pasta 'Propeus.Module.WorkerService.Exmaple' e execute o programa 'Propeus.Module.WorkerService.Exmaple.exe' para iniciar este programa
* 3 - Copie a dll atual para dentro do projeto em execu��o executando o seguinte comando no terminal do item 1 'cp .\bin\Debug\net7.0\Propeus.Module.WorkerService.Exmaple.DLL.dll ..\Propeus.Module.WorkerService.Exmaple\bin\Debug\net7.0\' e perceba que o modulo come�ou a executar no momento em que foi copiado para dentro da pasta do execut�vel
* 4 - Abra o arquivo 'Worker.cs' do projeto 'Propeus.Module.WorkerService.Exmaple.DLL' e modifique o texto 'Worker running at time: {0}' para 'Meu novo modulo funcionando no tempo: {0}'
* 5 - Salve o arquivo e execute os seguintes comandos no terminal do item 1, 'dotnet build' e depois 'cp .\bin\Debug\net7.0\Propeus.Module.WorkerService.Exmaple.DLL.dll ..\Propeus.Module.WorkerService.Exmaple\bin\Debug\net7.0\'
* 6 - Perceba que o texto antigo parou de ser exibido e o novo come�ou em seu lugar.
* 
* Resumo:
* O gerenciador din�mico fica observando qualquer mudan�a em seus arquivos, uma vez que ele encontra um modulo valido,
* ele verifica duas propriedades no atributo 'ModuloAttribute' que sao, 'AutoInicializavel' que permite iniciar um modulo automaticamente e 'PermitirReinicializacao',
* que indica se deve parar o modulo antigo e inicializar o novo imediatamente. Ambos sao muito uteis em diversas ocasi�es, entretanto deve tomar cuidado coma sua utilizacao,
* pois podem haver casos onde um modulo depende de outro modulo 'AutoInicializavel' de instancia unica, isto pode causar erros de regra de negocio por parte do gerenciador em sua aplica��o.
* Ha casos tamb�m onde e desej�vel que um modulo antigo permane�a ativo enquanto for utilizado, mas durante novas solicita��es o novo modulo sera carregado, permitindo
* que haja uma transi��o de modulo sem paralisar o fluxo de processamento.
* 
* Observa��o:
* Caso crie um modulo worker 'AutoInicializavel' definindo a propriedade 'PermitirReinicializacao' como false, vai acontecer de o gerenciador nunca parar 
* o modulo ate que ele mesmo pare por alguma circunstancia que nao seja a atualiza��o de modulo.
* **/
IHost host = Host.CreateDefaultBuilder(args)
    //Pode usar a extens�o UseModuleManager passando o modulo din�mico
    .UseModuleManager(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager()))
    .ConfigureServices(services =>
    {
        /**
         * Ou pode adicionar diretamente na inje��o de depend�ncia do .NET
         * services.AddSingleton(Propeus.Module.Dinamico.Gerenciador.Atual(Propeus.Module.Core.Gerenciador.Atual));
         * **/

        /**
         * Voce pode tamb�m adicionar o worker do modulo no AddHostedService como um worker normal do .NET (sem uso de gerenciadores)
         * services.AddHostedService<Propeus.Modulo.WorkerService.Exmaple.DLL.Worker>();
         * **/

    })
.Build();

host.Run();
