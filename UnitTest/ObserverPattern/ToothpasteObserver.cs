using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
    public class ToothpasteObserver : Observer
    {
        private ISubject _subject;
        public ToothpasteObserver(ISubject subject)
        {
            _subject = subject;
        }
        public override void DoThing()
        {
            Console.WriteLine("牙膏已经准备好了！！！{0}",_subject.SubjectState);
        }
    }
}
