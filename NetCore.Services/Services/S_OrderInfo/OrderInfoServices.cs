using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.OrderInfo;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_OrderInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.Services.S_OrderInfo
{
    public class OrderInfoServices : BaseServices<OrderInfo, OrderInfoViewModel>, IOrderInfoServices
    {
        private readonly IBaseDomain<OrderInfo> _baseDomain;
        public OrderInfoServices(IBaseDomain<OrderInfo> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
        }
    }
}
