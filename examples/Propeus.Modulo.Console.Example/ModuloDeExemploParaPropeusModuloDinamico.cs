using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Console.Example
{
    /**
     * Para criar um modulo, é necessario ter o atributo Modulo e herdar de IModulo, entretanto é recomentavel usar a classe ModuloBase, 
     * pois ja foi implementado os recusos necessarios para o modulo funcionar
     * 
     * Observação, durante o uso do gerenciador dinamico, lembre-se sempre de deixar o contrato como **public** pois por ser dinamico haverá erro de assecibilidade 
     * durante a criação do modulo
     * **/
    [Modulo]
    public class ModuloDeExemploParaPropeusModuloDinamico : ModuloBase
    {
        //Sempre será necessario que o construtor possua o parametro IGerenciador para que o gerenciador possa gerenciar a instancia dele
        //O parametro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilziando uma unica instancia quando houver.
        //Por padrao o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloDinamico(IGerenciador gerenciador) : base(gerenciador, false)
        {
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Ola mundo!");
            System.Console.WriteLine("Este é um modulo dinamico em funcionamento!");
        }

        public void EscreverOutraCoisaParaOutroContrato()
        {
            System.Console.WriteLine("Esta função está em outro contrato !!!");
        }

        public void EscreverOutraCoisaQueOContratoNaoPossui()
        {
            System.Console.WriteLine("Esta função não está no contrato ;)");
        }

        /**Obs.:
         * Qualquer modulo, possui a liberdade de manipuar o gerenciador, sendo assim, podendo até mesmo remover outros modulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IGerenciador como parametro, entretando, será necessario criar um novo nivel de gerenciador 
         * que conssiga realizar esta operação
        **/
    }

    [Modulo]
    public class ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria : ModuloBase
    {
        private readonly IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf outroModulo;

        public ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria(IGerenciador gerenciador, IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf outroModulo) : base(gerenciador, false)
        {
            this.outroModulo = outroModulo;
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            outroModulo.EscreverOutraCoisaParaOutroContrato();
        }
    }

    [Modulo]
    public class ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional : ModuloBase
    {
        private readonly IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente outroModulo;

        public ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional(IGerenciador gerenciador, bool instanciaUnica = false, IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente outroModulo = null) : base(gerenciador, instanciaUnica)
        {
            this.outroModulo = outroModulo;
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            if (outroModulo is not null)
                outroModulo.EscreverOlaMundo();
            else
            {
                System.Console.WriteLine("Olá mundo!");
                System.Console.WriteLine("Este modulo nao possui a sua dependencia pois ela nao foi carregada ou nao existe :)");
            }
        }
    }

    [Modulo]
    public class ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia : ModuloBase
    {
        private string mensagem;
        private int a;
        private int b;

        public ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia(IGerenciador gerenciador) : base(gerenciador, false)
        {

        }

        public void CriarInstancia(string mensagem, int a, int b)
        {
            this.mensagem = mensagem;
            this.a = a;
            this.b = b;
        }

        public void CriarInstancia(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public void CriarConfigurcao()
        {
            if(mensagem is null)
            {
                mensagem = "O valor da some é...";
            }
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Olá mundo!");
            System.Console.WriteLine("Este modulo possui uma pequena funcionalidade!!!");

            System.Console.WriteLine($"{mensagem}: {a + b}");

        }
    }

    [Modulo]
    public class ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao : ModuloBase
    {
        private string mensagem;
        private int a;
        private int b;

        public ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao(IGerenciador gerenciador) : base(gerenciador, false)
        {

        }

        public void CriarInstancia(string mensagem, int a, int b)
        {
            this.mensagem = mensagem;
            this.a = a;
            this.b = b;
        }

        public void CriarInstancia(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public void CriarConfiguracao()
        {
            if (mensagem is null)
            {
                mensagem = "O valor da some é...";
            }
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Olá mundo!");
            System.Console.WriteLine("Este modulo possui uma pequena funcionalidade!!!");

            System.Console.WriteLine($"{mensagem}: {a + b}");

        }
    }
}
