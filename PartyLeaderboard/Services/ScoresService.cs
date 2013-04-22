using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace PartyLeaderboard.Services
{
    [Route("/score/{Name}", "GET")]
    public class UserScores
    {
        public string Name { get; set; }
    }

    [Route("/userscore", "POST")]
    [Alias("UserScores")]
    public class UserScore
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime ScoreDate { get; set; }
        public string Notes { get; set; }
    }

    public class UserTotalScore
    {
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public int Ranking { get; set; }
        public double RelationToCut { get; set; }
    }

    [Route("/leaderboard")]
    public class LeaderBoard : IReturn<LeaderBoard>
    {
        public double CutLine { get; set; }
        public List<UserTotalScore> UserScores { get; set; }
    }

    public class ScoresService : LeaderBoardServiceBase
    {
        public LeaderBoard Get(LeaderBoard request)
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

            return new LeaderBoard {CutLine = cutLine, UserScores = userScores};
        }

        public List<UserScore> Get(UserScores request)
        {
            var userScores = DbConnExec((con) => con.Query<UserScore>("Name = @name Order By ScoreDate desc", new {name = request.Name}));

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