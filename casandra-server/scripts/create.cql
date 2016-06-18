CREATE KEYSPACE shelter
  WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 3 };
CREATE TABLE shelter.animals (
	id uuid PRIMARY KEY,
	type varchar,
 	name varchar,
 	image varchar,
 	age float,
 	gender varchar,
  	phone varchar,
 	status boolean
);
CREATE TABLE shelter.orders (
	id uuid PRIMARY KEY,
	name varchar,
 	address varchar,
 	email varchar,
 	phone varchar,
 	pet_id uuid
);

CREATE INDEX ON shelter.orders (email);