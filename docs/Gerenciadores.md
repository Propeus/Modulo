

## Gerenciadores
Neste projeto todos os gerenciadores s�o herdados da interface `Propeus.Modulo.Abstrato.Interfaces.IGerenciadores` e implementam os principais metodos:
- Criar
- Remover
- Obter
- Reciclar
- Existe
- Listar

### Propeus.Modulo.Core.Gerenciador
Esta classe � responsavel por gerenciar todos os modulos do sistema sendo o gerenciador nativo. 

Para que fosse possivel o funcionamento, foi necessario a cria��o de dois provedores
que sao, `TypeProvider`, responsavel por gerenciar todos os tipos da aplica��o que esta em execucao incluindo os modulos e `InstanciaProvider`, responsavel por gerenciar todas as 
instancias de mudulo que s�o criados ou obtidos.

Esta classe possui uma propriedade estatica que s� � inicializado quando chamado. A chamada do metodo `Dispose()` faz com que uma nova instancia do gerenciador seja
criado e inserido na propriedade `Atual`.

Al�m de gerenciar os modulos, esta classe permite a inje��o de dependencia entre os modulos, seja pelo tipo implementado ou pela interface.

Obs.:

\* Este gerenciador � apelidado de `nativo` pois ele **n�o � um modulo**.

### Propeus.Modulo.Dinamico.Gerenciador
Esta classe � um `Modulo` que se comporta como um gerenciador. � necessario a instacia de um `IGerenciador` nativo para que possa funcionar de forma correta.

Este gerenciador � responsavel por carregar e descarregar dinamicamente todos os modulos de uma aplica��o e tambem � responsavel por versionar os modulos impedindo a quebra ou perda de 
referencia entre os modulos, em outras palavras, caso seja adicionado uma nova versao do modulo, enquanto houver modulos consumido a versao antiga, o modulo antigo n�o ser� descarregado, entretanto
as novas solicta��es do modulo ser�o redirecinados para a nova versao dela. 

Este gerenciador possui um provedor que � responsavel por carregar e descarregar os modulos a medida que � solicitado pelo gerenciador

## Perguntas e respostas

> Posso criar um novo gerenciador?

Sim, voce pode criar um gerenciador que dependa ou nao, do `Propeus.Modulo.Dinamico.Gerenciador`, isto ir� de acordo com suas necessidades

> Sou obrigado a usar o `Propeus.Modulo.Dinamico.Gerenciador` em minhas aplica��es?

Depende, voce ir� utilizar modulos em tempo de execu��o (Criar, remover, trocar)? Se sim, ser� necess�rio o uso dele,
exceto se voce for criar um gerenciador proprio.

> Eu gostei do `Propeus.Modulo.Dinamico.Gerenciador` entretando gostaria de criar/adaptar um novo o `Propeus.Modulo.Core.Gerenciador` para antender as minhas necessidades, terei que refazer ambos
os gerenciadores?

A principio n�o ser� necess�rio modificar o `Propeus.Modulo.Dinamico.Gerenciador` exceto se o novo `Propeus.Modulo.Core.Gerenciador` possua modifica�oes que estejam fora do escopo
da interface `IGerenciador`

> Posso usar em produ��o?

Pode se voce se enquadrar nos seguintes casos: 
- Quer ser demitido por justa causa
- Vingan�a

** Esta lista pode ser modificada a qualquer momento

> O `Propeus.Modulo.Dinamico.Gerenciador` possui problemas de vazamento de memoria?

Nos testes foi detectado vazamento de memoria quando utilizava o `System.Reflection.Emit`, m�s foi resolvido (eu acho)

> Por que foi criado "provedores" que assumem o papel de um gerenciador?

Alem de facilitar o desenvolvimento, serve como base para que outros possam utilizar em seus proprios gerenciadores.

> Posso criar mais niveis de gerenciadores?

Pode, mas cada nivel � mais CPU e mem�ria na sua conta de energia

> Este projeto funciona 100%?

Na minha maquina funcionou :). Este projeto ainda esta em fase de PoC (Prova de Conceito), ent�o ainda existem muitas 
funcionalidades que nao estao funcionando como deveria, por exemplo, renderiza��o de Razor Views em modulos MVC.

> Posso trabalhar com o gerenciador dentro de um escopo `using`?

Pode, � muito util para testes unitarios ou quando requer o gerenciador para algo especifico, m�s n�o � recomendavel
pois dependendo do inicializador, o custo para iniciar e descartar � alto.

> Posso usar o gerenciador em `using` aninhado?

N�o, o gerenciador � estatico e somente quando � chamado o `Dispose()` que � gereado uma nova instancia dele.

> Porque n�o � permitido a cria��o de mais de uma instancia de Gerenciadores simultanamete?

O principal motivo � para evitar o descontrole de cria��o e descarte indevido de modulos, m�s tambem, serve para facilitar a manuten��o e
depura��o do que acontece em seu programa.