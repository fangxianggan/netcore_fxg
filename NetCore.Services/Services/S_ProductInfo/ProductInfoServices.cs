using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.ProductInfo;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_ProductInfo;

namespace NetCore.Services.Services.S_ProductInfo
{
    public class ProductInfoServices : BaseServices<ProductInfo, ProductInfoViewModel>, IProductInfoServices
    {
        private readonly IBaseDomain<ProductInfo> _baseDomain;
        public ProductInfoServices(IBaseDomain<ProductInfo> baseDomain) : base(baseDomain)
        {
            _baseDomain = baseDomain;
        }


    }
}
