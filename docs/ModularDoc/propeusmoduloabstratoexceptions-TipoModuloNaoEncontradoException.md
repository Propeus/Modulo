# TipoModuloNaoEncontradoException `class`

## Description
Excecao para quando o tipo do modulo informado nao for encontrado no Assembly

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.TipoModuloNaoEncontradoException[[TipoModuloNaoEncontradoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.TipoModuloNaoEncontradoException
```

## Details
### Summary
Excecao para quando o tipo do modulo informado nao for encontrado no Assembly

### Inheritance
 - `Exception`

### Constructors
#### TipoModuloNaoEncontradoException
```csharp
public TipoModuloNaoEncontradoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
