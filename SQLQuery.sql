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
('����','�������', 'ura280@mail.ru', 'AxOn', '', ''),
('������','��������', 'decastrio@gmail.com', 'MIHLAW', '', '');

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

ALTER TABLE USERS
ADD CONSTRAINT TASKID PRIMARY KEY (TASKID)

exec sp_help 'TASKS'
EXEC sp_sproc_columns 'TASKS.USID'
exec sp_helpdb TaskManager;

INSERT INTO TASKS (USERID, TITLE, TASKSTATUS, TASKTERM, TAGS, DISCRIPTION)
VALUES
('1','��������� � ��', '��������', '20.07.2015:12:12:12', '�����������', '� ���� ���������� ������ �����');

EXEC sp_rename 'TASKS.ID', 'TASKID', 'COLUMN';

ALTER TABLE TASKS 
ADD USID int REFERENCES USERS(USERID) --1 ����� �������,  2 �������(������� ��� �����)

DELETE TASKS WHERE TASKID != 0;

EXEC sp_rename 'USERS.ID', 'USERID', 'COLUMN';

ALTER TABLE TASKS drop column USID
alter table TASKS 
add OWNERID int NOT NULL
DEFAULT '0'

ALTER TABLE TASKS DROP COLUMN OWNERID

ALTER TABLE USERS ALTER COLUMN [CONFIRM] varchar(MAX) null
