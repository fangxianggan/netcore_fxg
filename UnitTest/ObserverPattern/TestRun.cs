using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
    public class TestRun
    {
        public TestRun()
        {

            #region 观察者模式
            //SmartClick click = new SmartClick();
            //click.AddObserver(new ToothpasteObserver(click));
            //click.AddObserver(new MilkMachineObserver(click));
            //click.AddObserver(new BakingMachineObserver(click));
            //click.Notify();
            #endregion

            #region  action委托
            SmartClick click1 = new SmartClick();
            Action doThing = new ToothpasteObserver(click1).DoThing;
            doThing += new MilkMachineObserver(click1).DoThing;
            doThing += new BakingMachineObserver(click1).DoThing;
            click1.doAtion = doThing;
            click1.ActionNotify();
            #endregion


        }
    }
}
