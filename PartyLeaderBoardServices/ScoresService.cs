using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyLeaderBoardServiceModel;
using PartyLeaderBoardServiceModel.Operations;
using PartyLeaderboardServices;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderBoardServices
{
    public class ScoresService : LeaderBoardServiceBase
    {
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
            var userScores = DbConnExec((con) => con.Query<UserScore>("Name = @name Order By ScoreDate desc", new { name = request.Name }));

            return userScores;
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
                    newUserScore.Id = (int)con.GetLastInsertId();
                });
            }


            return newUserScore;
        }
    }
}
