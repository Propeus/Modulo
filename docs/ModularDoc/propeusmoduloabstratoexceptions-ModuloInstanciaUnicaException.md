# ModuloInstanciaUnicaException `class`

## Description
Excecao para tentativa de criacao de um novo modulo de instancia unica

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.ModuloInstanciaUnicaException[[ModuloInstanciaUnicaException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.ModuloInstanciaUnicaException
```

## Details
### Summary
Excecao para tentativa de criacao de um novo modulo de instancia unica

### Inheritance
 - `Exception`

### Constructors
#### ModuloInstanciaUnicaException
```csharp
public ModuloInstanciaUnicaException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
