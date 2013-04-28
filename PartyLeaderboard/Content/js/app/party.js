var PartyCtrl = function ($scope, $http) {

    $('#CreationSuccess').hide();
    $('#error').hide();

    $scope.createParty = function () {
        var newParty = {
            'name': $scope.partyName,
            'partyDate': $scope.partyDate,
        };

        var partyCreation = $http.post('/api/party', newParty);
        partyCreation.success(function (data) {
            $('#CreatePartyForm').hide();
            $('#CreationSuccess').show();
        });
        partyCreation.error(function (data) {
            $('#error').show();
            $('#CreationSuccess').hide();
            $scope.errorMsg = data.responseStatus.message;
        });
    };
}