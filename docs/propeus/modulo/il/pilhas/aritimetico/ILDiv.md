# ILDiv `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Aritimetico
  Propeus.Modulo.IL.Pilhas.Aritimetico.ILDiv[[ILDiv]]
  end
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.Aritimetico.ILDiv
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

## Details
### Inheritance
 - [
`ILPilha`
](../ILPilha.md)

### Constructors
#### ILDiv
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Pilhas/Aritimetico/ILDiv.cs#L9)
```csharp
public ILDiv(ILBuilderProxy iLBuilderProxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Pilhas/Aritimetico/ILDiv.cs#L14)
```csharp
public override void Executar()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Pilhas/Aritimetico/ILDiv.cs#L26)
```csharp
public override string ToString()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
