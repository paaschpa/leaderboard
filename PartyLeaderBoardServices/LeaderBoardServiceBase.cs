using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace PartyLeaderboardServices
{
    public abstract class LeaderBoardServiceBase : Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public virtual T DbConnExec<T>(Func<IDbConnection, T> dbConnFn)
        {
            using (var dbCon = DbConnectionFactory.OpenDbConnection())
            {
                return dbConnFn(dbCon);
            }
        }

        protected virtual void DbConnExecTransaction(Action<IDbConnection> dbConnFn)
        {
            using (var dbCon = DbConnectionFactory.OpenDbConnection())
            {
                using (var trans = dbCon.OpenTransaction())
                {
                    try
                    {
                        dbConnFn(dbCon);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}