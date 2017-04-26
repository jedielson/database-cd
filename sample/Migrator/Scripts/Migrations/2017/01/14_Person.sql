/****** Object:  Table [dbo].[Person]    Script Date: 23/01/2017 14:38:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Person](
	[PersonId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Cpf] [varchar](14) NULL,
	[Cnpj] [varchar](18) NULL,
	[Email] [varchar](100) NULL,
	[BirthDate] [datetime] NOT NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[ProfileUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
