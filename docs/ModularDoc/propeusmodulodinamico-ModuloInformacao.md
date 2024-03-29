# ModuloInformacao `class`

## Description
Modelo para detalhar informações sobre o modulo

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Dinamico
  Propeus.Modulo.Dinamico.ModuloInformacao[[ModuloInformacao]]
  end
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IModuloInformacao[[IModuloInformacao]]
  class Propeus.Modulo.Abstrato.Interfaces.IModuloInformacao interfaceStyle;
  Propeus.Modulo.Abstrato.Interfaces.IBaseModelo[[IBaseModelo]]
  class Propeus.Modulo.Abstrato.Interfaces.IBaseModelo interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
  subgraph Propeus.Modulo.Abstrato
  Propeus.Modulo.Abstrato.BaseModelo[[BaseModelo]]
  end
Propeus.Modulo.Abstrato.Interfaces.IModuloInformacao --> Propeus.Modulo.Dinamico.ModuloInformacao
Propeus.Modulo.Abstrato.Interfaces.IBaseModelo --> Propeus.Modulo.Abstrato.Interfaces.IModuloInformacao
System.IDisposable --> Propeus.Modulo.Abstrato.Interfaces.IBaseModelo
Propeus.Modulo.Abstrato.BaseModelo --> Propeus.Modulo.Dinamico.ModuloInformacao
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `Assembly` | [`Assembly`](#assembly)<br>Assembly a qual o modulo pertence | `get` |
| `AssemblyName` | [`AssemblyName`](#assemblyname)<br>Informações sobre o assembly do modulo | `get` |
| `string` | [`Caminho`](#caminho)<br>Caminho do modulo em disco | `get` |
| [`IModuloTipo`](./propeusmoduloabstratointerfaces-IModuloTipo.md) | [`Item`](#item) | `get, set` |
| `Dictionary`&lt;`string`, [`IModuloTipo`](./propeusmoduloabstratointerfaces-IModuloTipo.md)&gt; | [`Modulos`](#modulos)<br>ModuloInformacao mapeados do assembly | `get` |
| `int` | [`ModulosCarregados`](#moduloscarregados)<br>Informa a quantidade de modulos criados. | `get` |
| `int` | [`ModulosDescobertos`](#modulosdescobertos)<br>Informa a quantidade de modulos disponiveis dentro de uma DLL | `get` |
| `string` | [`Versao`](#versao)<br>Versao do modelo | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`AdicionarContrato`](#adicionarcontrato)(`string` nomeModulo, `Type` contrato)<br>Adiciona um contrato atrelado a um modulo |
| `Type` | [`CarregarTipoModulo`](#carregartipomodulo)(`string` nomeModulo)<br>Obtem o Type do modulo informado |
| `List`&lt;`Type`&gt; | [`ObterContratos`](#obtercontratos)(`string` nomeModulo)<br>Obtem a lista de contratos do modulo informado |
| `bool` | [`PossuiModulo`](#possuimodulo)(`string` nomeModulo)<br>Indica se o modulo informado esta prensente |
| `string` | [`ToString`](#tostring)()<br>Exibe informações basicas sobre o modelo |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose)(`bool` disposing)<br>Libera os objetos deste modelo e altera o estado dele para [Estado](./propeusmoduloabstrato-Estado.md).[Desligado](#desligado) |

#### Public Static methods
| Returns | Name |
| --- | --- |
| `bool` | [`PossuiModuloValido`](#possuimodulovalido)([`IModuloBinario`](./propeusmoduloabstratointerfaces-IModuloBinario.md) path, [`IRegra`](./propeusmoduloabstratointerfaces-IRegra.md)`[]` regra)<br>Metodo estatico para validação de modulo |

## Details
### Summary
Modelo para detalhar informações sobre o modulo

### Inheritance
 - [
`IModuloInformacao`
](./propeusmoduloabstratointerfaces-IModuloInformacao.md)
 - [
`IBaseModelo`
](./propeusmoduloabstratointerfaces-IBaseModelo.md)
 - `IDisposable`
 - [
`BaseModelo`
](./propeusmoduloabstrato-BaseModelo.md)

### Constructors
#### ModuloInformacao [1/2]
```csharp
public ModuloInformacao(Type moduloTipo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `Type` | moduloTipo | Tipo do modulo |

##### Summary
Inicializa o objeto obtendo as informacoes sobre o tipo informado

##### Exceptions
| Name | Description |
| --- | --- |
| ArgumentNullException | Argumento nulo |

#### ModuloInformacao [2/2]
```csharp
public ModuloInformacao(IModuloBinario moduloBinario)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IModuloBinario`](./propeusmoduloabstratointerfaces-IModuloBinario.md) | moduloBinario |  |

##### Summary
Inicializa o objeto obtendo as informacoes sobre o binario carregado

##### Exceptions
| Name | Description |
| --- | --- |
| ArgumentNullException |  |

### Methods
#### AdicionarContrato
```csharp
public virtual void AdicionarContrato(string nomeModulo, Type contrato)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo | Nome do modulo a ser vinculado o contrato |
| `Type` | contrato | Tipo da interface de contrato |

##### Summary
Adiciona um contrato atrelado a um modulo

#### ObterContratos
```csharp
public virtual List<Type> ObterContratos(string nomeModulo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo |  |

##### Summary
Obtem a lista de contratos do modulo informado

##### Returns


#### PossuiModulo
```csharp
public virtual bool PossuiModulo(string nomeModulo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo | Nome do modulo |

##### Summary
Indica se o modulo informado esta prensente

##### Returns
Retorna caso ache o modulo, caso contrario retorna

#### CarregarTipoModulo
```csharp
public virtual Type CarregarTipoModulo(string nomeModulo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo | Nome do modulo a ser obtido o Type |

##### Summary
Obtem o Type do modulo informado

##### Returns
Type

#### ToString
```csharp
public override string ToString()
```
##### Summary
Exibe informações basicas sobre o modelo

##### Returns


#### Dispose
```csharp
protected override void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing | Indica se deve alterar o estado do objeto para [Estado](./propeusmoduloabstrato-Estado.md).[Desligado](#desligado) |

##### Summary
Libera os objetos deste modelo e altera o estado dele para [Estado](./propeusmoduloabstrato-Estado.md).[Desligado](#desligado)

#### PossuiModuloValido
```csharp
public static bool PossuiModuloValido(IModuloBinario path, IRegra[] regra)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IModuloBinario`](./propeusmoduloabstratointerfaces-IModuloBinario.md) | path |  |
| [`IRegra`](./propeusmoduloabstratointerfaces-IRegra.md)`[]` | regra |  |

##### Summary
Metodo estatico para validação de modulo

##### Returns


### Properties
#### Versao
```csharp
public override string Versao { get; }
```
##### Summary
Versao do modelo

#### Item
```csharp
public virtual IModuloTipo Item { get; set; }
```

#### Assembly
```csharp
public virtual Assembly Assembly { get; }
```
##### Summary
Assembly a qual o modulo pertence

#### AssemblyName
```csharp
public virtual AssemblyName AssemblyName { get; }
```
##### Summary
Informações sobre o assembly do modulo

#### Modulos
```csharp
public virtual Dictionary<string, IModuloTipo> Modulos { get; }
```
##### Summary
ModuloInformacao mapeados do assembly

#### Caminho
```csharp
public virtual string Caminho { get; }
```
##### Summary
Caminho do modulo em disco

#### ModulosDescobertos
```csharp
public virtual int ModulosDescobertos { get; }
```
##### Summary
Informa a quantidade de modulos disponiveis dentro de uma DLL

#### ModulosCarregados
```csharp
public virtual int ModulosCarregados { get; }
```
##### Summary
Informa a quantidade de modulos criados.

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
