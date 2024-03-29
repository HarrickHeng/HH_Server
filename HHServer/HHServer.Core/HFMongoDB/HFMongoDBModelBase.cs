﻿using HHServer.Core.Logger;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HHServer.Core.HFMongoDB
{
    /// <summary>
    /// MongoDBModel基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HFMongoDBModelBase<T> where T : HFMongoEntityBase, new()
    {
        #region 子类需要实现的属性

        /// <summary>
        /// MongoClient
        /// </summary>
        protected abstract MongoClient Client
        {
            get;
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        protected abstract string DatabaseName
        {
            get;
        }

        /// <summary>
        /// 集合名称
        /// </summary>
        protected abstract string CollectionName
        {
            get;
        }

        /// <summary>
        /// 是否记录错误日志
        /// </summary>
        protected virtual bool CanLogError
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region GetCollection 获取文档集合

        private IMongoCollection<T> m_Collection = null;

        /// <summary>
        /// 获取文档集合
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection()
        {
            try
            {
                if (m_Collection == null)
                {
                    IMongoDatabase dataBase = Client.GetDatabase(DatabaseName);
                    m_Collection = dataBase.GetCollection<T>(CollectionName);
                }
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
            return m_Collection;
        }

        #endregion

        #region GetEntity 根据编号查询实体

        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="HFId"></param>
        /// <returns>实体</returns>
        public T GetEntity(long HFId)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<T>.Filter.Eq("HFId", HFId);
                return collection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T GetEntity(FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection();
                return collection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        #endregion

        #region GetEntityAsync 根据编号异步查询实体

        /// <summary>
        /// 根据编号异步查询实体
        /// </summary>
        /// <param name="HFId"></param>
        /// <returns>实体</returns>
        public async Task<T> GetEntityAsync(long HFId)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<T>.Filter.Eq("HFId", HFId);
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        /// <summary>
        /// 根据编号异步查询实体
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<T> GetEntityAsync(FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        #endregion

        #region GetCount 查询数量

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>数量</returns>
        public long GetCount(FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection();
                return collection.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return 0;
            }
        }

        #endregion

        #region GetCountAsync 异步查询数量

        /// <summary>
        /// 异步查询数量
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>数量</returns>
        public async Task<long> GetCountAsync(FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection();
                return await collection.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return 0;
            }
        }

        #endregion

        #region GetList 根据条件查询数据集合

        /// <summary>
        /// 根据条件查询数据集合
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="count">总数量</param>
        /// <param name="field">字段</param>
        /// <param name="sort">排序</param>
        /// <returns>数据集合</returns>
        public List<T> GetList(FilterDefinition<T> filter, out long count, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection();
                count = collection.CountDocuments(filter);

                // 不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return collection.Find(filter).ToList();
                    // 进行排序
                    return collection.Find(filter).Sort(sort).ToList();
                }

                // 指定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                // 不排序
                if (sort == null) return collection.Find(filter).Project<T>(projection).ToList();

                // 排序查询
                return collection.Find(filter).Sort(sort).Project<T>(projection).ToList();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                count = 0;
                return null;
            }
        }

        #endregion

        #region GetListAsync 根据条件异步查询数据集合

        /// <summary>
        /// 根据条件异步查询数据集合
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="field">查询字段</param>
        /// <param name="sort">排序</param>
        /// <returns>数据集合</returns>
        public async Task<List<T>> GetListAsync(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection();

                // 不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await collection.Find(filter).ToListAsync();
                    // 进行排序
                    return await collection.Find(filter).Sort(sort).ToListAsync();
                }

                // 指定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                // 不排序
                if (sort == null) return await collection.Find(filter).Project<T>(projection).ToListAsync();

                // 排序查询
                return await collection.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        #endregion

        #region GetListByPage 查询分页集合

        /// <summary>
        /// 查询分页集合
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="count">总数量</param>
        /// <param name="field">字段</param>
        /// <param name="sort">排序</param>
        /// <returns>分页集合</returns>
        public List<T> GetListByPage(FilterDefinition<T> filter, int pageSize, int pageIndex, out long count, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection();
                count = collection.CountDocuments(filter);

                // 不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                    // 进行排序
                    return collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                }

                // 指定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                // 不排序
                if (sort == null) return collection.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

                // 排序查询
                return collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                count = 0;
                return null;
            }
        }

        #endregion

        #region GetListByPageAsync 异步查询分页集合

        /// <summary>
        /// 异步查询分页集合
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="field">字段</param>
        /// <param name="sort">排序</param>
        /// <returns>分页集合</returns>
        public async Task<List<T>> GetListByPageAsync(FilterDefinition<T> filter, int pageSize, int pageIndex, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection();

                // 不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                    // 进行排序
                    return await collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                // 指定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                // 不排序
                if (sort == null) return await collection.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

                // 排序查询
                return await collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
                return null;
            }
        }

        #endregion

        #region Add 添加实体

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            try
            {
                var collection = GetCollection();
                collection.InsertOne(entity);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region AddAsync 异步添加实体

        /// <summary>
        /// 异步添加实体
        /// </summary>
        /// <param name="entity"></param>
        public async Task AddAsync(T entity)
        {
            try
            {
                var collection = GetCollection();
                await collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region AddMany 添加多个实体

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="lst"></param>
        public void AddMany(List<T> lst)
        {
            try
            {
                var collection = GetCollection();
                collection.InsertMany(lst);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region AddManyAsync 异步添加多个实体

        /// <summary>
        /// 异步添加多个实体
        /// </summary>
        /// <param name="lst"></param>
        public async Task AddManyAsync(List<T> lst)
        {
            try
            {
                var collection = GetCollection();
                await collection.InsertManyAsync(lst);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Update 修改实体

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<T>.Filter.Eq("HFId", entity.HFId);
                collection.FindOneAndReplace(filter, entity);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region UpdateAsync 异步修改实体

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<T>.Filter.Eq("HFId", entity.HFId);
                await collection.FindOneAndReplaceAsync(filter, entity);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Delete 删除实体

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="HFId"></param>
        public void Delete(long HFId)
        {
            try
            {
                T entity = GetEntity(HFId);
                if (entity != null)
                {
                    entity.Status = DataStatus.Delete;
                    Update(entity);
                }
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region DeleteAsync 异步删除实体

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="HFId"></param>
        public async Task DeleteAsync(long HFId)
        {
            try
            {
                T entity = await GetEntityAsync(HFId);
                if (entity != null)
                {
                    entity.Status = DataStatus.Delete;
                    await UpdateAsync(entity);
                }
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region DeleteAll 删除所有文档

        /// <summary>
        /// 删除所有文档
        /// </summary>
        public void DeleteAll()
        {
            try
            {
                var dataBase = Client.GetDatabase(DatabaseName);
                dataBase.DropCollection(CollectionName);
                dataBase.CreateCollection(CollectionName);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region DeleteAllAsync 异步删除所有文档

        /// <summary>
        /// 异步删除所有文档
        /// </summary>
        public async Task DeleteAllAsync()
        {
            try
            {
                var dataBase = Client.GetDatabase(DatabaseName);
                await dataBase.DropCollectionAsync(CollectionName);
                await dataBase.CreateCollectionAsync(CollectionName);
            }
            catch (Exception ex)
            {
                if (CanLogError)
                {
                    LoggerMgr.Log(LoggerLevel.LogError, 0, ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}
