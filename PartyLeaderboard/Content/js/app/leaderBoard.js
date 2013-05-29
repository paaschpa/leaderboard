var ngLeaderBoard = angular.module('ngLeaderBoard', []);

ngLeaderBoard.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "list-template.html",
            controller: "LeaderBoardCtrl"
        })
        .when("/player/:name", {
            templateUrl: "user-scores.html",
            controller: "UserCtrl"
        })
        .when("/pendingScores", {
            templateUrl: "pending-scores.html",
            controller: "PendingScoreCtrl"
        });
});

ngLeaderBoard.factory('pendingScoresService', function ($http) {
    var pendingScores;
    var myService = {
        async: function () {
            if (!pendingScores) {
                pendingScores = $http.get('/api/Party/' + partyIndex + '/PendingScores')
                    .then(function (response) {
                        return response.data;
                    });
            }
            return pendingScores;
        },
        refresh: function () {
            pendingScores = $http.get('/api/Party/' + partyIndex + '/PendingScores')
                    .then(function (response) {
                        return response.data;
                    });

            return pendingScores;
        }
    };
    return myService;
});

ngLeaderBoard.controller("LeaderBoardCtrl", function ($scope, $http, pendingScoresService) {
    $scope.users = [];
    getUsers(partyIndex);
    $scope.user = 'test';
    $scope.pendingScoresCount = 0;
    pendingScoresService.async().then(function (d) {
        $scope.pendingScoresCount = d.length;
    });

    var leaderBoardResults = $http.get('/api/party/' + partyIndex + '/leaderboard');
    leaderBoardResults.success(function (data) {
        $scope.cutLine = data.cutLine;
        $scope.userScores = data.userScores;
    });

    $scope.refreshScores = function () {
        var refreshedData = $http.get('/api/party/' + partyIndex + '/leaderboard');
        refreshedData.success(function (data) {
            $scope.cutLine = data.cutLine;
            $scope.userScores = data.userScores;
        });
    };

    $scope.addScore = function () {
        var userScore = { partyId: partyIndex, name: $scope.user.name, score: $scope.newUserScoreScore, notes: $scope.newUserScoreNotes };
        $http.post('/api/userscore', userScore)
            .success(function (data) {
                console.log('success');
                pendingScoresService.refresh().then(function (d) {
                    $scope.pendingScoresCount = d.length;
                });
            })
            .error(function (data) {
                console.log('failure');
            });

        $('#modalNewScore').modal('hide');
        $scope.refreshScores();
    };

    function getUsers(partyIndex) {
        $http.get('/api/party/' + partyIndex + '/players')
            .success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.users.push({ name: data[i].name });
                }
            })
            .error(function (data) {
                console.log(data);
            });
    };
});

ngLeaderBoard.controller("UserCtrl", function ($scope, $http, $routeParams, dateFilter) {

    var userResults = $http.get('/api/party/' + partyIndex + '/score/' + $routeParams.name);
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

ngLeaderBoard.controller("PendingScoreCtrl", function ($scope, $http, pendingScoresService, dateFilter) {
    pendingScoresService.async().then(function (d) {
        $scope.pendingScores = d;
    });

    $scope.approveScore = function (userScore) {
        $http.post('/api/ApprovePendingUserScore', userScore)
            .success(function (data) {
                console.log('success');
                pendingScoresService.refresh().then(function (d) {
                    $scope.pendingScores = d;
                });
            })
            .error(function (data) {
                console.log('failure');
            });
    };

    $scope.dateFormat = function (d) {
        if (d) {
            var dt = new Date(parseInt(d.substr(6)));
            return dateFilter(dt, 'medium');
        }
    };

});