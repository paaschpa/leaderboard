var PartyCtrl = function ($scope, $http) {

    $('#CreationSuccess').hide();
    $('#error').hide();

    $scope.createParty = function () {
        var newParty = {
            'name': $scope.partyName,
            'partyDate': $scope.partyDate,
            'commissionerName': $scope.commissionerName
        };

        var partyCreation = $http.post('/api/party', newParty);
        partyCreation.success(function (data) {
            $('#CreatePartyForm').hide();
            window.location = '/Party/' + data.id;
        });
        partyCreation.error(function (data) {
            $('#error').show();
            $('#CreationSuccess').hide();
            $scope.errorMsg = data.responseStatus.message;
        });
    };
}