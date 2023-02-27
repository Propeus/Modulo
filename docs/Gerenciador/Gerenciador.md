
# Gerenciador

A classe Gerenciador é responsável por carregar, inicializar e gerenciar todos os módulos do sistema.

## Estado de um modulo
```mermaid
stateDiagram-v2
    [*] --> Inicializado
    Inicializado --> Desligado
    Desligado --> [*]
```


## Criar
Fluxo para criacao de um novo modulo
```mermaid
flowchart TD
    A[Gerenciador]-->|Solicita a criacao de uma instancia| B(Criar)
    B --> C{E interface?}
    C -->|Sim| D{Possui o atributo ModuloContrato?}
    D -->|Nao| J[Lanca excecao de contrato invalido]
    D -->|Sim| E{Possui um tipo?}
    E -->|Nao| K[Lanca excecao de tipo invalido]
    E -->|Sim| F{O tipo e uma classe?}
    F -->|Nao| L[Lanca excecao de tipo invalido]
    F -->|Sim| G{Possui a implementacao da interface IModulo?}
    G -->|Nao| M[Lanca excecao de modulo invalido]
    G -->|Sim| H{Possui o atributo Modulo?}
    H -->|Nao| N[Lanca excecao de modulo invalido]
    H -->|Sim| I[Cria a instancia do modulo]
    I -->|Devolve a instancia do objeto|B
    B -->|Devolve a instancia do objeto|A 
    C -->|Nao| F
```

## Remover
```mermaid
flowchart TD
    A[Gerenciador]-->|Solicita a remocao de uma instancia| B(Remover)
    B --> C{Existe no gerenciador?}
    C -->|Sim| D[Remove do dicionario]
    D -->E[Realiza o Dispose do modulo]
```

## Obter
```mermaid
flowchart TD
    A[Gerenciador]-->|Obtem a instancia do modulo| B(Obter)
    B --> C{E interface?}
    C -->|Sim| D{Possui o atributo ModuloContrato?}
    D -->|Nao| J[Lanca excecao de contrato invalido]
    D -->|Sim| E{Possui um tipo?}
    E -->|Nao| K[Lanca excecao de tipo invalido]
    E -->|Sim| F{O tipo e uma classe?}
    F -->|Nao| L[Lanca excecao de tipo invalido]
    F -->|Sim| G[Busca os modulos com o mesmo tipo]
    G -->|Sim| H{Existe algum modulo do tipo?}
    H -->|Nao| I[Lanca excecao de modulo nao encontrado]
    H -->|Sim| M{Foi eliminado pelo G.C.?}
    M -->|Sim| N[Lanca excecao de modulo descartado]
    M -->|Nao - Retorna a instancia do objeto| B
    B -->|Devolve a instancia do objeto|A 
```

**Este documento nao esta finalizado, caso queria mais detalhes, acesse a pasta "propeus" para obter a documentacao auto-gerado.
    