ALTER TABLE [dbo].[Person] DROP CONSTRAINT [PERSON_Active_Default]
GO

-- Alter table create new camp PersonStats
ALTER TABLE Person
	DROP COLUMN Active,
	StandbyLimit
GO