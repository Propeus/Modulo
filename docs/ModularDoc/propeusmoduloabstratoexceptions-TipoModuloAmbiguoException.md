# TipoModuloAmbiguoException `class`

## Description
Excecao para quando for encontrado mais de um modulo de mesmo nome

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.TipoModuloAmbiguoException[[TipoModuloAmbiguoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.TipoModuloAmbiguoException
```

## Details
### Summary
Excecao para quando for encontrado mais de um modulo de mesmo nome

### Inheritance
 - `Exception`

### Constructors
#### TipoModuloAmbiguoException
```csharp
public TipoModuloAmbiguoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
