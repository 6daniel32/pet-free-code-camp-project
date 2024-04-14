/*

Enter custom T-SQL here that would run after SQL Server has started up. 

*/
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '$(DB_DATABASE)')
BEGIN
  CREATE DATABASE [$(DB_DATABASE)];
END;
GO
