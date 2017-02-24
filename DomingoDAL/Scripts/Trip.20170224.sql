SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Trip] ADD  [TripCurrency] [nvarchar](4) NOT NULL DEFAULT ('GBP') ;
GO

ALTER TABLE [dbo].[Trip] ADD [EstimatedCost] [money] NULL
GO


