
/****** Object:  Table [dbo].[DestinationActivity]    Script Date: 14/02/2017 11:02:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DestinationActivity](
	[Id] [int] IDENTITY(100000,1) NOT NULL,
	[DestinationId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[Type] [nvarchar](50) NULL,
	[AdditionalHtml] [ntext] NULL,
	[ThumbnailPath] [nvarchar](255) NULL,
 CONSTRAINT [PK_DestinationActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


