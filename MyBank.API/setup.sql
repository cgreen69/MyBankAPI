use test


begin tran
insert into [user] (username) values ('ChrisGreen')
insert into [UserAccount] (userid,accountid) values (@@IDENTITY,1)


commit

delete from [user]

select * from [User]
select * from [UserAccount]
select * from [Transaction]

