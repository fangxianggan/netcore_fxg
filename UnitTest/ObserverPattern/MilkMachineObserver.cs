using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
   public  class MilkMachineObserver:Observer
    {
        private ISubject _subject;
        public MilkMachineObserver(ISubject subject)
        {
            _subject = subject;
        }

        public override void DoThing()
        {
            Console.WriteLine("牛奶机已经把牛奶热好了！！！{0}",_subject.SubjectState);
        }
    }
}
