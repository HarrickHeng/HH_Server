using HHServer.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Model.Logic.DBModels
{
    public sealed class DBModelMgr
    {
        private static object m_lock = new object();

        #region UniqueIDGameServer
        private static UniqueIDGameServer m_UniqueIDGameServer;

        public static UniqueIDGameServer UniqueIDGameServer
        {
            get
            {
                if (m_UniqueIDGameServer == null)
                {
                    lock (m_lock)
                    {
                        if (m_UniqueIDGameServer == null)
                        {
                            m_UniqueIDGameServer = new UniqueIDGameServer();
                        }
                    }
                }
                return m_UniqueIDGameServer;
            }
        }

        #endregion


        #region RoleDBModel
        private static RoleDBModel m_RoleDBModel;

        public static RoleDBModel RoleDBModel
        {
            get
            {
                if (m_RoleDBModel == null)
                {
                    lock (m_lock)
                    {
                        if (m_RoleDBModel == null)
                        {
                            m_RoleDBModel = new RoleDBModel();
                        }
                    }
                }
                return m_RoleDBModel;
            }
        }

        #endregion
    }
}
