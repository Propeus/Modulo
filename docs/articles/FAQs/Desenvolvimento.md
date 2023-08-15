# Desenvolvimento

### Para que serve cada projeto?

Os projetos estão separados na seguinte ordem:
1 - Propeus.Modulo.Util: É um conjunto de funcoes que ajuda no desenvolvimento.
2 - Propeus.Modulo.IL: É um projeto para criar proxy dinamico, utilizado em conjunto com o `Propeus.Modulo.Dinamico`
3 - Propeus.Modulo.Abstrato: É um projeto que possui todos os modelos e base para desenvolver qualquer modulo ou gerenciador
4 - Propeus.Modulo.Core: É o gerenciador principal que orquestra todos os outros modulos
5 - Propeus.Modulo.Dinamico: É o segundo gerenciador, ele é gerenciado pelo gerenciador core e serve para realizar proxy dinamico dos modulos e contratos
6 - Propeus.Modulo.Workservice: É um modulo que implementa o `IHostedService`, resumindo, serve para criar modulos como um Work Service
7 - Propeus.Modulo.Hosting: É um projeto que modifica o programa WEB MVC para permitir o uso do gerenciador dinamico, permitindo o carregamento de novos controllers e views em tempo de execução

### Um modulo pode manipular um gerenciador?
Sim, porém só consegue manipular os metodos expostos pelo `IModuleManager`, um exemplo é o proprio gerenciador dinamico.

### Para usar os gerenciadores em um projeto WEB MVC, eu preciso carregar o `Propeus.Modulo.Hosting` junto?
Não, se fosse assim não faria sentido o projeto como um todo. O `Propeus.Modulo.Hosting` serve para "embutir" o gerenciador dentro do sistema de DI da microsoft, voce pode carregar o gerenciador como singleton e fazer as chamadas dela para seus services e controllers como um servico qualquer, criar services como modulo tambem não te impede de usar como uma classe comum ou service no MVC.

### Da para carregar views e controllers usando a ferramenta?
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

### Se eu remover um modulo controller do MVC em execução, o controller deixará de existir?
Não, para manter a integridade, ele só deixará de existir quando reiniciar o programa, voce pode criar uma versão de circuit break para evitar uso indevido.

### Existe algum lugar que possa ver as informacoes do modulo?
Sim, voce pode usar a função `.ToString()` para mostrar as informacoes do modulo, caso implemente com base no `BaseModule`