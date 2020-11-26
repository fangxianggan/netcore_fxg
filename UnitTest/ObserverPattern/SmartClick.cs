using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
    /// <summary>
    /// 闹钟生产者
    /// </summary>
    public class SmartClick : ISubject
    {
        private  List<Observer> observers;

        /// <summary>
        /// 定义委托
        /// </summary>
        public Action doAtion ;
        public string SubjectState { get ; set; }

        public SmartClick()
        {
            observers = new List<Observer>();
            SubjectState = "现在早上6点整开始闹铃。。。。。";
        }

        public void AddObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void Notify()
        {
            foreach (var item in observers)
            {
                item.DoThing();
            }
            
        }

        /// <summary>
        /// 委托通知
        /// </summary>
        public void ActionNotify()
        {
            doAtion();
        }
    }
}
