
/****** Object:  Procedure [GetActorById]  ******/
CREATE OR ALTER PROCEDURE GetActorById 
    (@id int)  
AS  
BEGIN    
    SELECT ID, FirstName, LastName, BirthDate FROM Actor where ID = @Id  
END
GO

/****** Object:  Procedure [GetAllActor]  ******/
CREATE OR ALTER PROCEDURE GetAllActor 
AS  
BEGIN    
    SELECT ID, FirstName, LastName, BirthDate FROM Actor 
END
GO