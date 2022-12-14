using HHServer.Core.Logger;
using HHServer.Model;
using HHServer.Model.Test;
using System;

namespace HHServer.WorldServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerMgr.Init();
            HFRedisClient.InitRedisClient();

            //TestMongoDB.TestAdd();
            TestMongoDB.GetRoleFromRedis(2);
            //TestRedis.TestString();
            //TestRedis.TestHash();
            //TestRedis.TestList();
            //TestRedis.TestSet();
            //TestRedis.TestZSet();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
