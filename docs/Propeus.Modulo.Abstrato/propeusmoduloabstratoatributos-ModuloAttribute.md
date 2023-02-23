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
```csharp
public ModuloAttribute()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
