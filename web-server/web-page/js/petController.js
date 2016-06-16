/**
 * Created by Alexander on 31.05.2016.
 */
'use strict';

var url_path = 'http://myhost';

var app = angular.module('app', ['ui-notification', 'ngAnimate']);
app.controller('PetController', ['$scope', 'PetService', '$http', 'Notification', '$filter', function ($scope, PetService, $http, Notification, $filter) {

    $scope.list = [];


    var orderBy = $filter('orderBy');

    $scope.dataPetId = {
        petId: []
    };

    var self = this;

    $scope.pets = [];


    $scope.me = function () {
        $.ajax({
            url: url_path + '/me',
            type: 'post',
            dataType: 'json',
            success: function (data) {
                Notification.success({message: JSON.parse(data).Name + " " + JSON.parse(data).Email, delay: 3000});
            },
            data: sessionStorage.getItem('token'),
            timeout: 10000
        });
    };


    $scope.takeAnimal = function () {
        var formData = {
            "name": $scope.name,
            "email": $scope.email,
            "phone": $scope.phone,
            "address": $scope.address,
            "petId": $scope.petId
        };
        if (formData.petId != null && formData.address != null && formData.name != null && formData.phone != null && formData.email != null) {

            $.ajax({
                url: url_path + '/request',
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    sessionStorage.setItem('token', JSON.stringify(data));
                    Notification.success({message: 'Animal successfuly been take from shelter', delay: 1000});
                },
                data: JSON.stringify(formData)
            });
        }


        else {
            Notification.error({message: 'Animal not added fill the form!', delay: 1000});
        }
        self.fetchAllPets();

    };


    self.fetchAllPets = function () {
        PetService.fetchAllPets()
            .then(
                function (d) {
                    self.pets = JSON.parse(d);
                },
                function (errResponse) {
                    console.error('Error while fetching Currencies');
                }
            );
    };


    self.fetchAllPets();

}]);

app.factory('PetService', ['$http', '$q', function ($http, $q) {
    return {
        fetchAllPets: function () {
            return $http.get(url_path + '/all', {timeout: 7000})
                .then(
                    function (response) {
                        return response.data;
                    },
                    function (errResponse) {
                        console.error('Error while fetching pets');
                        return $q.reject(errResponse);
                    }
                );
        },


    };
}]);

