# Console `class`

## Description
Classe de extencao para o System.Configuration

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Util.Console
  Propeus.Modulo.Abstrato.Util.Console.Console[[Console]]
  end
```

## Members
### Methods
#### Public Static methods
| Returns | Name |
| --- | --- |
| `string` | [`ReadLine`](#readline)(`CancellationToken` cancellationToken)<br>Le uma linha no console de forma assincrona |

## Details
### Summary
Classe de extencao para o System.Configuration

### Methods
#### ReadLine
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Console/Helper.cs#L16707566)
```csharp
public static string ReadLine(CancellationToken cancellationToken)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `CancellationToken` | cancellationToken | Use o CancellationTokenSource para finalizar esta tarefa |

##### Summary
Le uma linha no console de forma assincrona

##### Returns
Texto inserido no console

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
