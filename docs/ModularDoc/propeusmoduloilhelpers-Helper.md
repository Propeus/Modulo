# Helper `class`

## Description
Classe de ajuda para montagem de classes

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Helpers
  Propeus.Modulo.IL.Helpers.Helper[[Helper]]
  end
```

## Members
### Methods
#### Public Static methods
| Returns | Name |
| --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`AtribuirMetodoEmDelegate`](#atribuirmetodoemdelegate)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) iLDelegate, [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) metodoDelegate) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CarregarFalse`](#carregarfalse)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CarregarParametro`](#carregarparametro)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CarregarTrue`](#carregartrue)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`ChamarFuncao`](#chamarfuncao)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, `MethodInfo` methodInfo) |
| [`ILCampo`](./propeusmoduloilgeradores-ILCampo.md) | [`CriarCampoArray`](#criarcampoarray)([`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) iLClasse, [`Token`](./propeusmoduloilenums-Token.md)`[]` acessadores, `Type` tipo, `string` nome, `int` comprimento)<br>Cria e inicializa um campo |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CriarConstrutor`](#criarconstrutor)([`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) iLClasseProvider, [`Token`](./propeusmoduloilenums-Token.md)`[]` acessadores, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` parametros) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CriarMetodo`](#criarmetodo-12)(`...`) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`CriarRetorno`](#criarretorno)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Diferente`](#diferente)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Dividir`](#dividir-12)(`...`) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`E`](#e)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; operador, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Igual`](#igual)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| `IEnumerable`&lt;[`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; | [`ImplementarMetodoInterface`](#implementarmetodointerface)([`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) iLClasse) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`InvocarDelegate`](#invocardelegate)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) iLDelegate, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` iLParametros) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`MaiorOuIgualQue`](#maiorouigualque)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`MaiorQue`](#maiorque)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`MenorOuIgualQue`](#menorouigualque)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`MenorQue`](#menorque)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Multiplicar`](#multiplicar-12)(`...`) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Ou`](#ou)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; operador, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Se`](#se)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro1, `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; operador, [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) iLParametro2) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`SeFim`](#sefim)([`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) iLMetodo) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Soma`](#soma-12)(`...`) |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | [`Subitrair`](#subitrair-12)(`...`) |

## Details
### Summary
Classe de ajuda para montagem de classes

### Methods
#### CriarCampoArray
```csharp
public static ILCampo CriarCampoArray(ILClasseProvider iLClasse, Token[] acessadores, Type tipo, string nome, int comprimento)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasse |  |
| [`Token`](./propeusmoduloilenums-Token.md)`[]` | acessadores |  |
| `Type` | tipo |  |
| `string` | nome |  |
| `int` | comprimento |  |

##### Summary
Cria e inicializa um campo

##### Example
public int a = new int[99];

##### Returns


#### ImplementarMetodoInterface
```csharp
public static IEnumerable<ILMetodo> ImplementarMetodoInterface<TInterface>(ILClasseProvider iLClasse)
where TInterface : 
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasse |   |

#### CriarMetodo [1/2]
```csharp
public static ILMetodo CriarMetodo(ILClasseProvider iLClasseProvider, Token[] acessadores, Type retorno, string nome, ILParametro[] parametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasseProvider |   |
| [`Token`](./propeusmoduloilenums-Token.md)`[]` | acessadores |   |
| `Type` | retorno |   |
| `string` | nome |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | parametros |   |

#### CriarMetodo [2/2]
```csharp
public static ILMetodo CriarMetodo(ILDelegate iLClasseProvider, Token[] acessadores, Type retorno, string nome, ILParametro[] parametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) | iLClasseProvider |   |
| [`Token`](./propeusmoduloilenums-Token.md)`[]` | acessadores |   |
| `Type` | retorno |   |
| `string` | nome |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | parametros |   |

#### CriarConstrutor
```csharp
public static ILMetodo CriarConstrutor(ILDelegate iLClasseProvider, Token[] acessadores, ILParametro[] parametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) | iLClasseProvider |   |
| [`Token`](./propeusmoduloilenums-Token.md)`[]` | acessadores |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | parametros |   |

#### Soma [1/2]
```csharp
public static ILMetodo Soma(ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel1 |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel2 |   |

#### Soma [2/2]
```csharp
public static ILMetodo Soma(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Subitrair [1/2]
```csharp
public static ILMetodo Subitrair(ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel1 |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel2 |   |

#### Subitrair [2/2]
```csharp
public static ILMetodo Subitrair(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Dividir [1/2]
```csharp
public static ILMetodo Dividir(ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel1 |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel2 |   |

#### Dividir [2/2]
```csharp
public static ILMetodo Dividir(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Multiplicar [1/2]
```csharp
public static ILMetodo Multiplicar(ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel1 |   |
| [`ILVariavel`](./propeusmoduloilgeradores-ILVariavel.md) | iLVariavel2 |   |

#### Multiplicar [2/2]
```csharp
public static ILMetodo Multiplicar(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Igual
```csharp
public static ILMetodo Igual(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Diferente
```csharp
public static ILMetodo Diferente(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### MaiorQue
```csharp
public static ILMetodo MaiorQue(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### MenorQue
```csharp
public static ILMetodo MenorQue(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### MaiorOuIgualQue
```csharp
public static ILMetodo MaiorOuIgualQue(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### MenorOuIgualQue
```csharp
public static ILMetodo MenorOuIgualQue(ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### SeFim
```csharp
public static ILMetodo SeFim(ILMetodo iLMetodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |

#### Se
```csharp
public static ILMetodo Se(ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; | operador |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### E
```csharp
public static ILMetodo E(ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; | operador |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### Ou
```csharp
public static ILMetodo Ou(ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro1 |   |
| `Func`&lt;[`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md), [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md)&gt; | operador |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro2 |   |

#### CarregarParametro
```csharp
public static ILMetodo CarregarParametro(ILMetodo iLMetodo, ILParametro iLParametro)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md) | iLParametro |   |

#### CarregarTrue
```csharp
public static ILMetodo CarregarTrue(ILMetodo iLMetodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |

#### CarregarFalse
```csharp
public static ILMetodo CarregarFalse(ILMetodo iLMetodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |

#### CriarRetorno
```csharp
public static ILMetodo CriarRetorno(ILMetodo iLMetodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |

#### ChamarFuncao
```csharp
public static ILMetodo ChamarFuncao(ILMetodo iLMetodo, MethodInfo methodInfo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| `MethodInfo` | methodInfo |   |

#### AtribuirMetodoEmDelegate
```csharp
public static ILMetodo AtribuirMetodoEmDelegate(ILMetodo iLMetodo, ILDelegate iLDelegate, ILMetodo metodoDelegate)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) | iLDelegate |   |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | metodoDelegate |   |

#### InvocarDelegate
```csharp
public static ILMetodo InvocarDelegate(ILMetodo iLMetodo, ILDelegate iLDelegate, ILParametro[] iLParametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILMetodo`](./propeusmoduloilgeradores-ILMetodo.md) | iLMetodo |   |
| [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) | iLDelegate |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | iLParametros |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
