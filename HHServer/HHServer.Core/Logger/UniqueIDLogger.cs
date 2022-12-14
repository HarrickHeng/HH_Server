using HHServer.Core.HFMongoDB;
using MongoDB.Driver;

namespace HHServer.Core.Logger
{
    public class UniqueIDLogger : HFUniqueIDBase
    {
        protected override MongoClient Client => LoggerMgr.CurrClient;

        protected override string DatabaseName => "Logger";

        protected override string CollectionName => "UniqueIDLogger";
    }
}
