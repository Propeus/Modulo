# ModuloComAtributoRegra `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Dinamico.Regras
  Propeus.Modulo.Dinamico.Regras.ModuloComAtributoRegra[[ModuloComAtributoRegra]]
  end
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IRegra[[IRegra]]
  class Propeus.Modulo.Abstrato.Interfaces.IRegra interfaceStyle;
  end
Propeus.Modulo.Abstrato.Interfaces.IRegra --> Propeus.Modulo.Dinamico.Regras.ModuloComAtributoRegra
```

## Members
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
#### ModuloComAtributoRegra
```csharp
public ModuloComAtributoRegra()
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

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
