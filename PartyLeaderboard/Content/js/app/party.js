var PartyCtrl = function ($scope, $http) {

    $('#CreationSuccess').hide();
    $('#error').hide();

    $scope.players = [];
    $scope.newPlayerName = '';
    $scope.addPlayer = function (playerName) {
        $scope.players.push(playerName);
    };

    $scope.removePlayer = function (playerName) {
        var index = $scope.players.indexOf(playerName);
        $scope.players.splice(index, 1);
    };

    $scope.createParty = function () {
        var newParty = {
            'name': $scope.partyName,
            'partyDate': $scope.partyDate,
            'commissionerName': $scope.commissionerName,
            'players': $scope.players
        };

        var partyCreation = $http.post('/api/party', newParty);
        partyCreation.success(function (data) {
            $('#CreatePartyForm').hide();
            window.location = '/Party/Leaderboard/' + data.id;
        });
        partyCreation.error(function (data) {
            $('#error').show();
            $('#CreationSuccess').hide();
            $scope.errorMsg = data.responseStatus.message;
        });
    };
}