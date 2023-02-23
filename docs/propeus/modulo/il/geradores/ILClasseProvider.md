# ILClasseProvider `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Geradores
  Propeus.Modulo.IL.Geradores.ILClasseProvider[[ILClasseProvider]]
  end
  subgraph Propeus.Modulo.IL.Interfaces
  Propeus.Modulo.IL.Interfaces.IILExecutor[[IILExecutor]]
  class Propeus.Modulo.IL.Interfaces.IILExecutor interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.IL.Interfaces.IILExecutor --> Propeus.Modulo.IL.Geradores.ILClasseProvider
System.IDisposable --> Propeus.Modulo.IL.Geradores.ILClasseProvider
```

## Members
### Properties
#### Internal  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`ILClasse`](./ILClasse.md) | [`Atual`](#atual) | `get` |

#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`Token`](../enums/Token.md)`[]` | [`Acessadores`](#acessadores) | `get` |
| `Type` | [`Base`](#base) | `get` |
| `bool` | [`Executado`](#executado) | `get` |
| `Type``[]` | [`Interfaces`](#interfaces) | `get` |
| `string` | [`Namespace`](#namespace) | `get` |
| `string` | [`Nome`](#nome) | `get` |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | [`Proxy`](#proxy) | `get` |
| `int` | [`Versao`](#versao) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-22)() |
| `void` | [`Executar`](#executar)() |
| [`ILClasseProvider`](propeus/modulo/il/geradores/ILClasseProvider.md) | [`NovaVersao`](#novaversao)(`string` namespace, `Type` base, `Type``[]` interfaces, [`Token`](../enums/Token.md)`[]` acessadores) |
| `string` | [`ToString`](#tostring)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

## Details
### Inheritance
 - [
`IILExecutor`
](../interfaces/IILExecutor.md)
 - `IDisposable`

### Constructors
#### ILClasseProvider
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L53)
```csharp
public ILClasseProvider(ILBuilderProxy proxy, string nome, string namespace, Type base, Type[] interfaces, Token[] acessadores)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | proxy |   |
| `string` | nome |   |
| `string` | namespace |   |
| `Type` | base |   |
| `Type``[]` | interfaces |   |
| [`Token`](../enums/Token.md)`[]` | acessadores |   |

### Methods
#### NovaVersao
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILClasseProvider.cs#L70)
```csharp
public ILClasseProvider NovaVersao(string namespace, Type base, Type[] interfaces, Token[] acessadores)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | namespace |   |
| `Type` | base |   |
| `Type``[]` | interfaces |   |
| [`Token`](../enums/Token.md)`[]` | acessadores |   |

#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L191)
```csharp
public virtual void Executar()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L220)
```csharp
public override string ToString()
```

#### Dispose [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILClasseProvider.cs#L161)
```csharp
protected virtual void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### Dispose [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L236)
```csharp
public virtual void Dispose()
```

### Properties
#### Atual
```csharp
internal ILClasse Atual { get; }
```

#### Versao
```csharp
public int Versao { get; }
```

#### Nome
```csharp
public string Nome { get; }
```

#### Namespace
```csharp
public string Namespace { get; }
```

#### Base
```csharp
public Type Base { get; }
```

#### Interfaces
```csharp
public Type Interfaces { get; }
```

#### Acessadores
```csharp
public Token Acessadores { get; }
```

#### Proxy
```csharp
public ILBuilderProxy Proxy { get; }
```

#### Executado
```csharp
public bool Executado { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
