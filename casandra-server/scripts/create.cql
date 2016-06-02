CREATE KEYSPACE "nursery" with replication = {'class':'SimpleStrategy', 'replication_factor':3};

CREATE TABLE nursery.animals (
	id int PRIMARY KEY,
	type varchar,
 	name varchar,
 	image varchar,
 	age float,
 	gender varchar,
  	phone varchar,
 	status boolean
);