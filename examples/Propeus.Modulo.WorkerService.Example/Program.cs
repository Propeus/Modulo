
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.WorkerService;

/**
 * Este exemplo serve para mostrar como o sistema de modulo dinamico funciona
 * 
 * Observe que na pasta 'examples' existem dois projetos chamado 'Propeus.Modulo.WorkerService.Exmaple' e 'Propeus.Modulo.WorkerService.Exmaple.DLL'.
 * O 'Propeus.Modulo.WorkerService.Exmaple.DLL' e o projeto que contem o worker que sera utilizando em nossos testes.
 * 
 * Antes de comcear, voce deve realizar o build completo caso tenha somente baixado o projeto.
 * Agora para inicializar a demonstracao, siga os seguintes passos:
 * 
 * 1 - Abra o terminal ou powerhsell no projeto 'Propeus.Modulo.WorkerService.Exmaple.DLL'
 * 2 - Va na pasta 'Propeus.Modulo.WorkerService.Exmaple' e execute o programa 'Propeus.Modulo.WorkerService.Exmaple.exe' para iniciar este programa
 * 3 - Copie a dll atual para dentro do projeto em execucao executando o seguinte comando no terminal do item 1 'cp .\bin\Debug\net7.0\Propeus.Modulo.WorkerService.Exmaple.DLL.dll ..\Propeus.Modulo.WorkerService.Exmaple\bin\Debug\net7.0\' e perceba que o modulo comecou a executar no momento em que foi copiado para dentro da pasta do executavel
 * 4 - Abra o arquivo 'Worker.cs' do projeto 'Propeus.Modulo.WorkerService.Exmaple.DLL' e modifique o texto 'Worker running at time: {0}' para 'Meu novo modulo funcionando no tempo: {0}'
 * 5 - Salve o arquivo e execute os seguintes comandos no terminal do item 1, 'dotnet build' e depois 'cp .\bin\Debug\net7.0\Propeus.Modulo.WorkerService.Exmaple.DLL.dll ..\Propeus.Modulo.WorkerService.Exmaple\bin\Debug\net7.0\'
 * 6 - Perceba que o texto antigo parou de ser exibido e o novo comecou em seu lugar.
 * 
 * Resumo:
 * O gerenciador dinamico fica observando qualquer mudanca em seus arquivios, uma vez que ele encontra um modulo valido,
 * ele verifica duas propriedades no atributo 'ModuloAttribute' que sao, 'AutoInicializavel' que permite iniciar um modulo automaticamente e 'PermitirReinicializacao',
 * que indica se deve parar o modulo antigo e inicializar o novo imediatamente. Ambos sao muito uteis em divesas ocasioes, entretanto deve tomar cuidado coma sua utilizacao,
 * pois podem haver casos onde um modulo depende de outro modulo 'AutoInicializavel' de instancia unica, isto pode causar erros de regra de negocio por parte do gerenciador em sua aplicacao.
 * Ha casos tambem onde e desejavel que um modulo antigo permaneca ativo enquanto for utilizado, mas durante novas solicitacoes o novo modulo sera carregado, permitindo
 * que haja uma transicao de modulo sem paralizar o fluxo de processamento.
 * 
 * Observacao:
 * Caso crie um modulo worker 'AutoInicializavel' definindo a propriedade 'PermitirReinicializacao' como false, vai acontecer de o gerenciador nuncar parar 
 * o modulo ate que ele mesmo pare por alguma circustancia que nao seja a atualizacao de modulo.
 * **/


IHost host = Host.CreateDefaultBuilder(args)
    //Pode usar a extencao UseGerenciador passando o modulo dinamico
    //.UseGerenciador(Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
    .ConfigureServices(services =>
    {
        /**
         * Ou pode adicionar diretamente na injecao de dependecia do .NET
         * services.AddSingleton(Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual));
         * **/

        /**
         * Voce pode tambem adicionar o worker do modulo no AddHostedService como um worker normal do .NET (sem uso de gerenciadores)
         * services.AddHostedService<Propeus.Modulo.WorkerService.Exmaple.DLL.Worker>();
         * **/

    })
.Build();

host.Run();
