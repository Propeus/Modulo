# ModuloAttribute `class`

## Description
Identificador de extremidade de um modulo

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Atributos
  Propeus.Modulo.Abstrato.Atributos.ModuloAttribute[[ModuloAttribute]]
  end
  subgraph System
System.Attribute[[Attribute]]
  end
System.Attribute --> Propeus.Modulo.Abstrato.Atributos.ModuloAttribute
```

## Details
### Summary
Identificador de extremidade de um modulo

### Inheritance
 - `Attribute`

### Constructors
#### ModuloAttribute
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILCampo.cs#L21)
```csharp
public ModuloAttribute()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
