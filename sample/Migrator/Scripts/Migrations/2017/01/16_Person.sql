-- Alter table create new camp PersonStats
ALTER TABLE Person
	ADD Active bit not null,
	StandbyLimit datetime null
GO

ALTER TABLE [dbo].[Person] ADD CONSTRAINT PERSON_Active_Default  DEFAULT ((1)) FOR [Active]
GO