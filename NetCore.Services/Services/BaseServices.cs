using NetCore.Core.Attributes;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.EntityModel.QueryModels;
using NetCore.Services.Interface;
using NetCore.Services.IServices.I_Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
namespace NetCore.Services.Services
{
    public class BaseServices<T, TView> : IBaseServices<TView> where T : class, new() where TView : class, new()
    {
        private readonly IBaseDomain<T> _baseDomain;
       
        public BaseServices(IBaseDomain<T> baseDomain)
        {
            _baseDomain = baseDomain;
         
        }
        public Task<bool> AddListService(List<TView> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpReponseObjViewModel<string>> AddOrEditService(TView entity)
        {
            var ent = entity.MapTo<T>();
            Type type = ent.GetType();
            PropertyInfo[] properties = ReflectionUtil.GetProperties(type);
            PropertyInfo pkProp = properties.Where(p => p.GetCustomAttributes(typeof(IdAttribute), true).Length > 0).FirstOrDefault();
            var value = pkProp.GetValue(ent).ToString();
            var vType = pkProp.PropertyType;
            var isAdd = false;
            if (vType.Name == "Guid")
            {
                if (value == "00000000-0000-0000-0000-000000000000")
                {
                    isAdd = true;
                    ReflectionUtil.SetPropertyValue(ent, pkProp, Guid.NewGuid());
                }
            }
            else if (vType.Name == "Int32")
            {
                if (value == "0")
                {
                    isAdd = true;
                }
            }
            var flag = true;
            if (isAdd)
            {
                flag = await _baseDomain.AddDomain(ent);
            }
            else
            {
                flag = await _baseDomain.EditDomain(ent);
            }
            string primaryV = ReflectionUtil.GetPropertyValue(ent, pkProp).ToString();
            return new HttpReponseObjViewModel<string>()
            {
                Data = primaryV,
                ExeSql = "",
                Message = flag == true ? "数据操作成功" : "数据操作失败",
                ResultSign = flag == true ? ResultSign.Success : ResultSign.Error,
                StatusCode = flag == true ? StatusCode.OK : StatusCode.InternalServerError
            };
        }

        public async Task<HttpReponseObjViewModel<string>> AddOrEditService(TView entity, bool isRedis, string hashId)
        {
            var ent = entity.MapTo<T>();
            Type type = ent.GetType();
            PropertyInfo[] properties = ReflectionUtil.GetProperties(type);
            PropertyInfo pkProp = properties.Where(p => p.GetCustomAttributes(typeof(IdAttribute), true).Length > 0).FirstOrDefault();
            var value = pkProp.GetValue(ent).ToString();
            var vType = pkProp.PropertyType;
            var isAdd = false;
            if (vType.Name == "Guid")
            {
                if (value == "00000000-0000-0000-0000-000000000000")
                {
                    isAdd = true;
                    ReflectionUtil.SetPropertyValue(ent, pkProp, Guid.NewGuid());
                }
            }
            else if (vType.Name == "Int32")
            {
                if (value == "0")
                {
                    isAdd = true;
                }
            }
            var flag = true;
            if (isAdd)
            {
                flag = await _baseDomain.AddDomain(ent);
            }
            else
            {
                flag = await _baseDomain.EditDomain(ent);
            }
            string primaryV = ReflectionUtil.GetPropertyValue(ent, pkProp).ToString();
            if (isRedis)
            {
                var _redisServices =(IRedisServices)AppConfigUtil.HttpCurrent.RequestServices.GetService(typeof(IRedisServices));

                _redisServices.hashId = hashId;
                if (!isAdd)
                {
                    if (_redisServices.IsExistKeyHash(primaryV))
                        _redisServices.RemoveHash(primaryV);
                }
                _redisServices.SetValueHash(primaryV, JsonUtil.JsonSerialize(ent));
            }
            return new HttpReponseObjViewModel<string>()
            {
                Data = primaryV,
                ExeSql = "",
                Message = flag == true ? "数据操作成功" : "数据操作失败",
                ResultSign = flag == true ? ResultSign.Success : ResultSign.Error,
                StatusCode = flag == true ? StatusCode.OK : StatusCode.InternalServerError
            };
        }

        public async Task<HttpReponseViewModel> DeleteService(object id)
        {
            var flag = await _baseDomain.DeleteDomain(id);
            return new HttpReponseViewModel(flag);
        }

        public async Task<HttpReponseViewModel> DeleteService(object id, bool isRedis, string hashId)
        {
            var flag = await _baseDomain.DeleteDomain(id);
            if (isRedis)
            {
                var _redisServices = (IRedisServices)AppConfigUtil.HttpCurrent.RequestServices.GetService(typeof(IRedisServices));
                _redisServices.hashId = hashId;
                if (_redisServices.IsExistKeyHash(id.ToString()))
                    _redisServices.RemoveHash(id.ToString());
            }
            return new HttpReponseViewModel(flag);
        }

        public async Task<HttpReponsePageViewModel<List<TView>>> GetPageListService(QueryModel queryModel)
        {
            var pageData = await _baseDomain.GetPageList(queryModel);
            var list = pageData.DataList.MapToList<TView>();
            HttpReponsePageViewModel<List<TView>> httpReponse = new HttpReponsePageViewModel<List<TView>>(list, queryModel.PageIndex, queryModel.PageSize, pageData.Total, pageData.EXESql);
            return httpReponse;
        }
    }
}
