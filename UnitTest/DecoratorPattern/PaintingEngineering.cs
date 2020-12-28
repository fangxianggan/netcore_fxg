using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.DecoratorPattern
{
    /// <summary>
    /// 油漆工
    /// </summary>
    public class PaintingEngineering : IDecoratorHouse
    {
        private IDecoratorHouse decorator;
        public PaintingEngineering(IDecoratorHouse decorator)
        {
            this.decorator = decorator;
        }

        public void DecoratorData()
        {
            this.decorator.DecoratorData();
            PaintingEngineeringData();
        }

        private void PaintingEngineeringData()
        {
            Console.WriteLine("油漆工开始进场！！！");
        }
    }
}
