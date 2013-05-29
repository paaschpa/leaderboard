using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyLeaderBoardServiceModel;
using PartyLeaderBoardServiceModel.Operations;
using PartyLeaderboardServices;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;

namespace PartyLeaderBoardServices
{
    public class ScoresService : LeaderBoardServiceBase
    {
        public IRedisClientsManager RedisClientsManager { get; set;  }

        public PartyLeaderBoard Get(PartyLeaderBoard request)
        {
            var userScoresSql = @"Select Name, Sum(Score) as TotalScore, RANK() OVER (ORDER BY Sum(Score) DESC) AS Ranking
                                    From UserScores
                                    Where PartyId = @partyId
                                    Group By Name Order By Sum(Score) Desc";
            var userScores = DbConnExec((con) => con.Query<UserTotalScore>(userScoresSql, new {partyId = request.PartyId}));

            var cutLine = userScores.Count > 0 ? userScores.Average(x => x.TotalScore) : 0;

            foreach (var userScore in userScores)
            {
                userScore.RelationToCut = cutLine - userScore.TotalScore;
            }

            return new PartyLeaderBoard { CutLine = cutLine, UserScores = userScores };
        }

        public List<UserScore> Get(UserScores request)
        {
            var userScores = DbConnExec((con) => con.Query<UserScore>("PartyId = @partyId And Name = @name Order By ScoreDate desc", new { partyId = request.PartyId, name = request.Name }));

            return userScores;
        }

        public List<UserScore> Get(PendingScores request)
        {
            using (var redisClient = RedisClientsManager.GetClient())
            {
                var key = UrnId.Create<UserScore>(request.PartyId.ToString());
                var scores = redisClient.GetAllItemsFromList(key).Select(x => x.FromJson<UserScore>());
                return scores.ToList();
            }
        }

        [Authenticate]
        public UserScore Post(ApprovePendingUserScore request)
        {
            var newUserScore = request.TranslateTo<UserScore>();

            var commissionerId = DbConnExec((con) => con.GetById<Party>(request.PartyId)).CommissionerId;
            var userId = base.SessionAs<AuthUserSession>().UserAuthId ?? "0";

            if (commissionerId == int.Parse(userId))
            {
                DbConnExecTransaction((con) =>
                {
                    con.Insert<UserScore>(newUserScore);
                    newUserScore.Id = (int)con.GetLastInsertId();
                });
            }

            //remove from pending
            using (var redisClient = RedisClientsManager.GetClient())
            {
                var key = UrnId.Create<UserScore>(request.PartyId.ToString());
                var pendingScoreToRemove = request.TranslateTo<UserScore>();
                pendingScoreToRemove.Id = 0;
                redisClient.RemoveItemFromList(key, pendingScoreToRemove.ToJson(), 1);
            }

            return newUserScore;
        }

        public UserScore Post(CreateUserScore request)
        {   
            var newUserScore = request.TranslateTo<UserScore>();
            newUserScore.ScoreDate = DateTime.Now;

            var commissionerId = DbConnExec((con) => con.GetById<Party>(request.PartyId)).CommissionerId;
            var userId = base.SessionAs<AuthUserSession>().UserAuthId ?? "0";

            if (commissionerId == int.Parse(userId))
            {
                DbConnExecTransaction((con) =>
                    {
                        con.Insert<UserScore>(newUserScore);
                        newUserScore.Id = (int) con.GetLastInsertId();
                    });
            }
            else
            { 
                //Add to pending scores
                using (var redisClient = RedisClientsManager.GetClient())
                {
                    var key = UrnId.Create<UserScore>(request.PartyId.ToString());
                    redisClient.AddItemToList(key, newUserScore.ToJson());
                }
            }

            return newUserScore;
        }
    }
}
