/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [UserId]
      ,[FirstName]
      ,[LastName]
      ,[Email]
      ,[LoginName]
      ,[Pass]
      ,[Confirmation]
  FROM [TaskManager].[dbo].[Users]

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

SELECT * FROM TAGS

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

ALTER TABLE USERS
ADD CONSTRAINT TASKID PRIMARY KEY (TASKID)

exec sp_help 'TASKS'
EXEC sp_sproc_columns 'TASKS.USID'
exec sp_helpdb TaskManager;

INSERT INTO TASKS (USERID, TITLE, TASKSTATUS, TASKTERM, TAGS, DISCRIPTION)
VALUES
('1','Напомнить о др', 'АКТИВНЫЙ', '20.07.2015:12:12:12', 'РАЗВЛЕЧЕНИЕ', 'Я хочу поздравить своего друга');

EXEC sp_rename 'TASKS.ID', 'TASKID', 'COLUMN';

ALTER TABLE TASKS 
ADD USID int REFERENCES USERS(USERID) --1 новый столбец,  2 таблица(столбец для связи)

DELETE TASKS WHERE TASKID != 0;

EXEC sp_rename 'USERS.ID', 'USERID', 'COLUMN';

ALTER TABLE TASKS drop column USID
alter table TASKS 
add OWNERID int NOT NULL
DEFAULT '0'

ALTER TABLE TASKS DROP COLUMN OWNERID

ALTER TABLE USERS ALTER COLUMN [CONFIRM] varchar(MAX) null

CREATE TABLE TAGS
(
	ID int IDENTITY NOT NULL PRIMARY KEY,
	TITLETAG varchar(255) NOT NULL,
)

INSERT INTO TAGS (TITLETAG)
VALUES
('доходы'),
('расходы'),
('работа'),
('развлечение'),
('семейный'),
('бюджет');
GO


SELECT * FROM [TaskManager].[dbo].TASKS

  USE TaskManager
  ALTER TABLE TASKS ALTER COLUMN Taskterm DATE not null

  exec sp_help 'TASKS'

  EXEC sp_rename 'Users.Password', 'Pass', 'COLUMN';
  GO

  CREATE TABLE Statuses
  (
	ID int IDENTITY NOT NULL PRIMARY KEY,
	TitleStatus varchar(MAX) NOT NULL
	)


	ALTER TABLE Users ALTER COLUMN Confirmation int not null

	ALTER TABLE Tasks 
	ADD StatusId int REFERENCES Statuses(Id)

	DELETE Users WHERE UserId != 0;

	exec sp_help 'Tasks'
	SELECT * FROM [TaskManager].[dbo].Users



	INSERT INTO Statuses (TitleStatus)
VALUES
('Завершен'),
('Активный'),
('Сегодня последний день'),
('Потрачено');
GO


