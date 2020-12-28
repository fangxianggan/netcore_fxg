using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.DecoratorPattern
{
    public class HydroppowerProject : IDecoratorHouse
    {
        private IDecoratorHouse decorator;
        public HydroppowerProject(IDecoratorHouse decorator)
        {
            this.decorator = decorator;
        }
      
        public void DecoratorData()
        {
            decorator.DecoratorData();
            DoHydroppowerProject();
        }

        private void DoHydroppowerProject()
        {
            Console.WriteLine("开始进行水电工程！！！");
        }
    }
}
