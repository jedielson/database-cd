/****** Object:  Table [dbo].[Program]    Script Date: 19/01/2017 11:00:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Program](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[GoalSucess] [int] NOT NULL,
	[MaximumAttempts] [int] NOT NULL,
	[MaximumDays] [int] NOT NULL,
	[InitialDate] [datetime] NOT NULL,
	[MaximumTimeRegular] [varchar](100) NOT NULL,
	[MaximumTimeIdeal] [varchar](100) NOT NULL,
	[LoyaltyPatientAttendant] [bit] NOT NULL,
 CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
