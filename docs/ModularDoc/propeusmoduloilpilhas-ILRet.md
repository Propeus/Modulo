# ILRet `class`

## Description
Cria uma instrução IL de retorno

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILRet[[ILRet]]
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.ILRet
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

## Details
### Summary
Cria uma instrução IL de retorno

### Inheritance
 - [
`ILPilha`
](./propeusmoduloilpilhas-ILPilha.md)

### Constructors
#### ILRet
```csharp
public ILRet(ILBuilderProxy proxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | proxy |   |

### Methods
#### Executar
```csharp
public override void Executar()
```

#### ToString
```csharp
public override string ToString()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
