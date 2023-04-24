## Modulo
O modulo é uma classe que possui estado, identidade e proposito.

Todo modulo pode ou não depender de outros modulos fortemente ou fracamente acoplado, recomenda-se adicionar o minimo de dependencia possivel entre os modulos e se houver, deve ser fracamente 
acoplado e de forma opcional.

### Estado
O modulo possui os seguintes estados
- Criado
- Pronto
- Inicializado
- Desligado
- Erro

Dentre eles os estados `Criado`, `Pronto`, `Inicializado` e `Desligado` são definidos automaticamente pelos gerenciadores.

O ciclo de vida de um modulo é defindo da seguinte maneira

`Criado`->`Pronto`->`Inicializado`->(`Desligado` ou `Erro`)

Quando uma instancia do modulo é criado, o estado é definido como `Criado`.

Se o gerenciador realiza alguma configuração, após a criação de sua instancia, o estado passa a ser `Pronto`.

Caso o modulo seja algum `Worker Service` o seu estado muda para `Inicializado` quando for executado.

Ao chamar o `Dispose()` do modulo, o seu estado muda para `Desligado`, ou caso haja algum erro durante a execução do `Worker Service` o seu estado passa a ser `Erro`

### Identificação
O modulo possui algumas propriedades para identificação que são:
- Nome
- Id
- IdManifesto
- Versao

#### Nome
O nome do modulo é o nome do tipo da classe

#### Id
O Id do modulo é um `Guid` gerado no momento em que a classe é instanciada

#### IdManifesto
O Id é o `Guid` do assembly compliado

#### Versao
É a versao do Assembly que o modulo esta contido

## Como usar
Todo modulo deve possuir o atributo `ModuloAttribute` e a implementacao da interface `IModulo` ou
herdar da classe `ModuloBase` que implementa a interface.

Segue o exemplo a baixo:
```cs
[Modulo]
public class MeuModulo : ModuloBase
{
	//Implemente seu codigo aqui
}
```

O atributo `ModuloAtttribute` possui duas propriedades que podem ser utilizados dependendo do 
gerenciador. As propriedades sao `AutoInicializavel` que indica se um modulo deve ser criado
no momento da sua descoberta e `AutoAtualizavel` que indica se um modulo deve ser reciclado automaticamente
no momento em que um novo assembly e descoberto no gerenciador.

Recomenda-se criar uma interface de contrato para cada modulo criado, isso permite que outros 
modulos nao fiquem fortemente dependente uns dos outros. A interface de contrato deve
possuir o atributo `ModuloContratoAttribute` e deve herdar da interface `IModulo`.

Segue o exemplo a baixo:
```cs
[ModuloContrato(typeof(MeuModulo)]
public interface MeuModuloContrato : IModulo
{

}
```

O atrubuto `ModuloContrato` deve receber obrigatoriamente o tipo do modulo que o contrato representa 
(`[ModuloContrato(typeof(MeuModulo)]`) ou o nome do modulo (`[ModuloContrato("MeuModulo"]`).

## Exemplos
Segue alguns exemplos para cada tipo de situacao

1. Quando a regra de negocio nao permita o uso de outros tipos de modulo durante a execucao
```cs

/**
 * Obs.:
 * - Assuma que todos os modulos a baixo estao no mesmo assembly (dll) 
 * - Este exemplo e uma demonstracao de isolamento/divisao de responsabilidade, cada modulo
 * cuida somente daquilo que e de seu proposito.
 **/

[Modulo]
public class AdicaoModulo : ModuloBase
{
	public int Adicao(int a, int b) => a+b;
}

[Modulo]
public class SubtracaoModulo : ModuloBase
{
	public int Subtracao(int a, int b) => a-b;
}

[Modulo]
public class MultiplicacaoModulo : ModuloBase
{
	public int Multiplicacao(int a, int b) => a*b;
}

[Modulo]
public class DivisaoModulo : ModuloBase
{
	public int Divisao(int a, int b) => a/b;
}

[Modulo]
public class CalculadoraModulo : ModuloBase
{

	AdicaoModulo adicao;
	SubtracaoModulo subtracao;
	MultiplicacaoModulo multiplicacao;
	DivisaoModulo divisao;

	pubic CalculadoraModulo(
	 AdicaoModulo adicao
	,SubtracaoModulo subtracao
	,MultiplicacaoModulo multiplicacao
	,DivisaoModulo divisao)
	{
		this.adicao = adicao;
		this.subtracao = subtracao;
		this.multiplicacao = multiplicacao;
		this.divisao = divisao;
	}

	public int Adicao(int a, int b) => adicao.Adicao(a,b);
	public int Subtracao(int a, int b) => subtracao.Subtracao(a,b);
	public int Multiplicacao(int a, int b) => multiplicacao.Multiplicacao(a,b);
	public int Divisao(int a, int b) => divisao.Divisao(a,b);
}
```

