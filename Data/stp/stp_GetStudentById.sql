SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Soran Shim
-- Create date: 2022.01.25
-- Description:	Get Student data By Id
-- =============================================

IF OBJECT_ID ( 'stp_GetStudentById', 'P' ) IS NOT NULL   
    DROP PROCEDURE stp_GetStudentById;  
GO

CREATE PROCEDURE [dbo].[stp_GetStudentById]
    @Id int = null
AS
BEGIN
    SET NOCOUNT ON;

    SELECT StudentId, FullName, Email, Gender
	FROM Students
	WHERE StudentId = @Id
END
GO
