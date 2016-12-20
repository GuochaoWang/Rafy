﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Rafy;
using Rafy.ComponentModel;
using Rafy.Data;
using Rafy.Domain;
using Rafy.Domain.ORM;
using Rafy.Domain.ORM.Query;
using Rafy.Domain.Validation;
using Rafy.ManagedProperty;
using Rafy.MetaModel;
using Rafy.MetaModel.Attributes;
using Rafy.MetaModel.View;
using Rafy.RBAC.RoleManagement;

namespace Rafy.RBAC.DataPermissionManagement
{
    /// <summary>
    /// 数据权限
    /// </summary>
    [RootEntity, Serializable]
    public partial class DataPermission : DataPermissionManagementEntity
    {
        #region 构造函数

        public DataPermission() { }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected DataPermission(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region 引用属性

        public static readonly IRefIdProperty RoleIdProperty =
         P<DataPermission>.RegisterRefId(e => e.RoleId, ReferenceType.Normal);
        public long RoleId
        {
            get { return (long)this.GetRefId(RoleIdProperty); }
            set { this.SetRefId(RoleIdProperty, value); }
        }

        public static readonly RefEntityProperty<Role> RoleProperty =
            P<DataPermission>.RegisterRef(e => e.Role, RoleIdProperty);
        /// <summary>
        /// 角色
        /// </summary>
        public Role Role
        {
            get { return this.GetRefEntity(RoleProperty); }
            set { this.SetRefEntity(RoleProperty, value); }
        }

        public static readonly IRefIdProperty ResourceIdProperty =
            P<DataPermission>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);
        public long ResourceId
        {
            get { return (long)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        public static readonly RefEntityProperty<Resource> ResourceProperty =
            P<DataPermission>.RegisterRef(e => e.Resource, ResourceIdProperty);
        /// <summary>
        /// 资源
        /// </summary>
        public Resource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 组合子属性

        #endregion

        #region 一般属性

        public static readonly Property<string> DataPermissionConstraintBuilderTypeProperty = P<DataPermission>.Register(e => e.DataPermissionConstraintBuilderType);
        /// <summary>
        /// 使用的数据权限的条件生成器的类型。
        /// </summary>
        public string DataPermissionConstraintBuilderType
        {
            get { return this.GetProperty(DataPermissionConstraintBuilderTypeProperty); }
            set { this.SetProperty(DataPermissionConstraintBuilderTypeProperty, value); }
        }

        public static readonly Property<string> BuilderPropertiesProperty = P<DataPermission>.Register(e => e.BuilderProperties);
        /// <summary>
        /// 数据权限的条件生成器所需要的属性的集合。（JSON格式）
        /// </summary>
        public string BuilderProperties
        {
            get { return this.GetProperty(BuilderPropertiesProperty); }
            set { this.SetProperty(BuilderPropertiesProperty, value); }
        }

        #endregion

        #region 只读属性

        #endregion

        public void SetBuilder(DataPermissionConstraintBuilder buider)
        {
            this.DataPermissionConstraintBuilderType = buider.GetType().AssemblyQualifiedName;

            //JSON 序列化这个对象的所有属性到 BuilderProperties 属性中。
            //this.BuilderProperties = JSONFY(buider);
            throw new NotImplementedException();
        }

        public DataPermissionConstraintBuilder CreateBuilder()
        {
            var builderType = Type.GetType(this.DataPermissionConstraintBuilderType);

            var constraintBuilder = Activator.CreateInstance(builderType, true) as DataPermissionConstraintBuilder;

            //通用 JSON 反序列化，把 dataPermission.BuilderProperties 中定义的值反射写入 constraintBuilder 对象中。
            throw new NotImplementedException();

            //return constraintBuilder;
        }
    }

    /// <summary>
    /// 实体的领域名称 列表类。
    /// </summary>
    [Serializable]
    public partial class DataPermissionList : DataPermissionManagementEntityList { }

    /// <summary>
    /// 数据权限 仓库类。
    /// 负责 实体的领域名称 类的查询、保存。
    /// </summary>
    public partial class DataPermissionRepository : DataPermissionManagementEntityRepository
    {
        /// <summary>
        /// 单例模式，外界不可以直接构造本对象。
        /// </summary>
        protected DataPermissionRepository() { }

        /// <summary>
        /// 获取资源角色的数据权限集合
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="roleIdList">角色Id集合</param>
        /// <returns></returns>
        [RepositoryQuery]
        public virtual DataPermissionList GetDataPermissionList(long resourceId, List<long> roleIdList)
        {
            var f = QueryFactory.Instance;
            var t = f.Table<DataPermission>();
            var q = f.Query(
                selection: t.Star(),//查询所有列
                from: t,//要查询的实体的表
                where: f.And(t.Column(DataPermission.ResourceIdProperty).Equal(resourceId),
                t.Column(DataPermission.RoleIdProperty).In(roleIdList))
            );
            return (DataPermissionList)this.QueryData(q);
        }
    }

    /// <summary>
    /// 数据权限 配置类。
    /// 负责 实体的领域名称 类的实体元数据的配置。
    /// </summary>
    internal class DataPermissionConfig : DataPermissionManagementEntityConfig<DataPermission>
    {
        /// <summary>
        /// 配置实体的元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            //配置实体的所有属性都映射到数据表中。
            Meta.MapTable().MapAllProperties();
        }
    }
}