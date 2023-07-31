# Q & A
Pagina para perguntas e respostas

### Geral

> Porque nao é utilizado componentes de terceiros para a manipulação de classes em tempo de execução como o Castle Project?

São diversos pontos más o principal é porque eu não queria depender de varios projetos de terceiros, pois iria contra a ideia de ser modular.

> Qual a vantagem de usar esse projeto?

A ideia surgiu dentro de 2 ocasioes jogo e web. Hoje em dia temos diversas ferramentas que possibilitam o deploy rapido e garantia de disponibilidade, porém nem todos possuem recursos para adquirir essas ferramentes, então sugiu a ideia da necessidade de realizar o deploy de uma parte sem precisar parar uma aplicação por completo ou ate mesmo aquela parte afetada. Para os jogos, a ideia seria a mesma, seria legal receber atualizações sem a necessidade de fechar ou reiniciar o jogo.

> Como funciona a atualização de modulo no Gerenciador dinamico?

O gerencenciador verifica se existe algum evento no arquivo e caso exista, ele descarrega o assembly antigo e carrega o novo, porém os objetos que utilizam o assembly antigo continuarao até que finalize seu ciclo de vida. Para evitar que um modulo antigo permaneca ativo por muito tempo após a sua eliminação, o gerenciador sempre irá te retornar uma nova instancia de modulo ao invés de retornar o existente.

> É possivel usar esse projeto em sistemas WEB MVC?

Sim, más há resalvas, para consegui utilziar o gerenciador como controle de DI, foi necessario copiar o controle de DI da microsoft e modifica-la para quando não houver registro de serviço, ele irá buscar no gerenciador. Para manter os padrões de DI, a Microsoft decidiu que ou criaria um do zero ou utilizaria o já existente. 
Obs.: A Microsft não possui responsabilidade sobre o codigo de injeção de dependencia que foi copiado e modificado no projeto `Propeus.Modulo.Hosting`

### Desenvolvimento

> Para que serve cada projeto?

Os projetos estão separados na seguinte ordem:
1 - Propeus.Modulo.Util: É um conjunto de funcoes que ajuda no desenvolvimento.
2 - Propeus.Modulo.IL: É um projeto para criar proxy dinamico, utilizado em conjunto com o `Propeus.Modulo.Dinamico`
3 - Propeus.Modulo.Abstrato: É um projeto que possui todos os modelos e base para desenvolver qualquer modulo ou gerenciador
4 - Propeus.Modulo.Core: É o gerenciador principal que orquestra todos os outros modulos
5 - Propeus.Modulo.Dinamico: É o segundo gerenciador, ele é gerenciado pelo gerenciador core e serve para realizar proxy dinamico dos modulos e contratos
6 - Propeus.Modulo.Workservice: É um modulo que implementa o `IHostedService`, resumindo, serve para criar modulos como um Work Service
7 - Propeus.Modulo.Hosting: É um projeto que modifica o programa WEB MVC para permitir o uso do gerenciador dinamico, permitindo o carregamento de novos controllers e views em tempo de execução

> Um modulo pode manipular um gerenciador?

Sim, porém só consegue manipular os metodos expostos pelo `IModuleManager`, um exemplo é o proprio gerenciador dinamico.

> Para usar os gerenciadores em um projeto WEB MVC, eu preciso carregar o `Propeus.Modulo.Hosting` junto?

Não, se fosse assim não faria sentido o projeto como um todo. O `Propeus.Modulo.Hosting` serve para "embutir" o gerenciador dentro do sistema de DI da microsoft, voce pode carregar o gerenciador como singleton e fazer as chamadas dela para seus services e controllers como um servico qualquer, criar services como modulo tambem não te impede de usar como uma classe comum ou service no MVC.

> Da para carregar views e controllers usando a ferramenta?

Sim, para este caso será necessario o uso do `Propeus.Modulo.Hosting` e `Propeus.Modulo.Dinamico` para funcionar de forma correta, além de precisar realizar a seguinte modificação no seu csproj do modulo.

# [XML](#tab/xml)

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
	<PropertyGroup>
		<GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
	</PropertyGroup>
</Project>
```
---

No sdk deverá utilziar o `Microsoft.NET.Sdk.Razor` e no PropertyGroup do seu projeto devera definir a tag `GenerateEmbeddedFilesManifest` como `true`.
Esta modificação é para os modulos que estão fora do projeto principal e precisam carregar views. 

> Se eu remover um modulo controller do MVC em execução, o controller deixará de existir?

Não, para manter a integridade, ele só deixará de existir quando reiniciar o programa, voce pode criar uma versão de circuit break para evitar uso indevido.

> Existe algum lugar que possa ver as informacoes do modulo?

Sim, voce pode usar a função `.ToString()` para mostrar as informacoes do modulo, caso implemente com base no `BaseModule`