using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.DecoratorPattern
{
    public class TestRun
    {
        public TestRun()
        {
            IDecoratorHouse house = new DecoratorHouse();
            house = new HydroppowerProject(house);
            house = new PaintingEngineering(house);
            house.DecoratorData();
        }
    }
}
