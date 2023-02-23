# ModuloAutoInicializavelAttribute `class`

## Description
Indica se o modulo marcado deve ser inicializado após o mapeamento

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Atributos
  Propeus.Modulo.Abstrato.Atributos.ModuloAutoInicializavelAttribute[[ModuloAutoInicializavelAttribute]]
  end
  subgraph System
System.Attribute[[Attribute]]
  end
System.Attribute --> Propeus.Modulo.Abstrato.Atributos.ModuloAutoInicializavelAttribute
```

## Details
### Summary
Indica se o modulo marcado deve ser inicializado após o mapeamento

### Inheritance
 - `Attribute`

### Constructors
#### ModuloAutoInicializavelAttribute
[*Source code*](https://github.com///blob//src/Propeus.Modulo.IL/Geradores/ILGerador.cs#L15)
```csharp
public ModuloAutoInicializavelAttribute()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
