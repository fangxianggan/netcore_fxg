using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
   public class BakingMachineObserver:Observer
    {
        private ISubject _subject;
        public BakingMachineObserver(ISubject subject) {
            _subject = subject;
        }

        public override void DoThing()
        {
            Console.WriteLine("烘烤机已经把面包烘烤好了！！！{0}",_subject.SubjectState);
        }
    }
}
