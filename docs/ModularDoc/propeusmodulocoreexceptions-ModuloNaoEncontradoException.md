# ModuloNaoEncontradoException `class`

## Description
Excecao para quando o modulo informado nao foi encontrado

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Core.Exceptions
  Propeus.Modulo.Core.Exceptions.ModuloNaoEncontradoException[[ModuloNaoEncontradoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Core.Exceptions.ModuloNaoEncontradoException
```

## Details
### Summary
Excecao para quando o modulo informado nao foi encontrado

### Inheritance
 - `Exception`

### Constructors
#### ModuloNaoEncontradoException
```csharp
public ModuloNaoEncontradoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
