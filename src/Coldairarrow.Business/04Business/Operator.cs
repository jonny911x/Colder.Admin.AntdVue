﻿using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Util;
using System;
using static Coldairarrow.Entity.Base_Manage.EnumType;

namespace Coldairarrow.Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IDependency
    {
        #region DI

        private IBase_UserBusiness _sysUserBus { get => AutofacHelper.GetScopeService<IBase_UserBusiness>(); }
        public ILogger Logger { get => AutofacHelper.GetScopeService<ILogger>(); }

        #endregion

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId
        {
            get
            {
                if (GlobalSwitch.RunModel == RunModel.LocalTest)
                    return GlobalSwitch.AdminId;
                else
                {
                    try
                    {
                        return HttpContextCore.Current.Request.GetJWTPayload()?.UserId;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);

                        return null;
                    }
                }
            }
        }

        public Base_UserDTO Property { get => _sysUserBus.GetTheData(UserId); }

        #region 操作方法

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var role = Property.RoleType;
            if (UserId == GlobalSwitch.AdminId || role.HasFlag(RoleType.超级管理员))
                return true;
            else
                return false;
        }

        #endregion
    }
}
