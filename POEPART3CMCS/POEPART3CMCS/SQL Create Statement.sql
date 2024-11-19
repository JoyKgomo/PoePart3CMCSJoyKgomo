create database poepart3;
use poepart3;

Create table user(
id int auto_increment not null primary key,
username varchar(50) not null,
password varchar(50) not null,
role varchar(50) not null
);

create table userdetails(
id int auto_increment not null primary key,
user_id int not null,
firstname varchar(50) not null,
lastname varchar(50) not null,
username varchar(50) not null,
password varchar(50) not null,
email varchar(50) not null,
cell varchar(50) not null,
role varchar(50) not null,
foreign key(user_id) references User(id)
);

create table Claim(
id int auto_increment not null primary key,
claimdate date not null,
user_id int not null,
hours int not null,
rate int not null,
foreign key(user_id) references User(id)
);

create table document(
id int auto_increment not null primary key,
document longblob,
filetype varchar(50),
claim_id int ,
foreign key(claim_id) references Claim(id)
);

create table invoice(
id int auto_increment not null primary key,
file longblob not null,
filetype varchar(50) not null,
user_id int not null,
foreign key(user_id) references User(id)
);