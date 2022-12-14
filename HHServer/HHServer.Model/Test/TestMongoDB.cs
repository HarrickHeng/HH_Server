using HHServer.Common.Managers;
using HHServer.Core.Logger;
using HHServer.Core.Utils;
using HHServer.Model.DBModels;
using HHServer.Model.Logic.DBModels;
using HHServer.Model.Logic.Entitys;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HHServer.Model.Test
{
    public class TestMongoDB
    {
        /// <summary>
        /// 测试添加
        /// </summary>
        public async static void TestAdd()
        {
            RoleDBModel roleDBModel = DBModelMgr.RoleDBModel;
            UniqueIDGameServer uniqueIDGameServer = new UniqueIDGameServer();

            RoleEntity roleEntity = new RoleEntity();
            roleEntity.HFId = uniqueIDGameServer.GetUniqueID((int)CollectionType.Role);
            roleEntity.NickName = "harrick" + new Random().Next(1, 8888);
            roleEntity.Level = 1;
            roleEntity.CreateTime = DateTime.Now;
            roleEntity.UpdateTime = DateTime.Now;

            roleEntity.TaskList.Add(new TaskEntity() { TaskId = 1, CurrStatus = 0 });
            roleEntity.TaskList.Add(new TaskEntity() { TaskId = 2, CurrStatus = 1 });

            roleEntity.SkillDic[1] = new SkillEntity() { SkillId = 1, CurrLevel = 1 };
            roleEntity.SkillDic[2] = new SkillEntity() { SkillId = 2, CurrLevel = 0 };
            roleEntity.SkillDic[3] = new SkillEntity() { SkillId = 3, CurrLevel = 12 };
            roleEntity.SkillDic[4] = new SkillEntity() { SkillId = 4, CurrLevel = 33 };

            await roleDBModel.AddAsync(roleEntity);

            LoggerMgr.Log(Core.LoggerLevel.Log, 0, "Add Role HFId={0}", roleEntity.HFId);

            await RedisHelper.HSetAsync(ServerConfig.RoleHashKey, roleEntity.HFId.ToString(), roleEntity);
            Console.WriteLine("添加完毕！");
        }

        public static async void GetRoleFromRedis(long roleId)
        {
            var entity = await HFRedisHelper.HFCacheSheellAsync<RoleEntity>(ServerConfig.RoleHashKey, roleId.ToString(), GetRoleFromMongodb);
            if (entity != null)
            {
                Console.WriteLine("nickName= " + entity.NickName);
            }
            else
            {
                Console.WriteLine("数据不存在");
            }
        }

        private static async Task<RoleEntity> GetRoleFromMongodb(string field)
        {
            // 从Mongodb中读取
            return await DBModelMgr.RoleDBModel.GetEntityAsync(long.Parse(field));
        }

        /// <summary>
        /// 测试查询
        /// </summary>
        public static void TestSearch()
        {
            RoleDBModel roleDBModel = DBModelMgr.RoleDBModel;

            //var filter = Builders<RoleEntity>.Filter.Or(
            //    Builders<RoleEntity>.Filter.Eq(t => t.HFId, 12),
            //    Builders<RoleEntity>.Filter.Eq(t => t.HFId, 18),
            //    Builders<RoleEntity>.Filter.Eq(t => t.NickName, "harrick2934")
            //    );

            var filter = Builders<RoleEntity>.Filter.Ne(t => t.HFId, 1000);

            //var entity = roleDBModel.GetEntity(filter);
            //if (entity != null)
            //{
            //    Console.WriteLine($"entity={entity.NickName}");
            //}
            var sort = Builders<RoleEntity>.Sort.Ascending(t => t.HFId);
            var lst = roleDBModel.GetListByPage(filter, 10, 1, out long count, field: new string[]
            {
                "HFId","NickName"
            }, sort);
            for (int i = 0; i < lst.Count; i++)
            {
                Console.WriteLine($"lst Id={lst[i].HFId} name={lst[i].NickName}");
            }
        }

        public static void TestUniqueID()
        {
            UniqueIDGameServer uniqueIDGameServer = new UniqueIDGameServer();
            var roleId = uniqueIDGameServer.GetUniqueID((int)CollectionType.Email);

            Console.WriteLine($"roleId={roleId}");
        }
    }
}
