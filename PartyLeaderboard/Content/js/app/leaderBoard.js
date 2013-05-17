var ngLeaderBoard = angular.module('ngLeaderBoard', []);

ngLeaderBoard.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "list-template.html",
            controller: "LeaderBoardCtrl"
        })
        .when("/:name", {
            templateUrl: "user-scores.html",
            controller: "UserCtrl"
        });
});

ngLeaderBoard.controller("LeaderBoardCtrl", function ($scope, $http) {
    $scope.users = [];
    $scope.user = 'test';
    var leaderBoardResults = $http.get('/api/party/' + 1 + '/leaderboard');
    leaderBoardResults.success(function (data) {
        $scope.cutLine = data.cutLine;
        $scope.userScores = data.userScores;
    });

    $scope.refreshScores = function () {
        var refreshedData = $http.get('/api/party/' + 1 + '/leaderboard');
        refreshedData.success(function (data) {
            $scope.cutLine = data.cutLine;
            $scope.userScores = data.userScores;
        });
    };

    $scope.addScore = function () {
        var userScore = { name: $scope.user.name, score: $scope.newUserScoreScore, notes: $scope.newUserScoreNotes };
        $http.post('/api/userscore', userScore)
            .success(function (data) {
                console.log('success');
            })
            .error(function (data) {
                console.log('failure');
            });

        $('#modalNewScore').modal('hide');
        refreshScores();
    };
});

ngLeaderBoard.controller("UserCtrl", function ($scope, $http, $routeParams, dateFilter) {

    var userResults = $http.get('/api/score/' + $routeParams.name);
    $scope.userName = $routeParams.name;
    userResults.success(function (data) {
        $scope.scores = data;
    });

    $scope.dateFormat = function (d) {
        if (d) {
            var dt = new Date(parseInt(d.substr(6)));
            return dateFilter(dt, 'medium');
        }
    };

});