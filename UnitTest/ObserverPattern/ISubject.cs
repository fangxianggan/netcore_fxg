using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObserverPattern
{
    /// <summary>
    /// 订阅的主题
    /// </summary>
    public interface ISubject
    {

        string SubjectState { set; get; }
        void AddObserver(Observer observer);

        /// <summary>
        /// 通知消息
        /// </summary>
        void Notify();

        /// <summary>
        /// 委托通知
        /// </summary>
        void ActionNotify();

    }
}
