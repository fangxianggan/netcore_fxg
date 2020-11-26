using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DIPDesign.IService;
using UnitTest.DIPDesign.Service;

namespace UnitTest.DIPDesign
{
   public class UserService
    {
        public readonly INotify _notify;
        public UserService()
        {
            _notify = new MSNService();
        }


        public void SendNotify()
        {
            _notify.Notify();
        }
    }
}
