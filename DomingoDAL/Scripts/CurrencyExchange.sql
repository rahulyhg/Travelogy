
/****** Object:  Table [dbo].[CurrencyExchange]    Script Date: 28/02/2017 21:03:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CurrencyExchange](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyFrom] [nvarchar](4) NOT NULL,
	[CurrencyTo] [nvarchar](4) NOT NULL,
	[XchangeRate] [decimal](18, 6) NOT NULL,
	[DateOfUpdate] [date] NOT NULL,
 CONSTRAINT [PK_CurrencyExchange] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO