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

                $scope.history = JSON.parse(data);

                Notification.success({message: "Order History", delay: 3000});
                var animals = JSON.parse(data);
                for (var i = 0; i < animals.length; i++) {
                    Notification.success({
                        message: animals[i].Name + " " + animals[i].Gender,
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
            "phone": $scope.phone,
            "age": $scope.age,
            "gender": $scope.gender,
            "image": $scope.image
        };
        if (formData.name != null && formData.type != null && formData.age != null && formData.gender != null && formData.image != null) {
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
        Notification.success({message: 'Animal successfuly added', delay: 1000});
        self.fetchAllPets();
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
            formData.petId = uuid.unparse(formData.petId);
            $.ajax({
                url: path + '/request',
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    sessionStorage.setItem('token', data);
                    Notification.success({message: 'Animal successfuly been take from shelter', delay: 1000});
                },
                data: JSON.stringify(formData)
            });
        }
        else {
            Notification.error({message: 'sadasdasAnimal not added fill the form!', delay: 1000});
        }
        self.fetchAllPets();
    };

    self.fetchAllPets = function () {
        PetService.fetchAllPets()
            .then(
                function (d) {
                    self.pets = JSON.parse(d);
                    for (var i = 0; i < self.pets.length; i++) {
                        self.pets[i].Id = uuid.parse(self.pets[i].Id);
                    }
                },
                function (errResponse) {
                    console.error('Error while fetching Currencies');
                }
            );
    };
	
	
	$scope.other = function(){
        var hist = document.getElementById('maText').value;
        alert(hist);
		    $.ajax({
            url: path + '/mebyemail',
            type: 'post',
            dataType: 'json',
            success: function (data) {
                Notification.success({message: "Orders for " + sessionStorage.getItem('token'), delay: 3000});
                var animals = JSON.parse(data);
                var list = document.getElementById("other");
                var HTML = "";
                for (var i = 0; i < animals.length; i++) {
                    HTML += "<thead>"+"<tr>"+"<th>"+"</th>"+"<th>"+"Name"+"</th>"+"<th>"+"Type"+"</th>"+"<th>"+"Age"+"</th>" + "<th>"+"Gender"+"</th>"+"</tr>"+"</thead>";
                    HTML += "<tr>" + "<td class='vert-align'><img class='img-thumbnail' style='width:200px;height:200px;' src='" + animals[i].Image + "'/></td>" +
                        "<td class='vert-align'>" + animals[i].Name + "</td>" + "<td class='vert-align'>" + animals[i].Gender + "</td>" + "<td class='vert-align'>" + animals[i].Age + "</td>" +
                        "<td class='vert-align'd>" + animals[i].Gender + "</td>" + "</tr>";
                }
                document.getElementById("history").innerHTML = HTML;
            },
                //added Stringify
            data: JSON.stringify(hist) ,
            timeout: 10000
        });
	};
	
	

    $scope.me = function () {
        $.ajax({
            url: path + '/me',
            type: 'post',
            dataType: 'json',
            success: function (data) {
                Notification.success({message: "Orders for " + sessionStorage.getItem('token'), delay: 3000});
                var animals = JSON.parse(data);
                var list = document.getElementById("history");
                var HTML = "";
                for (var i = 0; i < animals.length; i++) {
                    HTML += "<thead>"+"<tr>"+"<th>"+"</th>"+"<th>"+"Name"+"</th>"+"<th>"+"Type"+"</th>"+"<th>"+"Age"+"</th>" + "<th>"+"Gender"+"</th>"+"</tr>"+"</thead>";
                    HTML += "<tr>" + "<td class='vert-align'><img class='img-thumbnail' style='width:200px;height:200px;' src='" + animals[i].Image + "'/></td>" +
                        "<td class='vert-align'>" + animals[i].Name + "</td>" + "<td class='vert-align'>" + animals[i].Gender + "</td>" + "<td class='vert-align'>" + animals[i].Age + "</td>" +
                        "<td class='vert-align'd>" + animals[i].Gender + "</td>" + "</tr>";
                }
                document.getElementById("history").innerHTML = HTML;
            },
            data: sessionStorage.getItem('token'),
            timeout: 10000
        });
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
        }
    };
}]);

