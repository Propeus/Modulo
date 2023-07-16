

## Gerenciadores
Neste projeto todos os gerenciadores são herdados da interface `Propeus.Modulo.Abstrato.Interfaces.IGerenciadores` e implementam os principais metodos:
- Criar
- Remover
- Obter
- Reciclar
- Existe
- Listar

### Propeus.Modulo.Core.Gerenciador
Esta classe é responsavel por gerenciar todos os modulos do sistema sendo o gerenciador nativo. 

Para que fosse possivel o funcionamento, foi necessario a criação de dois provedores
que sao, `TypeProvider`, responsavel por gerenciar todos os tipos da aplicação que esta em execucao incluindo os modulos e `InstanciaProvider`, responsavel por gerenciar todas as 
instancias de mudulo que são criados ou obtidos.

Esta classe possui uma propriedade estatica que só é inicializado quando chamado. A chamada do metodo `Dispose()` faz com que uma nova instancia do gerenciador seja
criado e inserido na propriedade `Atual`.

Além de gerenciar os modulos, esta classe permite a injeção de dependencia entre os modulos, seja pelo tipo implementado ou pela interface.

Obs.:

\* Este gerenciador é apelidado de `nativo` pois ele **não é um modulo**.

### Propeus.Modulo.Dinamico.Gerenciador
Esta classe é um `Modulo` que se comporta como um gerenciador. É necessario a instacia de um `IGerenciador` nativo para que possa funcionar de forma correta.

Este gerenciador é responsavel por carregar e descarregar dinamicamente todos os modulos de uma aplicação e tambem é responsavel por versionar os modulos impedindo a quebra ou perda de 
referencia entre os modulos, em outras palavras, caso seja adicionado uma nova versao do modulo, enquanto houver modulos consumido a versao antiga, o modulo antigo não será descarregado, entretanto
as novas solictações do modulo serão redirecinados para a nova versao dela. 

Este gerenciador possui um provedor que é responsavel por carregar e descarregar os modulos a medida que é solicitado pelo gerenciador

## Perguntas e respostas

> Posso criar um novo gerenciador?

Sim, voce pode criar um gerenciador que dependa ou nao, do `Propeus.Modulo.Dinamico.Gerenciador`, isto irá de acordo com suas necessidades

> Sou obrigado a usar o `Propeus.Modulo.Dinamico.Gerenciador` em minhas aplicações?

Depende, voce irá utilizar modulos em tempo de execução (Criar, remover, trocar)? Se sim, será necessário o uso dele,
exceto se voce for criar um gerenciador proprio.

> Eu gostei do `Propeus.Modulo.Dinamico.Gerenciador` entretando gostaria de criar/adaptar um novo o `Propeus.Modulo.Core.Gerenciador` para antender as minhas necessidades, terei que refazer ambos
os gerenciadores?

A principio não será necessário modificar o `Propeus.Modulo.Dinamico.Gerenciador` exceto se o novo `Propeus.Modulo.Core.Gerenciador` possua modificaçoes que estejam fora do escopo
da interface `IGerenciador`

> Posso usar em produção?

Pode se voce se enquadrar nos seguintes casos: 
- Quer ser demitido por justa causa
- Vingança

** Esta lista pode ser modificada a qualquer momento

> O `Propeus.Modulo.Dinamico.Gerenciador` possui problemas de vazamento de memoria?

Nos testes foi detectado vazamento de memoria quando utilizava o `System.Reflection.Emit`, más foi resolvido (eu acho)

> Por que foi criado "provedores" que assumem o papel de um gerenciador?

Alem de facilitar o desenvolvimento, serve como base para que outros possam utilizar em seus proprios gerenciadores.

> Posso criar mais niveis de gerenciadores?

Pode, mas cada nivel é mais CPU e memória na sua conta de energia

> Este projeto funciona 100%?

Na minha maquina funcionou :). Este projeto ainda esta em fase de PoC (Prova de Conceito), então ainda existem muitas 
funcionalidades que nao estao funcionando como deveria, por exemplo, renderização de Razor Views em modulos MVC.

> Posso trabalhar com o gerenciador dentro de um escopo `using`?

Pode, é muito util para testes unitarios ou quando requer o gerenciador para algo especifico, más não é recomendavel
pois dependendo do inicializador, o custo para iniciar e descartar é alto.

> Posso usar o gerenciador em `using` aninhado?

Não, o gerenciador é estatico e somente quando é chamado o `Dispose()` que é gereado uma nova instancia dele.

> Porque não é permitido a criação de mais de uma instancia de Gerenciadores simultanamete?

O principal motivo é para evitar o descontrole de criação e descarte indevido de modulos, más tambem, serve para facilitar a manutenção e
depuração do que acontece em seu programa.