2. Quando e permitido modulos diferentes para um mesmo contrato (segue o principio de IoC)

```cs

/**
 * Obs.:
 * - Assuma que todos os modulos a baixo estao no mesmo assembly (dll) 
 * - Este exemplo e uma demonstracao de isolamento/divisao de responsabilidade, cada modulo
 * e seu contrato cuida somente daquilo que e de seu proposito.
 **/

[ModuloContrato(typeof(AdicaoModulo))]
public interface IAdicaoModuloContrato : IModulo
{
	public int Adicao(int a, int b);
}

[ModuloContrato(typeof(SubtracaoModulo))]
public interface ISubtracaoModuloContrato : IModulo
{
	public int Subtracao(int a, int b);
}

[ModuloContrato(typeof(MultiplicacaoModulo))]
public interface IMultiplicacaoModuloContrato : IModulo
{
	public int Multiplicacao(int a, int b);
}

[ModuloContrato(typeof(DivisaoModulo))]
public interface IDivisaoModuloContrato : IModulo
{
	public int Divisao(int a, int b);
}

[Modulo]
public class AdicaoModulo : ModuloBase, IAdicaoModuloContrato
{
	public int Adicao(int a, int b) => a+b;
}

[Modulo]
public class SubtracaoModulo : ModuloBase, ISubtracaoModuloContrato
{
	public int Subtracao(int a, int b) => a-b;
}

[Modulo]
public class MultiplicacaoModulo : ModuloBase, IMultiplicacaoModuloContrato
{
	public int Multiplicacao(int a, int b) => a*b;
}

[Modulo]
public class DivisaoModulo : ModuloBase, IDivisaoModuloContrato
{
	public int Divisao(int a, int b) => a/b;
}

[Modulo]
public class CalculadoraModulo : ModuloBase
{
	IAdicaoModuloContrato adicao;
	ISubtracaoModuloContrato subtracao;
	IMultiplicacaoModuloContrato multiplicacao;
	IDivisaoModuloContrato divisao;

	pubic CalculadoraModulo(
	 IAdicaoModuloContrato adicao
	,ISubtracaoModuloContrato subtracao
	,IMultiplicacaoModuloContrato multiplicacao
	,IDivisaoModuloContrato divisao)
	{
		this.adicao = adicao;
		this.subtracao = subtracao;
		this.multiplicacao = multiplicacao;
		this.divisao = divisao;
	}

	public int Adicao(int a, int b) => adicao.Adicao(a,b);
	public int Subtracao(int a, int b) => subtracao.Subtracao(a,b);
	public int Multiplicacao(int a, int b) => multiplicacao.Multiplicacao(a,b);
	public int Divisao(int a, int b) => divisao.Divisao(a,b);
}
```

3. Quando e permitido o carregamento dinamico dos modulos
```cs

/**
 * Obs.:
 * - Assuma que cada modulo a baixo estao em assembly diferentes (dll) 
 * - Este exemplo e uma demonstracao de isolamento/divisao de responsabilidade, cada modulo
 * e seu contrato cuida somente daquilo que e de seu proposito.
 **/

 [Modulo]
public class AdicaoModulo : ModuloBase
{
	public int Adicao(int a, int b) => a+b;
}

[Modulo]
public class SubtracaoModulo : ModuloBase
{
	public int Subtracao(int a, int b) => a-b;
}

[Modulo]
public class MultiplicacaoModulo : ModuloBase
{
	public int Multiplicacao(int a, int b) => a*b;
}

[Modulo]
public class DivisaoModulo : ModuloBase
{
	public int Divisao(int a, int b) => a/b;
}

/**
 * - Assuma que as interfaces a baixo pertencem ao mesmo assembly da calculadora
 * - Note que como nao ha o tipo, coloca o nome para localizar e instanciar.
 * - Observe que os modulos nao possuem as intefaces de contrato implementados
 **/

[ModuloContrato("AdicaoModulo")]
public interface IAdicaoModuloContrato : IModulo
{
	public int Adicao(int a, int b);
}

[ModuloContrato("SubtracaoModulo")]
public interface ISubtracaoModuloContrato : IModulo
{
	public int Subtracao(int a, int b);
}

[ModuloContrato("MultiplicacaoModulo")]
public interface IMultiplicacaoModuloContrato : IModulo
{
	public int Multiplicacao(int a, int b);
}

[ModuloContrato("DivisaoModulo")]
public interface IDivisaoModuloContrato : IModulo
{
	public int Divisao(int a, int b);
}

[Modulo]
public class CalculadoraModulo : ModuloBase
{
	IAdicaoModuloContrato adicao;
	ISubtracaoModuloContrato subtracao;
	IMultiplicacaoModuloContrato multiplicacao;
	IDivisaoModuloContrato divisao;

	pubic CalculadoraModulo(
	 IAdicaoModuloContrato adicao
	,ISubtracaoModuloContrato subtracao
	,IMultiplicacaoModuloContrato multiplicacao
	,IDivisaoModuloContrato divisao)
	{
		this.adicao = adicao;
		this.subtracao = subtracao;
		this.multiplicacao = multiplicacao;
		this.divisao = divisao;
	}

	public int Adicao(int a, int b) => adicao.Adicao(a,b);
	public int Subtracao(int a, int b) => subtracao.Subtracao(a,b);
	public int Multiplicacao(int a, int b) => multiplicacao.Multiplicacao(a,b);
	public int Divisao(int a, int b) => divisao.Divisao(a,b);
}
```

