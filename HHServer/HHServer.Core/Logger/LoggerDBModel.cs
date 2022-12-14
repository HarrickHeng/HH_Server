using HHServer.Core.HFMongoDB;
using MongoDB.Driver;
using System;

namespace HHServer.Core.Logger
{
    public class LoggerDBModel : HFMongoDBModelBase<LoggerEntity>
    {
        protected override MongoClient Client => LoggerMgr.CurrClient;

        protected override string DatabaseName => "Logger";

        protected override string CollectionName => $"Log_{DateTime.UtcNow.ToString("yyyy-MM-dd")}";

        protected override bool CanLogError => false;
    }
}
