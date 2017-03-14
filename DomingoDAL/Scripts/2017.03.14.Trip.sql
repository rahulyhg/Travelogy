
/****** Object:  Table [dbo].[Trip]    Script Date: 14/03/2017 11:17:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Trip] ADD [HomeLocation] nvarchar(50)
GO
ALTER TABLE [dbo].[Trip] ALTER COLUMN [StartLocation] nvarchar(200)
GO

ALTER TABLE [dbo].[TripStep] ADD [Duration] int
GO

ALTER TABLE [dbo].[TripStep] ADD [UserRemoved] bit
GO

