[[_TOC_]]

### Informações
---
Nome do projeto: Modulo <br>
Nome da biblioteca: Core <br>
Autor: Propeus 

Namespace: Propeus.Modulo.Util.Attributes<br>
Classe: Gerenciador<br>
Acessador: Publico<br>
Estatico: Não<br>
Partial: Não<br>

### Descrição
---
Permite manipular e gerenciar modulos criando, injetando e eliminado quando necessário sem interferir o GC.

### Metodos
---
|Nome| Parametros | Parametros de tipo  | Propriedade | Método | Assinatura|
|--|--|--|--|--|--|
| Criar | `params object[]` | `T : IModulo` | N | S | `T Criar<T>(params object[]) where T : IModulo;` |
| Criar | `Type` ,`params object[]` | N/A | N | S | `IModulo Criar(Type, params object[]);` |
| Existe | `IModulo`  | N/A | N | S | `bool Existe(IModulo);` |
| Existe | `Type`  | N/A | N | S | `bool Existe(Type);` |
| Existe | `string`  | N/A | N | S | `bool Existe(string);` |
| Obter | N/A  | `T : IModulo` | N | S | `T Obter<T>() where T : IModulo;` |
| Obter | `Type`  | N/A | N | S | `IModulo Obter(Type);` |
| Obter | `string`  | N/A | N | S | `IModulo Obter(string);` |
| Listar | N/A | N/A | N | S | `IEnumerable<IModulo> Listar();` |
| Reiniciar | `T : IModulo` | N/A | N | S | `T Reiniciar<T>(T) where T : IModulo;` |
| Reiniciar | N/A | `string` | N | S | `IModulo Reiniciar(string id);` |
| Remover| N/A  | `T : IModulo` | N | S | `void Remover<T>(T modulo) where T : IModulo;` |
| Remover| `string`  | N/A | N | S | `void Remover(string id);` |
| Remover| N/A | N/A | N | S | `void RemoverTodos();` |