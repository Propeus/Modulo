# Geral

### Porque nao é utilizado componentes de terceiros para a manipulação de classes em tempo de execução como o Castle Project?
São diversos pontos más o principal é porque eu não queria depender de varios projetos de terceiros, pois iria contra a ideia de ser modular.

### Qual a vantagem de usar esse projeto?
A ideia surgiu dentro de 2 ocasioes jogo e web. Hoje em dia temos diversas ferramentas que possibilitam o deploy rapido e garantia de disponibilidade, porém nem todos possuem recursos para adquirir essas ferramentes, então sugiu a ideia da necessidade de realizar o deploy de uma parte sem precisar parar uma aplicação por completo ou ate mesmo aquela parte afetada. Para os jogos, a ideia seria a mesma, seria legal receber atualizações sem a necessidade de fechar ou reiniciar o jogo.

### Como funciona a atualização de modulo no Gerenciador dinamico?
O gerencenciador verifica se existe algum evento no arquivo e caso exista, ele descarrega o assembly antigo e carrega o novo, porém os objetos que utilizam o assembly antigo continuarao até que finalize seu ciclo de vida. Para evitar que um modulo antigo permaneca ativo por muito tempo após a sua eliminação, o gerenciador sempre irá te retornar uma nova instancia de modulo ao invés de retornar o existente.

### É possivel usar esse projeto em sistemas WEB MVC?
Sim, más há resalvas, para consegui utilziar o gerenciador como controle de DI, foi necessario copiar o controle de DI da microsoft e modifica-la para quando não houver registro de serviço, ele irá buscar no gerenciador. Para manter os padrões de DI, a Microsoft decidiu que ou criaria um do zero ou utilizaria o já existente. 
Obs.: A Microsft não possui responsabilidade sobre o codigo de injeção de dependencia que foi copiado e modificado no projeto `Propeus.Modulo.Hosting`