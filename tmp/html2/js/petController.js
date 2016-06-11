/**
 * Created by Alexander on 31.05.2016.
 */
'use strict';
var app = angular.module('app', ['ui-notification', 'ngAnimate']);
var ip_address = "myhost";
app.controller('PetController', ['$scope', 'PetService', '$http', 'Notification', '$filter', function ($scope, PetService, $http, Notification, $filter) {

    $scope.list = [];

    var orderBy = $filter('orderBy');

    $scope.dataPetId = {
        petId: []
    };

    var self = this;
    $scope.info = [];
    

    $scope.pets = [];

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
              url: 'http://' + ip_address  + '/request',
              type: 'post',
              dataType: 'json',
              success: function (data) {
                Notification.success({message: 'Animal successfuly been take from shelter', delay: 1000});
              },
              data: JSON.stringify(formData)
          });
                }
        

    else {
            Notification.error({message: 'Animal not added fill form!', delay: 1000});
        }
                        self.fetchAllPets();

    };
    
    
    $scope.getMe = function(){
        alert('ok');
        PetService.getMe().then(
            function(d){
                self.info = JSON.parse(d);
                alert(self.info.Name + self.info.Address + self.info.Phone);
            },
            function(errResponse){
                console.error('Error while fetching get me');
            }
        ); 
    }
    


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
            return $http.get('http://' + ip_address + '/all')
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
        
        getMe: function(){
                        return $http.get('http://' + ip_address  + '/me')
                        .then(
                                function(response){
                                return response.data;
                                },
                        
                                function (errResponse) {
                                console.error('Error while fetching getting me');
                                return $q.reject(errResponse);
                                }
                        );
                        }
        
        
    };
}]);

