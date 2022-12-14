using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Model.Test
{
    public class TestRedis
    {
        public static void TestString()
        {
            RedisHelper.Set("name", "harrick", 10);

            Console.WriteLine("写入了一个Key");
        }

        public static void TestHash()
        {
            string key = "RoleHash";
            RedisHelper.HSet(key, "1", "haha1");
            RedisHelper.HSet(key, "2", "haha2");
            RedisHelper.HSet(key, "3", "haha3");
            RedisHelper.HSet(key, "4", "haha4");
            RedisHelper.HSet(key, "5", "haha5");

            //string value = RedisHelper.HGet(key, "6");
            string value = RedisHelper.CacheShell(key, "6", -1, () => { return "haha819"; });

            Console.WriteLine($"写入了一个Value={value}");
        }

        public static void TestList()
        {
            string key = "List";
            RedisHelper.LPush(key, "item1", "item2", "item3", "item4", "item5");
            RedisHelper.LInsertAfter(key, "item3", "item3After");
            string value = RedisHelper.LIndex(key, 3);
            Console.WriteLine($"查询一个Value={value}");
        }

        public static void TestSet()
        {
            string key = "Set";
            RedisHelper.SAdd(key, "set1", "set2", "set2");
            long ret = RedisHelper.SAdd(key, "set8");
            bool b = RedisHelper.SIsMember(key, "set8");
            long count = RedisHelper.SCard(key);
            RedisHelper.SRem(key, "set8");
            Console.WriteLine($"查询一个Value={ret} {count}");
        }

        public static void TestZSet()
        {
            string key = "ZSet";
            RedisHelper.Del(key);
            //for (int i = 0; i < 20000; i++)
            //{
            //    int roleId = i;
            //    decimal score = new Random(i).Next(0, 100);
            //    RedisHelper.ZAdd(key, (score, roleId));
            //}
            //string[] arr = RedisHelper.ZRange(key, 0, 15);
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    Console.WriteLine($"有序集合{arr[i]}");
            //}

            //var lst = RedisHelper.ZRevRangeWithScores(key, 0, 19);
            //foreach (var item in lst)
            //{
            //    Console.WriteLine($"有序集合={item.member} {item.score}");
            //}
        }
    }
}
