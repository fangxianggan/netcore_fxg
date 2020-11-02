using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.Enum;
using NetCore.DTO.ReponseViewModel.OrderInfo;
using NetCore.DTO.RequestViewModel.ProductInfo;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_OrderInfo;
using NetCore.Services.IServices.I_Redis;
using System;
using NetCore.DTO.RequestViewModel.OrderInfo;
using Microsoft.AspNetCore.Http;
using NetCore.DTO.ReponseViewModel.ProductInfo;

namespace NetCore.Services.Services.S_OrderInfo
{
    public class OrderInfoServices : BaseServices<OrderInfo, OrderInfoViewModel>, IOrderInfoServices
    {
        private readonly IBaseDomain<OrderInfo> _baseDomain;
        private readonly IRedisServices _redisService;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderInfoServices(IHttpContextAccessor contextAccessor, IRedisServices redisService, IBaseDomain<OrderInfo> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
            _redisService = redisService;
            _contextAccessor = contextAccessor;
        }

        public HttpReponseViewModel SumbitOrderData(QGProductViewModel model)
        {
            HttpReponseViewModel httpReponse = new HttpReponseViewModel();
            #region 基础验证

            // 判断同一操作 时间很短

            // 验证用户是否登录

            //验证同一ip限制


            // 获取产品信息 判断是否已经卖完了
            _redisService.hashId = EmDataKey.ProductInfoHash.ToString();
            var pro = JsonUtil.JsonDeserializeObject<ProductInfoViewModel>(_redisService.GetValueHash(model.ProId.ToString()));
            if (pro != null)
            {
                if (pro.StockNum <= 0)
                {
                    //已经卖完了
                    httpReponse.Message = $"你太慢了，商品：{pro.ProductName}，已经被抢完了！";
                }
                else if (model.Number > pro.StockNum)
                {
                    //库存不足
                    httpReponse.Message = $"库存不足，商品：{pro.ProductName}，只剩{pro.StockNum}了！";
                }
                else if (model.Number > pro.LimitedNum)
                {
                    //
                    httpReponse.Message = $"一个账号每次最多只能抢购【{pro.ProductName}】{pro.LimitedNum}件。";
                }
                else
                {
                    // 生成抢购订单请求
                    MoQiangGouRqVM moQiangGouRq = new MoQiangGouRqVM();
                    moQiangGouRq.ProId = model.ProId;
                    moQiangGouRq.Num = model.Number;
                    moQiangGouRq.MemberRq = new MoMemberRqVM()
                    {
                        Ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),  //用户Ip
                        RqSource = (int)EmRqSource.Web,
                        Token = ""
                    };


                    #region 加入
                    var dt = DateTime.Now;
                    MoOrderInfoVM orderInfoVM = new MoOrderInfoVM()
                    {
                        OrderId = Guid.NewGuid(),
                        CreatTime = dt,
                        Num = model.Number,
                        OrderStatus = (int)EmOrderStatus.排队抢购中,
                        PayOutTime = dt.AddMinutes(30),
                        ProId = model.ProId,
                        UserId = 0,
                        ProductInfo =pro
                    };

                    //记录会员下的订单
                    _redisService.hashId = $"User_{orderInfoVM.UserId}";
                    var flag= _redisService.SetValueHash(orderInfoVM.OrderId.ToString(),JsonUtil.JsonSerialize(orderInfoVM));
                    if (flag)
                    {
                        #region 加入抢购队列
                        var k = EmDataKey.QiangOrderEqueue.ToString() + "_" + orderInfoVM.ProId;
                        _redisService.SetValueList(k,JsonUtil.JsonSerialize(orderInfoVM));
                        #endregion
                    }
                    #endregion 



                }
            }
            #endregion

            throw new NotImplementedException();
        }
    }
}
