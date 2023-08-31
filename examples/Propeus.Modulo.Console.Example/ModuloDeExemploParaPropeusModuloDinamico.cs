using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Example
{
    /**
     * Para criar um modulo, é necessário ter o atributo ModuleProxy e herdar de IModule, entretanto é recomendável usar a classe BaseModule, 
     * pois ja foi implementado os recursos necessários para o modulo funcionar
     * 
     * Observação, durante o uso do gerenciador dinâmico, lembre-se sempre de deixar o contrato como **public** pois por ser dinâmico haverá erro de acessibilidade 
     * durante a criação do modulo
     * **/
    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamico : BaseModule
    {
        //O parâmetro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilizando uma unica instancia quando houver.
        //Por padrão o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloDinamico() : base()
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
         * Qualquer modulo, possui a liberdade de manipular o gerenciador, sendo assim, podendo até mesmo remover outros módulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IModuleManager como parâmetro, entretendo, será necessário criar um novo nível de gerenciador 
         * que consiga realizar esta operação
        **/
    }

    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria : BaseModule
    {
        private readonly IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf outroModulo;

        public ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf outroModulo) : base()
        {
            this.outroModulo = outroModulo;
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            outroModulo.EscreverOutraCoisaParaOutroContrato();
        }
    }

    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional : BaseModule
    {
        private readonly IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente outroModulo;

        public ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente outroModulo = null) : base()
        {
            this.outroModulo = outroModulo;
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            if (outroModulo is not null)
            {
                outroModulo.EscreverOlaMundo();
            }
            else
            {
                System.Console.WriteLine("Olá mundo!");
                System.Console.WriteLine("Este modulo nao possui a sua dependencia pois ela nao foi carregada ou nao existe :)");
            }
        }
    }

    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia : BaseModule
    {
        private string mensagem;
        private int a;
        private int b;

        public ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia() : base()
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
            mensagem ??= "O valor da some é...";
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Olá mundo!");
            System.Console.WriteLine("Este modulo possui uma pequena funcionalidade!!!");

            System.Console.WriteLine($"{mensagem}: {a + b}");

        }
    }

    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracaoExample : BaseModule
    {
        private string mensagem;
        private int a;
        private int b;

        public ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracaoExample() : base()
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
            mensagem ??= "O valor da some é...";
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
