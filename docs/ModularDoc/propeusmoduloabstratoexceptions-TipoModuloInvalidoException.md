# TipoModuloInvalidoException `class`

## Description
Excecao para quando o modulo nao seguir os padroes de implementacao

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.TipoModuloInvalidoException[[TipoModuloInvalidoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.TipoModuloInvalidoException
```

## Details
### Summary
Excecao para quando o modulo nao seguir os padroes de implementacao

### Inheritance
 - `Exception`

### Constructors
#### TipoModuloInvalidoException
```csharp
public TipoModuloInvalidoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
