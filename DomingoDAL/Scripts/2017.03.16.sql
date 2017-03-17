SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [dbo].[HtmlEmailTemplate]
           ([Name]
           ,[Alias]
           ,[Subject]
           ,[Body]
           ,[FromAddress]
           ,[FromName]
           ,[Description])
     VALUES
           ('Welcome Email'
           ,'WelcomeEmail'
           ,'Thank you for getting in touch with Travelogy Club'
           ,'<div id="DivTitle">
    <p style="font-family: calibri; font-size: 20px; padding: 0; margin: 0; color: #333333;">Thanks for calling the Travelogy Club!<br/> 	
</div>
<br/><br/>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">Hi [FirstName],</p><br/>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">Thanks for calling us, our travel advisor will call you shortly. In the meanwhile we invite you to sign up on our portal and use our FREE trip planner. Please follow the link to sign up: <a href="https://travelogyclub.com/Account/Register" style="color:#ff9900; text-decoration:underline;">Sign up and join the club, click here</a>. </p><br/>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">Like our page on <a href="https://www.facebook.com/travelogyclub/" style="color:#ff9900; text-decoration:underline;">Facebook</a> for exciting updates.</p><br/>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">Please read our <a href="http://travelogyclub.com/V2/Terms-and-Conditions.html" style="color:#ff9900; text-decoration:underline;">Terms And Conditions</a> and <a href="http://travelogyclub.com/V2/Privacy.html" style="color:#ff9900; text-decoration:underline;">Privacy Policy</a> on Travelogy Club before you get started</p>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">If you have any questions, please mail us at <a href="mailto:info@travelogyclub.com" style="color:#ff9900; text-decoration:underline;">info@travelogyclub.com</a>.</p><br/>
<p style="font-family: calibri; font-size: 16px; padding: 0; margin: 0; color: #333333;">Many Thanks,<br/>Team Travelogy</p><br/>
<p style="font-family: calibri; font-size: 12px; padding: 0; margin: 0; color: #333333;">This is an Auto-Generated email ,please do not reply to this mail.</p>'
           ,'no-reply@travelogyclub.com'
           ,'Info at Travelogy'
           ,'Welcome Email')
GO


/****** Object:  Table [dbo].[TripBookingAccommodation]    Script Date: 16/03/2017 07:30:49 ******/
ALTER TABLE [dbo].[TripBookingAccommodation] ADD [Currency] nvarchar(4)
GO
/****** Object:  Table [dbo].[TripBookingTransport]    Script Date: 16/03/2017 07:32:09 ******/
ALTER TABLE [dbo].[TripBookingTransport]  ADD [Currency] nvarchar(4)
GO

/****** Object:  View [dbo].[View_TripBookingAccommodation]    Script Date: 16/03/2017 07:39:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP VIEW [dbo].[View_TripBookingAccommodation]
GO

CREATE VIEW [dbo].[View_TripBookingAccommodation]
AS
SELECT        dbo.TripBookingAccommodation.Id, dbo.TripBookingAccommodation.AccommodationType, dbo.TripBookingAccommodation.EstimatedCost, dbo.AspNetUsers.UserName, dbo.Traveller.FirstName, 
                         dbo.Traveller.LastName, dbo.Trip.Description, dbo.Trip.StartDate AS TripStartDate, dbo.TripStep.Destination, dbo.TripStep.ShortDescription, dbo.TripStep.StartDate AS TripStepStartDate, 
                         dbo.TripBookingAccommodation.TripStepId, dbo.TripBookingAccommodation.CheckinDate, dbo.TripBookingAccommodation.Status, dbo.TripBookingAccommodation.TravellerNotes, 
                         dbo.TripBookingAccommodation.AdminNotes, dbo.TripBookingAccommodation.PropertyName, dbo.TripBookingAccommodation.PropertyAddress, dbo.TripBookingAccommodation.SpecialRequests, 
                         dbo.TripBookingAccommodation.CheckoutDate, dbo.TripBookingAccommodation.Adults, dbo.TripBookingAccommodation.Kids, dbo.TripBookingAccommodation.TripId, dbo.TripBookingAccommodation.TownOrCity, 
                         dbo.TripBookingAccommodation.Currency
FROM            dbo.TripBookingAccommodation INNER JOIN
                         dbo.Trip ON dbo.TripBookingAccommodation.TripId = dbo.Trip.Id INNER JOIN
                         dbo.TripStep ON dbo.TripBookingAccommodation.TripStepId = dbo.TripStep.Id INNER JOIN
                         dbo.AspNetUsers ON dbo.Trip.AspNetUserId = dbo.AspNetUsers.Id LEFT OUTER JOIN
                         dbo.Traveller ON dbo.Traveller.AspnetUserid = dbo.AspNetUsers.Id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TripBookingAccommodation"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Trip"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 136
               Right = 480
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TripStep"
            Begin Extent = 
               Top = 6
               Left = 518
               Bottom = 136
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AspNetUsers"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Traveller"
            Begin Extent = 
               Top = 138
               Left = 300
               Bottom = 268
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingAccommodation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingAccommodation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingAccommodation'
GO

/****** Object:  View [dbo].[View_TripBookingTransport]    Script Date: 16/03/2017 07:40:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP VIEW [dbo].[View_TripBookingTransport]
GO

CREATE VIEW [dbo].[View_TripBookingTransport]
AS
SELECT        dbo.TripBookingTransport.Id, dbo.TripBookingTransport.TransportType, dbo.TripBookingTransport.EstimatedCost, dbo.TripBookingTransport.TripId, dbo.TripBookingTransport.TripStepId, 
                         dbo.TripBookingTransport.BookingDate, dbo.TripBookingTransport.BookingStatus, dbo.TripBookingTransport.TransportFrom, dbo.TripBookingTransport.TransportTo, dbo.TripBookingTransport.AdminNotes, 
                         dbo.TripBookingTransport.TravellerNotes, dbo.TripBookingTransport.Adults, dbo.TripBookingTransport.Kids, dbo.TripBookingTransport.TravelClass, dbo.Trip.Description, dbo.Trip.StartDate AS TripStartDate, 
                         dbo.Trip.StartLocation AS TripStartLocation, dbo.Traveller.FirstName, dbo.Traveller.LastName, dbo.AspNetUsers.UserName, dbo.TripBookingTransport.TransferDetails, dbo.TripBookingTransport.Currency
FROM            dbo.TripBookingTransport INNER JOIN
                         dbo.Trip ON dbo.TripBookingTransport.TripId = dbo.Trip.Id LEFT OUTER JOIN
                         dbo.TripStep ON dbo.TripBookingTransport.TripStepId = dbo.TripStep.Id INNER JOIN
                         dbo.AspNetUsers ON dbo.Trip.AspNetUserId = dbo.AspNetUsers.Id LEFT OUTER JOIN
                         dbo.Traveller ON dbo.Traveller.AspnetUserid = dbo.AspNetUsers.Id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TripBookingTransport"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Trip"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 445
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TripStep"
            Begin Extent = 
               Top = 6
               Left = 483
               Bottom = 136
               Right = 676
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AspNetUsers"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Traveller"
            Begin Extent = 
               Top = 138
               Left = 300
               Bottom = 268
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingTransport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingTransport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_TripBookingTransport'
GO


