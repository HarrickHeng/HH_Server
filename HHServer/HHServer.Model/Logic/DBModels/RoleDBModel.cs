using HHServer.Common.Managers;
using HHServer.Core.HFMongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Model.DBModels
{
    /// <summary>
    /// 角色数据管理器
    /// </summary>
    public class RoleDBModel : HFMongoDBModelBase<RoleEntity>
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.ServerDBName;

        protected override string CollectionName => "Role";
    }
}
