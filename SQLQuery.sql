CREATE DATABASE TaskManager
ON
(
     NAME = 'TaskManager',
     FILENAME = 'D:\Task ManagerDB.mdf',
     SIZE = 10MB,
     MAXSIZE = 500MB,
     FILEGROWTH = 10MB
)
LOG ON
(
     NAME = 'LogTaskManagerDB',
     FILENAME = 'D:\TaskManagerDB.ldf',
     SIZE = 5MB,
     MAXSIZE = 250MB,
     FILEGROWTH = 5MB
)
COLLATE Cyrillic_General_CI_AS

USE TaskManager

CREATE TABLE USERS
(
	ID int IDENTITY NOT NULL,
	FIRST_NAME varchar(MAX)NOT NULL,
	LAST_NAME varchar(MAX)NOT NULL,
	EMAIL varchar(MAX)NOT NULL,
	LOGIN_NAME varchar(MAX)NOT NULL,
	PASS varchar(MAX)NOT NULL,
	CONFIRM varchar(MAX)NOT NULL
)

INSERT INTO USERS (FIRST_NAME, LAST_NAME, EMAIL, LOGIN_NAME, PASS, CONFIRM)
VALUES
('Юрий','Садовой', 'ura280@mail.ru', 'AxOn', '', ''),
('Михаил','Лаврищев', 'decastrio@gmail.com', 'MIHLAW', '', '');

SELECT * FROM TASKS

CREATE TABLE TASKS
(
	ID int IDENTITY NOT NULL,
	USERID int NOT NULL,
	TITLE varchar(MAX)NOT NULL,
	TASKSTATUS varchar(MAX)NOT NULL,
	TASKTERM varchar(MAX)NOT NULL,
	TAGS varchar(MAX)NOT NULL,
	DISCRIPTION varchar(MAX)NOT NULL,
)

ALTER TABLE TASKS
ADD CONSTRAINT TASKID PRIMARY KEY (TASKID)

exec sp_help 'TASKS'
exec sp_helpdb TaskManager;


INSERT INTO TASKS (USERID, TITLE, TASKSTATUS, TASKTERM, TAGS, DISCRIPTION)
VALUES
('1','Напомнить о др', 'АКТИВНЫЙ', '20.07.2015:12:12:12', 'РАЗВЛЕЧЕНИЕ', 'Я хочу поздравить своего друга');

EXEC sp_rename 'TASKS.ID', 'TASKID', 'COLUMN';

ALTER TABLE TASKS 
ADD USID int REFERENCES USERS(ID) --1 новый столбец,  2 таблица(столбец для связи)

DELETE TASKS WHERE TASKID != 0;

EXEC sp_rename 'TASKS.USERID', 'USID', 'COLUMN';