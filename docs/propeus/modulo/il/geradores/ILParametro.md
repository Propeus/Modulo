# ILParametro `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Geradores
  Propeus.Modulo.IL.Geradores.ILParametro[[ILParametro]]
  end
  subgraph Propeus.Modulo.IL.Interfaces
  Propeus.Modulo.IL.Interfaces.IILExecutor[[IILExecutor]]
  class Propeus.Modulo.IL.Interfaces.IILExecutor interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.IL.Interfaces.IILExecutor --> Propeus.Modulo.IL.Geradores.ILParametro
System.IDisposable --> Propeus.Modulo.IL.Geradores.ILParametro
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `int` | [`Indice`](#indice) | `get, internal set` |
| `string` | [`Nome`](#nome) | `get` |
| `Type` | [`Tipo`](#tipo) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-22)() |
| `void` | [`Executar`](#executar)() |
| `int` | [`ToInt32`](#toint32)() |
| `string` | [`ToString`](#tostring)() |
| `Type` | [`ToType`](#totype)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

#### Public Static methods
| Returns | Name |
| --- | --- |
| `Type` | `explicit` `operator` [`Type`](#operator-type)([`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) obj) |
| `int` | `implicit` `operator` [`int`](#operator-int)([`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) obj) |
| `string` | `implicit` `operator` [`string`](#operator-string)([`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) obj) |

## Details
### Inheritance
 - [
`IILExecutor`
](../interfaces/IILExecutor.md)
 - `IDisposable`

### Constructors
#### ILParametro [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILParametro.cs#L25)
```csharp
public ILParametro(string nomeMetodo, Type tipo, string nome)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeMetodo |   |
| `Type` | tipo |   |
| `string` | nome |   |

#### ILParametro [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L90)
```csharp
public ILParametro(ILBuilderProxy builderProxy, string nomeMetodo, Type tipo, string nome)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | builderProxy |   |
| `string` | nomeMetodo |   |
| `Type` | tipo |   |
| `string` | nome |   |

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L175)
```csharp
public virtual void Executar()
```

#### Dispose [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILParametro.cs#L79)
```csharp
protected virtual void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### Dispose [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L199)
```csharp
public virtual void Dispose()
```

#### Operator Type
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILParametro.cs#L113)
```csharp
public static explicit operator Type(ILParametro obj)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) | obj |   |

#### Operator int
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L227)
```csharp
public static implicit operator int(ILParametro obj)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) | obj |   |

#### Operator string
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILParametro.cs#L123)
```csharp
public static implicit operator string(ILParametro obj)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILParametro`](propeus/modulo/il/geradores/ILParametro.md) | obj |   |

#### ToInt32
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L244)
```csharp
public int ToInt32()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILParametro.cs#L133)
```csharp
public override string ToString()
```

#### ToType
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Console/Helper.cs#L16)
```csharp
public Type ToType()
```

### Properties
#### Tipo
```csharp
public Type Tipo { get; }
```

#### Nome
```csharp
public string Nome { get; }
```

#### Indice
```csharp
public int Indice { get; internal set; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
