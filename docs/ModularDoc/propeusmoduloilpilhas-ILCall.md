# ILCall `class`

## Description
Chama uma função

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILCall[[ILCall]]
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.ILCall
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `MemberInfo` | [`Valor`](#valor) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose)(`bool` disposing) |

## Details
### Summary
Chama uma função

### Inheritance
 - [
`ILPilha`
](./propeusmoduloilpilhas-ILPilha.md)

### Constructors
#### ILCall [1/4]
```csharp
public ILCall(ILBuilderProxy proxy, Type metodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | proxy |   |
| `Type` | metodo |   |

#### ILCall [2/4]
```csharp
public ILCall(ILBuilderProxy proxy, MethodInfo metodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | proxy |   |
| `MethodInfo` | metodo |   |

#### ILCall [3/4]
```csharp
public ILCall(ILBuilderProxy proxy, ConstructorInfo metodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | proxy |   |
| `ConstructorInfo` | metodo |   |

#### ILCall [4/4]
```csharp
public ILCall(ILBuilderProxy proxy, FieldInfo metodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | proxy |   |
| `FieldInfo` | metodo |   |

### Methods
#### Executar
```csharp
public override void Executar()
```

#### Dispose
```csharp
protected override void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### ToString
```csharp
public override string ToString()
```

### Properties
#### Valor
```csharp
public MemberInfo Valor { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
