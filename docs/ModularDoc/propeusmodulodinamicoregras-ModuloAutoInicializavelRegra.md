# ModuloAutoInicializavelRegra `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Dinamico.Regras
  Propeus.Modulo.Dinamico.Regras.ModuloAutoInicializavelRegra[[ModuloAutoInicializavelRegra]]
  end
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IRegra[[IRegra]]
  class Propeus.Modulo.Abstrato.Interfaces.IRegra interfaceStyle;
  end
Propeus.Modulo.Abstrato.Interfaces.IRegra --> Propeus.Modulo.Dinamico.Regras.ModuloAutoInicializavelRegra
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IEnumerable`&lt;`TypeInfo`&gt; | [`modulosValidos`](#modulosvalidos) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `bool` | [`Executar`](#executar)(`object``[]` args) |

## Details
### Inheritance
 - [
`IRegra`
](./propeusmoduloabstratointerfaces-IRegra.md)

### Constructors
#### ModuloAutoInicializavelRegra
```csharp
public ModuloAutoInicializavelRegra()
```

### Methods
#### Executar
```csharp
public virtual bool Executar(object[] args)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object``[]` | args |   |

### Properties
#### modulosValidos
```csharp
public IEnumerable<TypeInfo> modulosValidos { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
