
/****** Object:  Table [dbo].[Transaction]    Script Date: 01/05/2021 15:09:16 ******/
IF OBJECT_ID(N'dbo.Transaction', N'U') IS NOT NULL  
   DROP TABLE [dbo].[Transaction];  
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 01/05/2021 15:09:16 ******/

CREATE TABLE [dbo].[Transaction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[description] [nvarchar](256) NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[balance] [decimal](10, 2) NOT NULL
) ON [PRIMARY]
GO


