using System;

namespace RabbitMqConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            MQTest test = new MQTest();
            test.PDMQ();

            Console.WriteLine("Hello World!");
        }
    }
}
