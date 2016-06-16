/**
 * Created by Alexander on 31.05.2016.
 */
'use strict';

var path = 'http://myhost';

var app = angular.module('app', ['ui-notification', 'ngAnimate']);
app.controller('PetController', ['$scope', 'PetService', '$http', 'Notification', '$filter', function ($scope, PetService, $http, Notification, $filter) {

    $scope.list = [];

    $scope.history = [];

    var orderBy = $filter('orderBy');

    $scope.dataPetId = {
        petId: []
    };

    var self = this;

    $scope.pets = [];


    $scope.me = function () {
        $.ajax({
            url: path + '/me',
            type: 'post',
            dataType: 'json',
            success: function (data) {
                //Notification.success({message: JSON.parse(data).Name + " " + JSON.parse(data).Email , delay: 3000});

                Notification.success({message: "Order History", delay: 3000});
                for (var i = 0; i < data.length; i++) {
                    Notification.success({
                        message: JSON.parse(data[i]).Name + " " + JSON.parse(data[i]).Gender,
                        delay: 3000
                    });
                }
            },
            data: sessionStorage.getItem('token'),
            timeout: 10000
        });
    };


    $scope.addAnimal = function () {
        var formData = {
            "name": $scope.name,
            "type": $scope.type,
            "age": $scope.age,
            "gender": $scope.gender,
            "img": $scope.img
        };
        if (formData.name != null && formData.type != null && formData.age != null && formData.gender != null && formData.img != null) {
            $.ajax({
                url: path + '/add',
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    Notification.success({message: 'Animal successfuly added', delay: 1000});
                },
                data: JSON.stringify(formData)
            });
        }
        else {
            Notification.error({message: 'Animal not added fill the form!', delay: 1000});
        }
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
                url: path + '/request',
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
            return $http.get(path + '/all', {timeout: 7000})
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

