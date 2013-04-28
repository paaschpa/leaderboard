using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyLeaderBoardServiceModel;
using PartyLeaderBoardServiceModel.Operations;
using PartyLeaderboardServices;
using ServiceStack.OrmLite;

namespace PartyLeaderBoardServices
{
    public class ScoresService : LeaderBoardServiceBase
    {
        public PartyLeaderBoard Get(PartyLeaderBoard request)
        {
            var userScoresSql = @"Select Name, Sum(Score) as TotalScore, RANK() OVER (ORDER BY Sum(Score) DESC) AS Ranking
                                    From UserScores
                                    Group By Name Order By Sum(Score) Desc";
            var userScores = DbConnExec((con) => con.Select<UserTotalScore>(userScoresSql));

            var cutLine = userScores.Average(x => x.TotalScore);

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

        public UserScore Post(UserScore request)
        {
            request.ScoreDate = DateTime.Now;
            DbConnExecTransaction((con) =>
            {
                con.Insert<UserScore>(request);
                request.Id = (int)con.GetLastInsertId();
            });

            return request;
        }
    }
}
