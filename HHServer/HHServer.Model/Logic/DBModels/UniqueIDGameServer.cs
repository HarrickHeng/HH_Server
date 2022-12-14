using HHServer.Common.Managers;
using HHServer.Core.HFMongoDB;
using MongoDB.Driver;

namespace HHServer.Model.Logic.DBModels
{
    public class UniqueIDGameServer : HFUniqueIDBase
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.ServerDBName;

        protected override string CollectionName => "UniqueIDGameServer";
    }
}
