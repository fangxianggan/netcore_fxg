using NetCore.Core.EntityModel.ReponseModels;
using NetCore.DTO.RequestViewModel.ProductInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.IServices.I_OrderInfo
{
  public  interface IOrderInfoServices
    {
        HttpReponseViewModel SumbitOrderData(QGProductViewModel model);
    }
}
