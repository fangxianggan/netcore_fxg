using System;

namespace RabbitMqConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            MQTest test = new MQTest();

            //test.PDMQ();

            test.SXMQ();

           

            Console.WriteLine("Hello World!");
        }
    }
}
