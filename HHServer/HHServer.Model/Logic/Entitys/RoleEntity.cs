using HHServer.Core.HFMongoDB;
using HHServer.Model.Logic.Entitys;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace HHServer.Model.DBModels
{
    public class RoleEntity : HFMongoEntityBase
    {
        public RoleEntity()
        {
            TaskList = new List<TaskEntity>();
            SkillDic = new Dictionary<int, SkillEntity>();
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 任务列表
        /// </summary>
        public List<TaskEntity> TaskList;

        /// <summary>
        /// 技能字典
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, SkillEntity> SkillDic;
    }
}