4. Caso exista um unico modulo mas cada contrato executa a parte necessaria do modulo
```cs

/**
 * Obs.:
 * - Observe que o modulo nao possuem as intefaces de contrato implementados
 **/

[Modulo]
public class CalculadoraModulo : ModuloBase
{
	
	public int Adicao(int a, int b) => a+b;
	public int Subtracao(int a, int b) => a-b;
	public int Multiplicacao(int a, int b) => a*b;
	public int Divisao(int a, int b) => a/b;
}

/**
 * - Assuma que as interfaces a baixo estao em assembly diferentes (dll) 
 * - Note que todos os contratos possuem o mesmo modulo como referencia, entretanto cada um
 * possui um metodo especifico, reduzindo a responsabilidade de cada modulo
 * - Observe que o modulo nao possuem as intefaces de contrato implementados
 **/

[ModuloContrato("CalculadoraModulo")]
public interface IAdicaoModuloContrato : IModulo
{
	public int Adicao(int a, int b);
}

[ModuloContrato("CalculadoraModulo")]
public interface ISubtracaoModuloContrato : IModulo
{
	public int Subtracao(int a, int b);
}

[ModuloContrato("CalculadoraModulo")]
public interface IMultiplicacaoModuloContrato : IModulo
{
	public int Multiplicacao(int a, int b);
}

[ModuloContrato("CalculadoraModulo")]
public interface IDivisaoModuloContrato : IModulo
{
	public int Divisao(int a, int b);
}

```

## Padroes
- Use o sufixo "Modulo" para quando for criar uma classe de modulo
- Use o sufixo "Contrato" para quando for criar uma interface de contrato
## Recomendacoes
- Mantenha nomes unicos para os modulos, o gerenciador nao diferencia modulos por namespace
- Evite de realziar muitas mudancas de assembly nos modulos dinamicos.
- Exponha em documentacao a interface de contrato de um modulo caso queira compartilhar com a comunidade.
## Requerido
- As interfaces de contrato e modulos devem possuir o acessador `publuc`

## Perguntas e respostas

> Posso chamar o `Dispose()` diretamente?

Pode, mas não é recomendavel, pois, podem haver outros modulos que utilizam ele.

> Posso criar uma instancia do modulo diretamente `new`?

Pode, más não será resolvido as dependencias que ele precisa. Se o modulo for `InstanciaUnica` pode ocorrer uma exceção não tratado.

> O que acontece se eu criar uma interface de contrato com uma propriedade ou metodo que nao existe no tipo implementado?

Se o contrato for chamado pelo `Propeus.Modulo.Core.Gerenciador`, haverá erro de conversão, pois o tipo implementado não herda a interface. Caso 
o contrato seja chamado pelo `Propeus.Modulo.Dinamico.Gerenciador`, haverá o lançamento da exceção `NotImplementedException` durante a chamada do
metodo ou da propriedade, pois a interface
será implementada dinamicamente.

> Quando um modulo e suas dependencias são criados, ao criar novamente o modulo, as suas dependencias são reutilizados ou criados novamente?

Por padrão a cada nova instancia do modulo, é gerado novas instancias das dependencias no modulo, exceto quando o modulo injetado
é de instancia unica e não foi `Desligado` ou descartado. Foi pensado desta forma para permitir que as novas versoes de modulo sejam instanciadas
sempre que forem carregadas.