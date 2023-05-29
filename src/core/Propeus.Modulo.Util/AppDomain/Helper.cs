using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Util.AppDomain
{
    /**
     * Criar um sistema de transporte de mensagem usando o System.AppDomain.CurrentDomain.SetData
     * **/

    class DommainBroker
    {

        public delegate void Sender (string queue,object data);
        public delegate void Reciver(object data);

        public DommainBroker() { }

        public void CreateQueue(string queueName)
        {
            System.AppDomain.CurrentDomain.SetData(queueName, null);
        }

        public void AddListener(string queueName,string listener)
        {

        }

    }

    public static partial class Helper
    {
        
        
    }
}
