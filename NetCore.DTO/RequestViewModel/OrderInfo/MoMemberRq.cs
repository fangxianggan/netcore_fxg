using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.OrderInfo
{


    #region 抢购请求
    public class MoQiangGouRqVM : MoRqVM
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public Guid ProId { get; set; }

        /// <summary>
        /// 商品数量 默认1件，根据后台设置
        /// </summary>
        public int Num { get; set; } = 1;

    }
    #endregion

    /// <summary>
    /// 会员实体
    /// </summary>
    public class MoRqVM
    {
        public MoMemberRqVM MemberRq { get; set; }
    }
    /// <summary>
    /// 抢购会员的信息
    /// </summary>
    public class MoMemberRqVM
    {
        /// <summary>
        /// 用户身份Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用户请求Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 请求端id  EnumHelper.EmRqSource
        /// </summary>
        public int RqSource { get; set; }
        //设备号
    }
}
