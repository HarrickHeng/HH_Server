using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace HHServer.Core.HFMongoDB
{
    [BsonIgnoreExtraElements]
    /// <summary>
    /// MongoEntity基类
    /// </summary>
    public class HFMongoEntityBase
    {
        [JsonConverter(typeof(HFObjectIdConverter))]
        /// <summary>
        /// Id
        /// </summary>
        public ObjectId Id;

        /// <summary>
        /// 主键
        /// </summary>
        public long HFId;

        /// <summary>
        /// 状态
        /// </summary>
        public DataStatus Status;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime;
    }
}
