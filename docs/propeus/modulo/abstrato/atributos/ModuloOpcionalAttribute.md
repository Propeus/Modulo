# ModuloOpcionalAttribute `class`

## Description
Informa se o modulo atual é opcional a sua instancia

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Atributos
  Propeus.Modulo.Abstrato.Atributos.ModuloOpcionalAttribute[[ModuloOpcionalAttribute]]
  end
  subgraph System
System.Attribute[[Attribute]]
  end
System.Attribute --> Propeus.Modulo.Abstrato.Atributos.ModuloOpcionalAttribute
```

## Details
### Summary
Informa se o modulo atual é opcional a sua instancia

### Inheritance
 - `Attribute`

### Constructors
#### ModuloOpcionalAttribute
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Atributos/ModuloOpcionalAttribute.cs#L11)
```csharp
public ModuloOpcionalAttribute()
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
