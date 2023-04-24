# ModuloContratoNaoEncontratoException `class`

## Description
Excecao para quando a interface de contrato nao possui o atributo [ModuloContratoAttribute](./propeusmoduloabstratoatributos-ModuloContratoAttribute.md)

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.ModuloContratoNaoEncontratoException[[ModuloContratoNaoEncontratoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.ModuloContratoNaoEncontratoException
```

## Details
### Summary
Excecao para quando a interface de contrato nao possui o atributo [ModuloContratoAttribute](./propeusmoduloabstratoatributos-ModuloContratoAttribute.md)

### Inheritance
 - `Exception`

### Constructors
#### ModuloContratoNaoEncontratoException
```csharp
public ModuloContratoNaoEncontratoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
