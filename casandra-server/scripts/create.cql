CREATE KEYSPACE nursery
  WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 3 };
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
CREATE TABLE nursery.orders (
	id uuid PRIMARY KEY,
	name varchar,
 	address varchar,
 	email varchar,
 	phone varchar,
 	pet_id int
);